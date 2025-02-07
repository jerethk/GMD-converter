using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMD_converter
{
    // Internal representation of a Midi track
    public class TrackInternal
    {
        public List<MidiEvent> MidiEvents;
        
        public List<MetaEvent> MetaEvents
        {
            get
            {
                return this.MidiEvents
                    .Where(e => e is MetaEvent)
                    .Select(e => e as MetaEvent)
                    .ToList();
            }
        }

        public List<SysExEvent> SysExEvents
        {
            get
            {
                return this.MidiEvents
                    .Where(e => e is SysExEvent)
                    .Select(e => e as SysExEvent)
                    .ToList();
            }
        }

        public List<string> Errors { get; set; }

        public TrackInternal()
        {
            this.MidiEvents = new();
            this.Errors = new();
        }
    }
}
