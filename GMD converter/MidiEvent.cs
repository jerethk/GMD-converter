using System;
using System.Linq;

namespace GMD_converter
{
    public class MidiEvent
    {
        public int DeltaTime { get; set; }  // in ticks

        public int AbsTimeMsec { get; set; }    // milliseconds, calculated

        public byte[] Data { get; set; }

        
        /// <summary>
        /// Parses MIDI variable length values.
        /// These are between 1 and 4 bytes.
        /// The first 7 bits of each byte are used to hold value.
        /// The 8th bit is 0 to indicate the final byte else it is 1.
        /// </summary>
        /// <param name="buf">Bytes</param>
        /// <returns>Tuple: length, number of bytes used to represent the length</returns>
        public static (int length, int byteCount) GetLength(byte[] buf, int position)
        {
            if (buf.Length == 0)
            {
                return (0, 0);
            }

            // Position cannot be beyond end of array
            if (position >= buf.Length)
            {
                return (0, 0);
            }

            int byteCount = 0;
            int length = 0;
            
            for (int i = 0; i < 4; i++)
            {
                var b = buf[position + i];
                byteCount += 1;
                length <<= 7;
                length += (b & 0x7f);

                if ((b & 0x80) == 0)
                {
                    break;
                }
            }
            
            return (length, byteCount);
        }
    }

    public sealed class MetaEvent : MidiEvent
    {
        // Format is 0XFF, Type, Length, Data

        public byte Start => 0xFF;

        public byte Type { get; set; }

        public int Length { get; set; }
    }

    public sealed class SysExEvent : MidiEvent
    {
        // Format is 0XF0 | 0XF7, Length, Data, End
        // Data starts with manufacturer id (and model?) byte

        public byte Start { get; set; } // 0xF0 or 0xF7

        public int Length { get; set; }

        public byte EOX { get; set; } // 0xF7
    }
}
