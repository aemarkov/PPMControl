namespace ControlGUI
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackbarElevator = new System.Windows.Forms.TrackBar();
            this.trackbarEleron = new System.Windows.Forms.TrackBar();
            this.trackbarThrottle = new System.Windows.Forms.TrackBar();
            this.trackbarRudder = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listboxDevices = new System.Windows.Forms.ListBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarElevator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarEleron)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarThrottle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarRudder)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trackbarElevator);
            this.groupBox1.Controls.Add(this.trackbarEleron);
            this.groupBox1.Controls.Add(this.trackbarThrottle);
            this.groupBox1.Controls.Add(this.trackbarRudder);
            this.groupBox1.Location = new System.Drawing.Point(450, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 222);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // trackbarElevator
            // 
            this.trackbarElevator.AutoSize = false;
            this.trackbarElevator.Location = new System.Drawing.Point(284, 19);
            this.trackbarElevator.Maximum = 255;
            this.trackbarElevator.Name = "trackbarElevator";
            this.trackbarElevator.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackbarElevator.Size = new System.Drawing.Size(26, 150);
            this.trackbarElevator.TabIndex = 3;
            this.trackbarElevator.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarElevator.Value = 128;
            // 
            // trackbarEleron
            // 
            this.trackbarEleron.AutoSize = false;
            this.trackbarEleron.Location = new System.Drawing.Point(219, 175);
            this.trackbarEleron.Maximum = 255;
            this.trackbarEleron.Name = "trackbarEleron";
            this.trackbarEleron.Size = new System.Drawing.Size(150, 26);
            this.trackbarEleron.TabIndex = 2;
            this.trackbarEleron.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarEleron.Value = 128;
            // 
            // trackbarThrottle
            // 
            this.trackbarThrottle.AutoSize = false;
            this.trackbarThrottle.Location = new System.Drawing.Point(74, 19);
            this.trackbarThrottle.Maximum = 255;
            this.trackbarThrottle.Name = "trackbarThrottle";
            this.trackbarThrottle.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackbarThrottle.Size = new System.Drawing.Size(26, 150);
            this.trackbarThrottle.TabIndex = 1;
            this.trackbarThrottle.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // trackbarRudder
            // 
            this.trackbarRudder.AutoSize = false;
            this.trackbarRudder.Location = new System.Drawing.Point(9, 175);
            this.trackbarRudder.Maximum = 255;
            this.trackbarRudder.Name = "trackbarRudder";
            this.trackbarRudder.Size = new System.Drawing.Size(150, 26);
            this.trackbarRudder.TabIndex = 0;
            this.trackbarRudder.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackbarRudder.Value = 128;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listboxDevices);
            this.groupBox2.Controls.Add(this.btnUpdate);
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 222);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output device";
            // 
            // listboxDevices
            // 
            this.listboxDevices.FormattingEnabled = true;
            this.listboxDevices.HorizontalScrollbar = true;
            this.listboxDevices.Location = new System.Drawing.Point(6, 19);
            this.listboxDevices.Name = "listboxDevices";
            this.listboxDevices.Size = new System.Drawing.Size(420, 160);
            this.listboxDevices.TabIndex = 4;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(194, 189);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(78, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(99, 189);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(78, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 189);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(78, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 245);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "PPM Control";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackbarElevator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarEleron)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarThrottle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarRudder)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackbarElevator;
        private System.Windows.Forms.TrackBar trackbarEleron;
        private System.Windows.Forms.TrackBar trackbarThrottle;
        private System.Windows.Forms.TrackBar trackbarRudder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ListBox listboxDevices;
    }
}

