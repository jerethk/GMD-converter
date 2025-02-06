using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMD_converter
{
    public class MidiEvent
    {
        public int DeltaTime { get; set; }

        public byte[] Data { get; set; }
    }

    public class MetaEvent : MidiEvent
    {
        // Format is 0XFF, Type, Length, Data

        public byte Start { get; set; } // 0xFF

        public byte Type { get; set; }

        public int Length { get; set; }
    }

    public class SysExEvent : MidiEvent
    {
        // Format is 0XF0 | 0XF7, Length, Data, End

        public byte Start { get; set; } // 0xF0 or 0xF7

        public int Length { get; set; }

        public byte End { get; set; } // 0xF7
    }
}
