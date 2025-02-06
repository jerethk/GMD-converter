namespace GMD_converter
{
    partial class TrackInfoWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBoxMetaEvents = new System.Windows.Forms.ListBox();
            label1 = new System.Windows.Forms.Label();
            btnExportCsv = new System.Windows.Forms.Button();
            numericTrack = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            listBoxSysExEvents = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)numericTrack).BeginInit();
            SuspendLayout();
            // 
            // listBoxMetaEvents
            // 
            listBoxMetaEvents.FormattingEnabled = true;
            listBoxMetaEvents.HorizontalScrollbar = true;
            listBoxMetaEvents.ItemHeight = 15;
            listBoxMetaEvents.Location = new System.Drawing.Point(163, 53);
            listBoxMetaEvents.Name = "listBoxMetaEvents";
            listBoxMetaEvents.Size = new System.Drawing.Size(447, 469);
            listBoxMetaEvents.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(163, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(71, 15);
            label1.TabIndex = 1;
            label1.Text = "Meta Events";
            // 
            // btnExportCsv
            // 
            btnExportCsv.Location = new System.Drawing.Point(30, 127);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new System.Drawing.Size(99, 35);
            btnExportCsv.TabIndex = 2;
            btnExportCsv.Text = "To CSV";
            btnExportCsv.UseVisualStyleBackColor = true;
            // 
            // numericTrack
            // 
            numericTrack.Location = new System.Drawing.Point(30, 53);
            numericTrack.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            numericTrack.Name = "numericTrack";
            numericTrack.Size = new System.Drawing.Size(99, 23);
            numericTrack.TabIndex = 3;
            numericTrack.ValueChanged += numericTrack_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(30, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(34, 15);
            label2.TabIndex = 4;
            label2.Text = "Track";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(641, 25);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(73, 15);
            label3.TabIndex = 6;
            label3.Text = "SysEx Events";
            // 
            // listBoxSysExEvents
            // 
            listBoxSysExEvents.FormattingEnabled = true;
            listBoxSysExEvents.HorizontalScrollbar = true;
            listBoxSysExEvents.ItemHeight = 15;
            listBoxSysExEvents.Location = new System.Drawing.Point(641, 53);
            listBoxSysExEvents.Name = "listBoxSysExEvents";
            listBoxSysExEvents.Size = new System.Drawing.Size(438, 469);
            listBoxSysExEvents.TabIndex = 5;
            // 
            // TrackInfoWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1112, 548);
            Controls.Add(label3);
            Controls.Add(listBoxSysExEvents);
            Controls.Add(label2);
            Controls.Add(numericTrack);
            Controls.Add(btnExportCsv);
            Controls.Add(label1);
            Controls.Add(listBoxMetaEvents);
            Name = "TrackInfoWindow";
            Text = "TrackInfoWindow";
            Shown += TrackInfoWindow_Shown;
            ((System.ComponentModel.ISupportInitialize)numericTrack).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBoxMetaEvents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.NumericUpDown numericTrack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxSysExEvents;
    }
}