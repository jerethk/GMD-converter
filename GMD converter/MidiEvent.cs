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

        
        /// <summary>
        /// Parses MIDI variable length values.
        /// These are between 1 and 4 bytes.
        /// The first 7 bits of each byte are used to hold value.
        /// The 8th bit is cleared to indicate the final byte.
        /// </summary>
        /// <param name="buf">Bytes</param>
        /// <returns>Tuple: length, number of bytes used to represent the length</returns>
        public static (int length, int byteCount) GetLength(byte[] buf)
        {
            if (buf.Length == 0)
            {
                return (0, 0);
            }

            var bufSize = Math.Min(buf.Length, 4);
            int byteCount = 0;
            int length = 0;
            
            for (int i = 0; i < bufSize; i++)
            {
                var b = buf[i];
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

        public byte Start { get; set; } // 0xFF

        public byte Type { get; set; }

        public int Length { get; set; }
    }

    public sealed class SysExEvent : MidiEvent
    {
        // Format is 0XF0 | 0XF7, Length, Data, End

        public byte Start { get; set; } // 0xF0 or 0xF7

        public int Length { get; set; }

        public byte End { get; set; } // 0xF7
    }
}
