using System;

namespace GMD_converter
{
    public class MidiChunk
    {
        public int chunkType { get; set; }
        public int chunkSize { get; set; }          // excluding header
    }

    public sealed class MDpgChunk : MidiChunk
    {
        // chunkType 'MDpg'  0x4d 44 70 67
        public byte[] content { get; set; }
    }

    public sealed class MThdChunk : MidiChunk
    {
        // chunkType 'MThd'  0x 4d 54 68 64

        public short format { get; set; }
        public short nTracks { get; set; }
        public short division { get; set; }
    }

    public sealed class MTrkChunk : MidiChunk
    {
        // chunkType 'MTrk'  0x4d 54 72 6b
        public byte[] data { get; set; }
    }
}
