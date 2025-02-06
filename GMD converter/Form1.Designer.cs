
namespace GMD_converter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openGMDDg = new System.Windows.Forms.OpenFileDialog();
            saveMIDDg = new System.Windows.Forms.SaveFileDialog();
            btnLoadGMD = new System.Windows.Forms.Button();
            labelGMDInfo = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            radioBMultitrack = new System.Windows.Forms.RadioButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioBSingletracks = new System.Windows.Forms.RadioButton();
            btnExport = new System.Windows.Forms.Button();
            btnInfo = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // openGMDDg
            // 
            openGMDDg.DefaultExt = "gmd";
            openGMDDg.Filter = "GMD Files|*.GMD;*.GMID";
            openGMDDg.Title = "Load GMD file";
            // 
            // saveMIDDg
            // 
            saveMIDDg.DefaultExt = "mid";
            saveMIDDg.Filter = "MIDI files|*.mid";
            saveMIDDg.Title = "Save MIDI ";
            saveMIDDg.FileOk += saveMIDDg_FileOk;
            // 
            // btnLoadGMD
            // 
            btnLoadGMD.Location = new System.Drawing.Point(12, 22);
            btnLoadGMD.Name = "btnLoadGMD";
            btnLoadGMD.Size = new System.Drawing.Size(103, 37);
            btnLoadGMD.TabIndex = 0;
            btnLoadGMD.Text = "Load GMD";
            btnLoadGMD.UseVisualStyleBackColor = true;
            btnLoadGMD.Click += btnLoadGMD_Click;
            // 
            // labelGMDInfo
            // 
            labelGMDInfo.AutoSize = true;
            labelGMDInfo.Location = new System.Drawing.Point(12, 188);
            labelGMDInfo.Name = "labelGMDInfo";
            labelGMDInfo.Size = new System.Drawing.Size(92, 15);
            labelGMDInfo.TabIndex = 1;
            labelGMDInfo.Text = "No GMD loaded";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 160);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(103, 15);
            label1.TabIndex = 2;
            label1.Text = "GMD information:";
            // 
            // radioBMultitrack
            // 
            radioBMultitrack.AutoSize = true;
            radioBMultitrack.Checked = true;
            radioBMultitrack.Location = new System.Drawing.Point(7, 32);
            radioBMultitrack.Name = "radioBMultitrack";
            radioBMultitrack.Size = new System.Drawing.Size(187, 19);
            radioBMultitrack.TabIndex = 3;
            radioBMultitrack.TabStop = true;
            radioBMultitrack.Text = "Format 2 MIDI (multi track file)";
            radioBMultitrack.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioBSingletracks);
            groupBox1.Controls.Add(radioBMultitrack);
            groupBox1.Location = new System.Drawing.Point(171, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(200, 113);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Export options";
            // 
            // radioBSingletracks
            // 
            radioBSingletracks.AutoSize = true;
            radioBSingletracks.Location = new System.Drawing.Point(6, 69);
            radioBSingletracks.Name = "radioBSingletracks";
            radioBSingletracks.Size = new System.Drawing.Size(179, 19);
            radioBSingletracks.TabIndex = 5;
            radioBSingletracks.Text = "Format 0 MIDI (multiple files)";
            radioBSingletracks.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            btnExport.Enabled = true;
            btnExport.Location = new System.Drawing.Point(424, 22);
            btnExport.Name = "btnExport";
            btnExport.Size = new System.Drawing.Size(103, 37);
            btnExport.TabIndex = 5;
            btnExport.Text = "Export to MIDI";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btnInfo
            // 
            btnInfo.Enabled = true;
            btnInfo.Location = new System.Drawing.Point(424, 73);
            btnInfo.Name = "btnInfo";
            btnInfo.Size = new System.Drawing.Size(103, 37);
            btnInfo.TabIndex = 6;
            btnInfo.Text = "Track Info";
            btnInfo.UseVisualStyleBackColor = true;
            btnInfo.Click += btnInfo_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(591, 407);
            Controls.Add(btnInfo);
            Controls.Add(btnExport);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Controls.Add(labelGMDInfo);
            Controls.Add(btnLoadGMD);
            MinimumSize = new System.Drawing.Size(600, 440);
            Name = "Form1";
            Text = "GMD to MID converter";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openGMDDg;
        private System.Windows.Forms.SaveFileDialog saveMIDDg;
        private System.Windows.Forms.Button btnLoadGMD;
        private System.Windows.Forms.Label labelGMDInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioBMultitrack;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBSingletracks;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnInfo;
    }
}

