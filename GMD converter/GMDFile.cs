using System;
using System.Collections.Generic;
using System.IO;

namespace GMD_converter
{
    public class GMDFile
    {
        public int fileType { get; set; }           // 'MIDI' 0x4d 49 44 49   or 'GMD '
        public int fileSize { get; set; }           // excluding header

        public MDpgChunk MDpg { get; set; }         // This seems to have no purpose in DF
        public MThdChunk MThd { get; set; }
        public MTrkChunk[] tracks { get; set; }

        private static int ReverseInt32Endian(int input)
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

        private static short ReverseInt16Endian(short input)
        {
            byte[] inputArray = BitConverter.GetBytes(input);
            byte[] outputArray = new byte[2];

            outputArray[0] = inputArray[1];
            outputArray[1] = inputArray[0];

            short output = BitConverter.ToInt16(outputArray, 0);
            return output;
        }

        public bool LoadFile(string filename, out string errorString)
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
                        
                    this.fileSize = ReverseInt32Endian(GMDReader.ReadInt32());

                    bool isMThd = false;
                    while (!isMThd)
                    {
                        int nextChunkType = GMDReader.ReadInt32();

                        if (nextChunkType == 0x6770444d)        // MDpg
                        {
                            // MDpg chunk
                            this.MDpg = new MDpgChunk();
                            this.MDpg.chunkType = nextChunkType;
                            this.MDpg.chunkSize = ReverseInt32Endian(GMDReader.ReadInt32());

                            this.MDpg.content = GMDReader.ReadBytes(MDpg.chunkSize);
                        }
                        else if (nextChunkType == 0x6468544d)   // MThd
                        {
                            isMThd = true;
                        }
                        else
                        {
                            // Other chunk type - skip over
                            int otherChunkSize = ReverseInt32Endian(GMDReader.ReadInt32());
                            GMDReader.ReadBytes(otherChunkSize);
                        }
                    }

                    // MThd chunk 
                    this.MThd = new MThdChunk();
                    this.MThd.chunkType = 0x6468544d;

                    this.MThd.chunkSize = ReverseInt32Endian(GMDReader.ReadInt32());
                    this.MThd.format = ReverseInt16Endian(GMDReader.ReadInt16());
                    this.MThd.nTracks = ReverseInt16Endian(GMDReader.ReadInt16());
                    this.MThd.division = ReverseInt16Endian(GMDReader.ReadInt16());

                    // Tracks
                    this.tracks = new MTrkChunk[MThd.nTracks];

                    for (int t = 0; t < MThd.nTracks; t++)
                    {
                        this.tracks[t] = new MTrkChunk();
                        this.tracks[t].chunkType = GMDReader.ReadInt32();

                        if (this.tracks[t].chunkType != 0x6b72544d)
                        {
                            errorString = "Error loading MTrk";
                            return false;
                        }
                        else
                        {
                            this.tracks[t].chunkSize = ReverseInt32Endian(GMDReader.ReadInt32());
                            this.tracks[t].data = GMDReader.ReadBytes(this.tracks[t].chunkSize);
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
        public bool ExportMIDI2(string fname)
        {
            using (BinaryWriter MIDWriter = new BinaryWriter(new FileStream(fname, FileMode.Create))) 
            {
                short MIDIformat = 02;
                
                try
                {
                    MIDWriter.Write(MThd.chunkType);
                    MIDWriter.Write(ReverseInt32Endian(MThd.chunkSize));
                    MIDWriter.Write(ReverseInt16Endian(MIDIformat));
                    MIDWriter.Write(ReverseInt16Endian(MThd.nTracks));
                    MIDWriter.Write(ReverseInt16Endian(MThd.division));

                    // tracks
                    foreach (MTrkChunk t in this.tracks)
                    {
                        MIDWriter.Write(t.chunkType);
                        MIDWriter.Write(ReverseInt32Endian(t.chunkSize));
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
        public bool ExportMIDI0(string fname)
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
                        MIDWriter.Write(ReverseInt32Endian(MThd.chunkSize));
                        MIDWriter.Write(ReverseInt16Endian(MIDIformat));
                        MIDWriter.Write(ReverseInt16Endian(1));
                        MIDWriter.Write(ReverseInt16Endian(MThd.division));

                        MIDWriter.Write(tracks[t].chunkType);
                        MIDWriter.Write(ReverseInt32Endian(tracks[t].chunkSize));
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
}
