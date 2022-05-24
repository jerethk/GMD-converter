﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GMD_converter
{
    class GMDFile
    {
        public int      fileType { get; set; }           // 'MIDI' 0x4d 49 44 49
        public int      fileSize { get; set; }            // excluding header

        public MDpgChunk MDpg { get; set; }
        public MThdChunk MThd { get; set; }
        public MTrk[]    tracks { get; set; }

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

        public bool loadFile(string filename)
        {
            using (BinaryReader GMDReader = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                try
                {
                    this.fileType = GMDReader.ReadInt32();
                    if (this.fileType != 0x4944494D) return false;    // wrong file type
                    this.fileSize = reverseInt32Endian(GMDReader.ReadInt32());

                    // MDpg chunk should come first
                    MDpg = new MDpgChunk();
                    MDpg.chunkType = GMDReader.ReadInt32();

                    if (MDpg.chunkType == 0x6770444d)
                    {
                        MDpg.chunkSize = reverseInt32Endian(GMDReader.ReadInt32());

                        MDpg.content = new byte[MDpg.chunkSize];
                        MDpg.content = GMDReader.ReadBytes(MDpg.chunkSize);
                    }
                    else 
                        return false;

                    // MThd chunk next
                    MThd = new MThdChunk();
                    MThd.chunkType = GMDReader.ReadInt32();

                    if (MThd.chunkType == 0x6468544d)
                    {
                        MThd.chunkSize = reverseInt32Endian(GMDReader.ReadInt32());
                        MThd.format = reverseInt16Endian(GMDReader.ReadInt16());
                        MThd.nTracks = reverseInt16Endian(GMDReader.ReadInt16());
                        MThd.division = reverseInt16Endian(GMDReader.ReadInt16());
                    }
                    else 
                        return false;

                    // Tracks
                    this.tracks = new MTrk[MThd.nTracks];
                    
                    for (int t = 0; t < MThd.nTracks; t++)
                    {
                        this.tracks[t] = new MTrk();
                        this.tracks[t].heading = GMDReader.ReadInt32();

                        if (this.tracks[t].heading != 0x6b72544d)
                        {
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
                    string errorString = e.Message;
                    return false;
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