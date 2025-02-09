using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMD_converter
{
    public partial class TrackInfoWindow : Form
    {
        private static decimal defaultMilliSecondsPerBeat = 500m;
        private decimal ticksPerBeat;
        private List<TrackInternal> tracks;

        #region Constructors

        private TrackInfoWindow()
        {
            InitializeComponent();
        }

        public TrackInfoWindow(MTrkChunk[] tracks, int division)
            : this()
        {
            this.ticksPerBeat = division;
            this.tracks = tracks.Select(track => this.LoadTrack(track)).ToList();
        }

        #endregion

        private TrackInternal LoadTrack(MTrkChunk track)
        {
            var trackInternal = new TrackInternal();
            var dataPos = 0;
            var absTime = 0;
            decimal tempo = defaultMilliSecondsPerBeat;

            while (true)
            {
                // Get the next event from the data //

                // Delta time
                var (deltaTime, count) = MidiEvent.GetLength(track.data, dataPos);
                dataPos += count;

                var nextByte = track.data[dataPos];
                switch (nextByte)
                {
                    // Meta event
                    case 0XFF:
                        var mEvt = new MetaEvent();
                        mEvt.DeltaTime = deltaTime;
                        absTime += this.ToAbsoluteTime(deltaTime, tempo);
                        mEvt.AbsTimeMsec = absTime;

                        // meta event type
                        dataPos++;
                        mEvt.Type = track.data[dataPos];

                        // meta event length
                        dataPos++;
                        var (lenM, countM) = MidiEvent.GetLength(track.data, dataPos);
                        mEvt.Length = lenM;

                        // meta event data
                        dataPos += countM;
                        mEvt.Data = new byte[lenM];
                        Array.Copy(track.data, dataPos, mEvt.Data, 0, mEvt.Length);

                        dataPos += mEvt.Length;
                        trackInternal.MidiEvents.Add(mEvt);

                        // change tempo if set tempo event
                        if (mEvt.Type == 0x51)
                        {
                            decimal microsecPerBeat = (mEvt.Data[0] << 16) + (mEvt.Data[1] << 8) + (mEvt.Data[2]);
                            tempo = microsecPerBeat / 1000m;
                        }

                        break;

                    // SysEx event
                    case 0xF0:
                    case 0xF7:
                        var sEvt = new SysExEvent();
                        sEvt.DeltaTime = deltaTime;
                        absTime += this.ToAbsoluteTime(deltaTime, tempo);
                        sEvt.AbsTimeMsec = absTime;

                        // SysEx event length
                        dataPos++;
                        var (lenS, countS) = MidiEvent.GetLength(track.data, dataPos);
                        sEvt.Length = lenS - 1;     // exclude EOX byte

                        // SysEx event data (exclude the EOX byte, 0xF7)
                        dataPos += countS;
                        sEvt.Data = new byte[lenS - 1];
                        Array.Copy(track.data, dataPos, sEvt.Data, 0, sEvt.Length);

                        // End byte, should be 0xF7
                        dataPos += sEvt.Length;
                        sEvt.EOX = track.data[dataPos];
                        if (sEvt.EOX != 0xF7)
                        {
                            MessageBox.Show($"Error: failed to find EOX marker at position 0x{Convert.ToString(dataPos, 16)}");
                            throw new Exception("Failed to find EOX");
                        }
                        dataPos++;

                        trackInternal.MidiEvents.Add(sEvt);


                        //var errorMsg = $"Unexpected end of track at position 0x{Convert.ToString(dataPos, 16)}";
                        //trackInternal.Errors.Add(errorMsg);

                        break;

                    // Normal Midi event
                    default:
                        var evt = new MidiEvent();
                        evt.DeltaTime = deltaTime;
                        absTime += this.ToAbsoluteTime(deltaTime, tempo);
                        evt.AbsTimeMsec = absTime;

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
                            default:
                                var errorMsg = $"Unknown event type 0x{Convert.ToString(nextByte, 16)} at position 0x{Convert.ToString(dataPos, 16)}";
                                trackInternal.Errors.Add(errorMsg);
                                break;
                        }

                        evt.Data = new byte[msgLength];
                        Array.Copy(track.data, dataPos, evt.Data, 0, msgLength);
                        dataPos += msgLength;

                        trackInternal.MidiEvents.Add(evt);
                        break;
                }

                // Hack to account for dodgy track endings i.e. STALK-04
                if (dataPos + 2 >= track.data.Length)
                {
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

            this.numericTrack.Maximum = this.tracks.Count;
            this.DisplayData((int)numericTrack.Value - 1);
        }

        private void numericTrack_ValueChanged(object sender, EventArgs e)
        {
            this.DisplayData((int)numericTrack.Value - 1);
        }

        private void DisplayData(int trackNum)
        {
            if (this.tracks.Count <= trackNum) { return; }

            listBoxMetaEvents.Items.Clear();
            listBoxSysExEvents.Items.Clear();
            listBoxErrors.Items.Clear();

            var track = this.tracks[trackNum];
            var encoding = new UTF8Encoding();

            if (track.MetaEvents != null)
            {
                foreach (var evt in track.MetaEvents)
                {
                    var time = this.GetTimeString(evt.AbsTimeMsec);
                    var str = $"Time {time} , Type {Convert.ToString(evt.Type, 16)} , {BitConverter.ToString(evt.Data)}";

                    if (evt.Type >= 0x01 && evt.Type <= 0x07)
                    {
                        str += $" , {encoding.GetString(evt.Data).Trim('\0')}";
                    }

                    listBoxMetaEvents.Items.Add(str);
                }
            }

            if (track.SysExEvents != null)
            {
                foreach (var evt in track.SysExEvents)
                {
                    var time = this.GetTimeString(evt.AbsTimeMsec);

                    // Strip off the manufacturer & model bytes and the EOX byte (0xF7)
                    var msg = new byte[evt.Data.Length - 2];
                    Array.Copy(evt.Data, 2, msg, 0, msg.Length);
                    var msgString = encoding.GetString(msg).Trim('\0');
                    listBoxSysExEvents.Items.Add($"Time {time} , {msgString} , {BitConverter.ToString(evt.Data)}");
                }
            }

            if (track.Errors != null)
            {
                foreach (var error in track.Errors)
                {
                    listBoxErrors.Items.Add(error);
                }
            }
        }

        private int ToAbsoluteTime(int ticks, decimal millisecPerBeat)
        {
            decimal beats = ticks / this.ticksPerBeat;
            decimal millisec = beats * millisecPerBeat;
            return (int)millisec;
        }

        private string GetTimeString(int millisec)
        {
            decimal minutes = Math.Floor(millisec / 60000m);
            decimal secondsRemaining = Math.Floor((millisec % 60000m) / 1000);
            decimal milliSecondsReamining = millisec % 1000m;
            return $"{minutes}:{secondsRemaining}:{milliSecondsReamining}";
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            var dialogResult = this.saveCsvDialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                using var writer = new StreamWriter(File.Create(this.saveCsvDialog.FileName));

                writer.WriteLine($"Track {this.numericTrack.Value}");
                writer.WriteLine(string.Empty);
                writer.WriteLine("Meta Events");
                foreach (var evt in this.listBoxMetaEvents.Items)
                {
                    writer.WriteLine(evt as string);
                }

                writer.WriteLine(string.Empty);
                writer.WriteLine("System Exclusive Events");
                foreach (var evt in this.listBoxSysExEvents.Items)
                {
                    writer.WriteLine(evt as string);
                }
            }
        }
    }
}
