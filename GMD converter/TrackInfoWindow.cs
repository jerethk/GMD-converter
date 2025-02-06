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

            public TrackInternal()
            {
                this.MidiEvents = new();
                this.MetaEvents = new();
                this.SysExEvents = new();
            }
        }

        private List<TrackInternal> tracks;

        #region Constructors

        private TrackInfoWindow()
        {
            InitializeComponent();
        }

        public TrackInfoWindow(MTrkChunk[] tracks)
            : this()
        {
            this.tracks = tracks.Select(track => this.LoadTrack(track)).ToList();
        }

        #endregion

        private TrackInternal LoadTrack(MTrkChunk track)
        {
            var trackInternal = new TrackInternal();
            var finished = false;
            var dataPos = 0;

            while (!finished)
            {
                if (dataPos + 4 >= track.data.Length)
                {
                    break;
                }

                // Get the next event from the data
                var nextFour = new byte[4];
                Array.Copy(track.data, dataPos, nextFour, 0, 4);

                var (deltaTime, count) = MidiEvent.GetLength(nextFour);
                dataPos += count;

                var nextByte = track.data[dataPos];
                switch (nextByte)
                {
                    // Meta event
                    case 0XFF:
                        var mEvt = new MetaEvent();
                        mEvt.DeltaTime = deltaTime;

                        // meta event type
                        dataPos++;
                        mEvt.Type = track.data[dataPos];

                        // meta event length
                        dataPos++;
                        Array.Copy(track.data, dataPos, nextFour, 0, 4);
                        var (lenM, countM) = MidiEvent.GetLength(nextFour);
                        mEvt.Length = lenM;

                        // meta event data
                        dataPos += countM;
                        mEvt.Data = new byte[lenM];
                        Array.Copy(track.data, dataPos, mEvt.Data, 0, mEvt.Length);

                        dataPos += mEvt.Length;
                        trackInternal.MetaEvents.Add(mEvt);
                        break;

                    // SysEx event
                    case 0xF0:
                    case 0xF7:
                        var sEvt = new SysExEvent();
                        sEvt.DeltaTime = deltaTime;

                        // SysEx event length
                        dataPos++;
                        Array.Copy(track.data, dataPos, nextFour, 0, 4);
                        var (lenS, countS) = MidiEvent.GetLength(nextFour);
                        sEvt.Length = lenS;

                        // SysEx event data
                        dataPos += countS;
                        sEvt.Data = new byte[lenS];
                        Array.Copy(track.data, dataPos, sEvt.Data, 0, sEvt.Length);

                        // End byte, should be 0xF7
                        dataPos += sEvt.Length;
                        if (track.data[dataPos - 1] != 0xF7)
                        {
                            throw new Exception("Unexpected");
                        }

                        trackInternal.SysExEvents.Add(sEvt);
                        break;

                    // Normal Midi event
                    default:
                        var evt = new MidiEvent();
                        evt.DeltaTime = deltaTime;

                        // Get the message type (80 - E0)
                        var t = nextByte & 0xF0;
                        var msgLength = 0;

                        switch (t)
                        {
                            case 0x80:  // Note off
                            case 0x90:  // Note on
                            case 0xA0:  // Polyphonic pressure
                            case 0xB0:  // Controller
                            case 0xE0:  // Pitch bend
                                msgLength = 3;
                                break;

                            case 0xC0:  // Instrument change
                            case 0xD0:  // Channel pressure
                                msgLength = 2;
                                break;
                        }

                        evt.Data = new byte[msgLength];
                        Array.Copy(track.data, dataPos, evt.Data, 0, msgLength);
                        dataPos += msgLength;

                        trackInternal.MidiEvents.Add(evt);
                        break;
                }
            }

            return trackInternal;
        }

        private void TrackInfoWindow_Shown(object sender, EventArgs e)
        {
            if (this.tracks == null || this.tracks.Count == 0)
            {
                return;
            }

            this.numericTrack.Maximum = this.tracks.Count - 1;
            this.DisplayData((int)numericTrack.Value);
        }

        private void numericTrack_ValueChanged(object sender, EventArgs e)
        {
            this.DisplayData((int)numericTrack.Value);
        }

        private void DisplayData(int trackNum)
        {
            if (this.tracks.Count <= trackNum) { return; }
            
            listBoxMetaEvents.Items.Clear();
            listBoxSysExEvents.Items.Clear();
            
            var track = this.tracks[trackNum];
            var encoding = new UTF8Encoding();

            if (track.MetaEvents != null)
            {
                foreach (var evt in track.MetaEvents)
                {
                    listBoxMetaEvents.Items.Add($"Time {evt.DeltaTime} , Type {evt.Type} , {BitConverter.ToString(evt.Data)} , {encoding.GetString(evt.Data)}");
                }
            }

            if (track.SysExEvents != null)
            {
                foreach (var evt in track.SysExEvents)
                {
                    listBoxSysExEvents.Items.Add($"Time {evt.DeltaTime} , {BitConverter.ToString(evt.Data)} , {encoding.GetString(evt.Data)}");
                }
            }
        }
    }
}
