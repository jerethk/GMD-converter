using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMD_converter
{
    public partial class TrackInfoWindow : Form
    {
        struct TrackInternal
        {
            public List<MidiEvent> MidiEvents;
            public List<MetaEvent> MetaEvents;
            public List<SysExEvent> SysExEvents;
        }

        private List<TrackInternal> tracks;
        
        private TrackInfoWindow()
        {
            InitializeComponent();
        }

        public TrackInfoWindow(MTrkChunk[] tracks)
            : this()
        {
            foreach (var track in tracks)
            {

            }
        }
    }
}
