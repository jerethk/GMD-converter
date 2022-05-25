
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
            this.openGMDDg = new System.Windows.Forms.OpenFileDialog();
            this.saveMIDDg = new System.Windows.Forms.SaveFileDialog();
            this.btnLoadGMD = new System.Windows.Forms.Button();
            this.labelGMDInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioBMultitrack = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBSingletracks = new System.Windows.Forms.RadioButton();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openGMDDg
            // 
            this.openGMDDg.DefaultExt = "gmd";
            this.openGMDDg.Filter = "GMD Files|*.GMD";
            this.openGMDDg.Title = "Load GMD file";
            // 
            // saveMIDDg
            // 
            this.saveMIDDg.DefaultExt = "mid";
            this.saveMIDDg.Filter = "MIDI files|*.mid";
            this.saveMIDDg.Title = "Save MIDI ";
            this.saveMIDDg.FileOk += new System.ComponentModel.CancelEventHandler(this.saveMIDDg_FileOk);
            // 
            // btnLoadGMD
            // 
            this.btnLoadGMD.Location = new System.Drawing.Point(12, 22);
            this.btnLoadGMD.Name = "btnLoadGMD";
            this.btnLoadGMD.Size = new System.Drawing.Size(103, 37);
            this.btnLoadGMD.TabIndex = 0;
            this.btnLoadGMD.Text = "Load GMD";
            this.btnLoadGMD.UseVisualStyleBackColor = true;
            this.btnLoadGMD.Click += new System.EventHandler(this.btnLoadGMD_Click);
            // 
            // labelGMDInfo
            // 
            this.labelGMDInfo.AutoSize = true;
            this.labelGMDInfo.Location = new System.Drawing.Point(12, 188);
            this.labelGMDInfo.Name = "labelGMDInfo";
            this.labelGMDInfo.Size = new System.Drawing.Size(92, 15);
            this.labelGMDInfo.TabIndex = 1;
            this.labelGMDInfo.Text = "No GMD loaded";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "GMD information:";
            // 
            // radioBMultitrack
            // 
            this.radioBMultitrack.AutoSize = true;
            this.radioBMultitrack.Checked = true;
            this.radioBMultitrack.Location = new System.Drawing.Point(7, 32);
            this.radioBMultitrack.Name = "radioBMultitrack";
            this.radioBMultitrack.Size = new System.Drawing.Size(187, 19);
            this.radioBMultitrack.TabIndex = 3;
            this.radioBMultitrack.TabStop = true;
            this.radioBMultitrack.Text = "Format 2 MIDI (multi track file)";
            this.radioBMultitrack.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBSingletracks);
            this.groupBox1.Controls.Add(this.radioBMultitrack);
            this.groupBox1.Location = new System.Drawing.Point(171, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 113);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export options";
            // 
            // radioBSingletracks
            // 
            this.radioBSingletracks.AutoSize = true;
            this.radioBSingletracks.Location = new System.Drawing.Point(6, 69);
            this.radioBSingletracks.Name = "radioBSingletracks";
            this.radioBSingletracks.Size = new System.Drawing.Size(179, 19);
            this.radioBSingletracks.TabIndex = 5;
            this.radioBSingletracks.Text = "Format 0 MIDI (multiple files)";
            this.radioBSingletracks.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(424, 22);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(103, 37);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export to MIDI";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 407);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelGMDInfo);
            this.Controls.Add(this.btnLoadGMD);
            this.MinimumSize = new System.Drawing.Size(600, 440);
            this.Name = "Form1";
            this.Text = "GMD to MID converter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

