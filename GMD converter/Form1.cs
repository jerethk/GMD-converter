using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GMD_converter
{
    public partial class Form1 : Form
    {
        private GMDFile GMID;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadGMD_Click(object sender, EventArgs e)
        {
            var dialogResult = openGMDDg.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                GMDFile g = new GMDFile();
                
                if (g.loadFile(openGMDDg.FileName))
                {
                    this.GMID = g;

                    labelGMDInfo.Text = openGMDDg.FileName;
                    // labelGMDInfo.Text += $"\nGMD size {GMID.fileSize}";
                    labelGMDInfo.Text += $"\nMDPg size {GMID.MDpg.chunkSize}";
                    // labelGMDInfo.Text += $"\nMThd size {GMID.MThd.chunkSize}";
                    labelGMDInfo.Text += $"\nMIDI format {GMID.MThd.format}";
                    labelGMDInfo.Text += $"\nNumber of tracks {GMID.MThd.nTracks}";
                    labelGMDInfo.Text += $"\nDivision {GMID.MThd.division}";
                    labelGMDInfo.Text += $"\n";

                    int tracknum = 0;
                    foreach (MTrk track in GMID.tracks)
                    {
                        tracknum++;
                        labelGMDInfo.Text += $"\nTrack {tracknum} length {track.trkLength}";
                    }

                    btnExport.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Error loading GMD", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string filename = Path.GetFileNameWithoutExtension(openGMDDg.FileName);
            saveMIDDg.FileName = filename;
            
            saveMIDDg.ShowDialog();
        }

        private void saveMIDDg_FileOk(object sender, CancelEventArgs e)
        {
            bool isSuccess = false;
            
            if (radioBMultitrack.Checked)
            {
                isSuccess = GMID.exportMIDI2(saveMIDDg.FileName);
            }
            else if (radioBSingletracks.Checked)
            {
                isSuccess = GMID.exportMIDI0(saveMIDDg.FileName);
            }

            if (isSuccess)
            {
                MessageBox.Show("Successfully exported");
            }
            else
            {
                MessageBox.Show("Failed to export");
            }

        }
    }
}
