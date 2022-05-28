using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GMD_converter
{
    class GMDFile
    {
        public int fileType { get; set; }           // 'MIDI' 0x4d 49 44 49   or 'GMD '
        public int fileSize { get; set; }            // excluding header

        public MDpgChunk MDpg { get; set; }
        public MThdChunk MThd { get; set; }
        public MTrk[] tracks { get; set; }

        private static int reverseInt32Endian(int input)
        {
            byte[] inputArray = BitConverter.GetBytes(input);
            byte[] outputArray = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                outputArray[i] = inputArray[3 - i];
            }

            int output = BitConverter.ToInt32(outputArray, 0);
            return output;
        }

        private static short reverseInt16Endian(short input)
        {
            byte[] inputArray = BitConverter.GetBytes(input);
            byte[] outputArray = new byte[2];

            outputArray[0] = inputArray[1];
            outputArray[1] = inputArray[0];

            short output = BitConverter.ToInt16(outputArray, 0);
            return output;
        }

        public bool loadFile(string filename, out string errorString)
        {
            using (BinaryReader GMDReader = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                errorString = "";
                
                try
                {
                    // GMD Header                    
                    this.fileType = GMDReader.ReadInt32();
                    if (this.fileType != 0x4944494D && this.fileType != 0x20444d47)
                    {
                        errorString = "Not a valid GMD file";
                        return false;    // wrong file type
                    }
                        
                    this.fileSize = reverseInt32Endian(GMDReader.ReadInt32());

                    bool isMThd = false;
                    while (!isMThd)
                    {
                        int nextChunkType = GMDReader.ReadInt32();

                        if (nextChunkType == 0x6770444d)        // MDpg
                        {
                            // MDpg chunk
                            MDpg = new MDpgChunk();
                            MDpg.chunkType = nextChunkType;
                            MDpg.chunkSize = reverseInt32Endian(GMDReader.ReadInt32());

                            MDpg.content = new byte[MDpg.chunkSize];
                            MDpg.content = GMDReader.ReadBytes(MDpg.chunkSize);
                        }
                        else if (nextChunkType == 0x6468544d)   // MThd
                        {
                            isMThd = true;
                        }
                        else
                        {
                            // Other chunk type - skip over
                            int otherChunkSize = reverseInt32Endian(GMDReader.ReadInt32());
                            GMDReader.ReadBytes(otherChunkSize);
                        }
                    }

                    // MThd chunk 
                    MThd = new MThdChunk();
                    MThd.chunkType = 0x6468544d;

                    MThd.chunkSize = reverseInt32Endian(GMDReader.ReadInt32());
                    MThd.format = reverseInt16Endian(GMDReader.ReadInt16());
                    MThd.nTracks = reverseInt16Endian(GMDReader.ReadInt16());
                    MThd.division = reverseInt16Endian(GMDReader.ReadInt16());

                    // Tracks
                    this.tracks = new MTrk[MThd.nTracks];

                    for (int t = 0; t < MThd.nTracks; t++)
                    {
                        this.tracks[t] = new MTrk();
                        this.tracks[t].heading = GMDReader.ReadInt32();

                        if (this.tracks[t].heading != 0x6b72544d)
                        {
                            errorString = "Error loading MTrk";
                            return false;
                        }
                        else
                        {
                            this.tracks[t].trkLength = reverseInt32Endian(GMDReader.ReadInt32());
                            this.tracks[t].data = new byte[this.tracks[t].trkLength];
                            this.tracks[t].data = GMDReader.ReadBytes(this.tracks[t].trkLength);
                        }
                    }

                    GMDReader.Close();
                }
                catch (IOException e)
                {
                    errorString = "Exception: " + e.Message;
                    return false;
                }
            }

            return true;
        }

        // Export to single format 2 MIDI file with multiple tracks
        public bool exportMIDI2(string fname)
        {
            using (BinaryWriter MIDWriter = new BinaryWriter(new FileStream(fname, FileMode.Create))) 
            {
                short MIDIformat = 02;
                
                try
                {
                    MIDWriter.Write(MThd.chunkType);
                    MIDWriter.Write(reverseInt32Endian(MThd.chunkSize));
                    MIDWriter.Write(reverseInt16Endian(MIDIformat));
                    MIDWriter.Write(reverseInt16Endian(MThd.nTracks));
                    MIDWriter.Write(reverseInt16Endian(MThd.division));

                    // tracks
                    foreach (MTrk t in this.tracks)
                    {
                        MIDWriter.Write(t.heading);
                        MIDWriter.Write(reverseInt32Endian(t.trkLength));
                        MIDWriter.Write(t.data);
                    }
                }
                catch (IOException e)
                {
                    string errorString = e.Message;
                    return false;
                }

                MIDWriter.Close();
            }

            return true;
        }

        // Export to multiple format 0 MIDI files
        public bool exportMIDI0(string fname)
        {
            string path = Path.GetDirectoryName(fname) + "\\" + Path.GetFileNameWithoutExtension(fname);
            short MIDIformat = 00;
            
            for (int t = 0; t < tracks.Length; t++)
            {
                string filename = path + '_' + t + ".MID";

                using (BinaryWriter MIDWriter = new BinaryWriter(new FileStream(filename, FileMode.Create)))
                {
                    try
                    {
                        MIDWriter.Write(MThd.chunkType);
                        MIDWriter.Write(reverseInt32Endian(MThd.chunkSize));
                        MIDWriter.Write(reverseInt16Endian(MIDIformat));
                        MIDWriter.Write(reverseInt16Endian(1));
                        MIDWriter.Write(reverseInt16Endian(MThd.division));

                        MIDWriter.Write(tracks[t].heading);
                        MIDWriter.Write(reverseInt32Endian(tracks[t].trkLength));
                        MIDWriter.Write(tracks[t].data);
                    }
                    catch (IOException e)
                    {
                        string errorString = e.Message;
                        return false;
                    }

                    MIDWriter.Close();
                }
            }

            return true;
        }

    }

    class MDpgChunk
    {
        public int      chunkType { get; set; }          // 'MDpg'  0x4d 44 70 67
        public int      chunkSize { get; set; }          // excluding header
        public byte[]   content { get; set; }
    }

    class MThdChunk
    {
        public int      chunkType { get; set; }          // 'MThd' 0x 4d 54 68 64
        public int      chunkSize { get; set; }          // excluding header

        public short    format { get; set; }
        public short    nTracks { get; set; }
        public short    division { get; set; }
    }

    class MTrk
    {
        public int      heading { get; set; }           // 'MTrk' 0x4d 54 72 6b
        public int      trkLength { get; set; }
        public byte[]   data { get; set; }
    }
}
