namespace WindowsFormsApp4
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
            this.log = new System.Windows.Forms.TextBox();
            this.safety_rms_lab = new System.Windows.Forms.Label();
            this.rms_alarm1 = new System.Windows.Forms.Panel();
            this.rms_alarm2 = new System.Windows.Forms.Panel();
            this.channel_plots = new LiveCharts.WinForms.CartesianChart();
            this.rms_plots = new LiveCharts.WinForms.CartesianChart();
            this.spectral_plots = new LiveCharts.WinForms.CartesianChart();
            this.chan_label1 = new System.Windows.Forms.Label();
            this.chan_label2 = new System.Windows.Forms.Label();
            this.spectral_alarm1 = new System.Windows.Forms.Panel();
            this.spectral_alarm2 = new System.Windows.Forms.Panel();
            this.safety_spectral_lab = new System.Windows.Forms.Label();
            this.clock_lab = new System.Windows.Forms.Label();
            this.clock = new System.Windows.Forms.Label();
            this.exit_button = new System.Windows.Forms.Button();
            this.changain_up_button = new System.Windows.Forms.Button();
            this.changain_down_button = new System.Windows.Forms.Button();
            this.changain_cntrl_label = new System.Windows.Forms.Label();
            this.chansep_cntrl_label = new System.Windows.Forms.Label();
            this.chansep_down_button = new System.Windows.Forms.Button();
            this.chansep_up_buttom = new System.Windows.Forms.Button();
            this.rmssep_cntrl_label = new System.Windows.Forms.Label();
            this.rmssep_down_button = new System.Windows.Forms.Button();
            this.rmssep_up_button = new System.Windows.Forms.Button();
            this.rmsgain_cntrl_label = new System.Windows.Forms.Label();
            this.rmsgain_down_button = new System.Windows.Forms.Button();
            this.rmsgain_up_button = new System.Windows.Forms.Button();
            this.rms_alarm_plots = new LiveCharts.WinForms.CartesianChart();
            this.refresh_button = new System.Windows.Forms.Button();
            this.changain_val = new System.Windows.Forms.Label();
            this.chansep_val = new System.Windows.Forms.Label();
            this.rmsgain_val = new System.Windows.Forms.Label();
            this.rmssep_val = new System.Windows.Forms.Label();
            this.limbandpow_plots = new LiveCharts.WinForms.CartesianChart();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(23, 677);
            this.log.Margin = new System.Windows.Forms.Padding(2);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(453, 195);
            this.log.TabIndex = 4;
            // 
            // safety_rms_lab
            // 
            this.safety_rms_lab.AutoSize = true;
            this.safety_rms_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.safety_rms_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safety_rms_lab.Location = new System.Drawing.Point(18, 83);
            this.safety_rms_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.safety_rms_lab.Name = "safety_rms_lab";
            this.safety_rms_lab.Size = new System.Drawing.Size(125, 26);
            this.safety_rms_lab.TabIndex = 5;
            this.safety_rms_lab.Text = "RMS Alarm";
            // 
            // rms_alarm1
            // 
            this.rms_alarm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.rms_alarm1.Location = new System.Drawing.Point(198, 83);
            this.rms_alarm1.Margin = new System.Windows.Forms.Padding(2);
            this.rms_alarm1.Name = "rms_alarm1";
            this.rms_alarm1.Size = new System.Drawing.Size(101, 31);
            this.rms_alarm1.TabIndex = 6;
            // 
            // rms_alarm2
            // 
            this.rms_alarm2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.rms_alarm2.Location = new System.Drawing.Point(347, 83);
            this.rms_alarm2.Margin = new System.Windows.Forms.Padding(2);
            this.rms_alarm2.Name = "rms_alarm2";
            this.rms_alarm2.Size = new System.Drawing.Size(101, 31);
            this.rms_alarm2.TabIndex = 7;
            // 
            // channel_plots
            // 
            this.channel_plots.Location = new System.Drawing.Point(477, 20);
            this.channel_plots.Name = "channel_plots";
            this.channel_plots.Size = new System.Drawing.Size(1411, 189);
            this.channel_plots.TabIndex = 8;
            this.channel_plots.Text = "channel_plots";
            // 
            // rms_plots
            // 
            this.rms_plots.Location = new System.Drawing.Point(477, 215);
            this.rms_plots.Name = "rms_plots";
            this.rms_plots.Size = new System.Drawing.Size(1411, 189);
            this.rms_plots.TabIndex = 9;
            this.rms_plots.Text = "rms_plots";
            // 
            // spectral_plots
            // 
            this.spectral_plots.Location = new System.Drawing.Point(1128, 622);
            this.spectral_plots.Name = "spectral_plots";
            this.spectral_plots.Size = new System.Drawing.Size(748, 304);
            this.spectral_plots.TabIndex = 10;
            this.spectral_plots.Text = "spectral_plots";
            // 
            // chan_label1
            // 
            this.chan_label1.AutoSize = true;
            this.chan_label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label1.Location = new System.Drawing.Point(200, 37);
            this.chan_label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label1.Name = "chan_label1";
            this.chan_label1.Size = new System.Drawing.Size(99, 24);
            this.chan_label1.TabIndex = 11;
            this.chan_label1.Text = "Channel A";
            // 
            // chan_label2
            // 
            this.chan_label2.AutoSize = true;
            this.chan_label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label2.Location = new System.Drawing.Point(350, 37);
            this.chan_label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label2.Name = "chan_label2";
            this.chan_label2.Size = new System.Drawing.Size(98, 24);
            this.chan_label2.TabIndex = 12;
            this.chan_label2.Text = "Channel B";
            // 
            // spectral_alarm1
            // 
            this.spectral_alarm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.spectral_alarm1.Location = new System.Drawing.Point(198, 134);
            this.spectral_alarm1.Margin = new System.Windows.Forms.Padding(2);
            this.spectral_alarm1.Name = "spectral_alarm1";
            this.spectral_alarm1.Size = new System.Drawing.Size(101, 31);
            this.spectral_alarm1.TabIndex = 15;
            // 
            // spectral_alarm2
            // 
            this.spectral_alarm2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.spectral_alarm2.Location = new System.Drawing.Point(347, 134);
            this.spectral_alarm2.Margin = new System.Windows.Forms.Padding(2);
            this.spectral_alarm2.Name = "spectral_alarm2";
            this.spectral_alarm2.Size = new System.Drawing.Size(101, 31);
            this.spectral_alarm2.TabIndex = 14;
            // 
            // safety_spectral_lab
            // 
            this.safety_spectral_lab.AutoSize = true;
            this.safety_spectral_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.safety_spectral_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safety_spectral_lab.Location = new System.Drawing.Point(18, 134);
            this.safety_spectral_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.safety_spectral_lab.Name = "safety_spectral_lab";
            this.safety_spectral_lab.Size = new System.Drawing.Size(156, 26);
            this.safety_spectral_lab.TabIndex = 13;
            this.safety_spectral_lab.Text = "Spectral Alarm";
            // 
            // clock_lab
            // 
            this.clock_lab.AutoSize = true;
            this.clock_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clock_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clock_lab.Location = new System.Drawing.Point(18, 183);
            this.clock_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.clock_lab.Name = "clock_lab";
            this.clock_lab.Size = new System.Drawing.Size(67, 26);
            this.clock_lab.TabIndex = 16;
            this.clock_lab.Text = "Clock";
            // 
            // clock
            // 
            this.clock.AutoSize = true;
            this.clock.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clock.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clock.Location = new System.Drawing.Point(310, 183);
            this.clock.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.clock.Name = "clock";
            this.clock.Size = new System.Drawing.Size(138, 26);
            this.clock.TabIndex = 17;
            this.clock.Text = "00:00:00.000";
            this.clock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.Transparent;
            this.exit_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit_button.Location = new System.Drawing.Point(141, 892);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(90, 34);
            this.exit_button.TabIndex = 18;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // changain_up_button
            // 
            this.changain_up_button.BackColor = System.Drawing.Color.Transparent;
            this.changain_up_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changain_up_button.Location = new System.Drawing.Point(282, 300);
            this.changain_up_button.Name = "changain_up_button";
            this.changain_up_button.Size = new System.Drawing.Size(69, 25);
            this.changain_up_button.TabIndex = 19;
            this.changain_up_button.Text = "Up";
            this.changain_up_button.UseVisualStyleBackColor = false;
            this.changain_up_button.Click += new System.EventHandler(this.changain_up_Click);
            // 
            // changain_down_button
            // 
            this.changain_down_button.BackColor = System.Drawing.Color.Transparent;
            this.changain_down_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changain_down_button.Location = new System.Drawing.Point(373, 299);
            this.changain_down_button.Name = "changain_down_button";
            this.changain_down_button.Size = new System.Drawing.Size(69, 25);
            this.changain_down_button.TabIndex = 20;
            this.changain_down_button.Text = "Down";
            this.changain_down_button.UseVisualStyleBackColor = false;
            this.changain_down_button.Click += new System.EventHandler(this.changain_down_Click);
            // 
            // changain_cntrl_label
            // 
            this.changain_cntrl_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changain_cntrl_label.Location = new System.Drawing.Point(12, 300);
            this.changain_cntrl_label.Name = "changain_cntrl_label";
            this.changain_cntrl_label.Size = new System.Drawing.Size(162, 25);
            this.changain_cntrl_label.TabIndex = 21;
            this.changain_cntrl_label.Text = "Channel gain";
            this.changain_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chansep_cntrl_label
            // 
            this.chansep_cntrl_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chansep_cntrl_label.Location = new System.Drawing.Point(12, 341);
            this.chansep_cntrl_label.Name = "chansep_cntrl_label";
            this.chansep_cntrl_label.Size = new System.Drawing.Size(174, 25);
            this.chansep_cntrl_label.TabIndex = 24;
            this.chansep_cntrl_label.Text = "Channel separation";
            this.chansep_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chansep_down_button
            // 
            this.chansep_down_button.BackColor = System.Drawing.Color.Transparent;
            this.chansep_down_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chansep_down_button.Location = new System.Drawing.Point(373, 340);
            this.chansep_down_button.Name = "chansep_down_button";
            this.chansep_down_button.Size = new System.Drawing.Size(69, 25);
            this.chansep_down_button.TabIndex = 23;
            this.chansep_down_button.Text = "Down";
            this.chansep_down_button.UseVisualStyleBackColor = false;
            this.chansep_down_button.Click += new System.EventHandler(this.chansep_down_Click);
            // 
            // chansep_up_buttom
            // 
            this.chansep_up_buttom.BackColor = System.Drawing.Color.Transparent;
            this.chansep_up_buttom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chansep_up_buttom.Location = new System.Drawing.Point(282, 341);
            this.chansep_up_buttom.Name = "chansep_up_buttom";
            this.chansep_up_buttom.Size = new System.Drawing.Size(69, 25);
            this.chansep_up_buttom.TabIndex = 22;
            this.chansep_up_buttom.Text = "Up";
            this.chansep_up_buttom.UseVisualStyleBackColor = false;
            this.chansep_up_buttom.Click += new System.EventHandler(this.chansep_up_Click);
            // 
            // rmssep_cntrl_label
            // 
            this.rmssep_cntrl_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmssep_cntrl_label.Location = new System.Drawing.Point(12, 440);
            this.rmssep_cntrl_label.Name = "rmssep_cntrl_label";
            this.rmssep_cntrl_label.Size = new System.Drawing.Size(174, 25);
            this.rmssep_cntrl_label.TabIndex = 30;
            this.rmssep_cntrl_label.Text = "RMS separation";
            this.rmssep_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rmssep_down_button
            // 
            this.rmssep_down_button.BackColor = System.Drawing.Color.Transparent;
            this.rmssep_down_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmssep_down_button.Location = new System.Drawing.Point(373, 439);
            this.rmssep_down_button.Name = "rmssep_down_button";
            this.rmssep_down_button.Size = new System.Drawing.Size(69, 25);
            this.rmssep_down_button.TabIndex = 29;
            this.rmssep_down_button.Text = "Down";
            this.rmssep_down_button.UseVisualStyleBackColor = false;
            this.rmssep_down_button.Click += new System.EventHandler(this.rmssep_down_Click);
            // 
            // rmssep_up_button
            // 
            this.rmssep_up_button.BackColor = System.Drawing.Color.Transparent;
            this.rmssep_up_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmssep_up_button.Location = new System.Drawing.Point(282, 440);
            this.rmssep_up_button.Name = "rmssep_up_button";
            this.rmssep_up_button.Size = new System.Drawing.Size(69, 25);
            this.rmssep_up_button.TabIndex = 28;
            this.rmssep_up_button.Text = "Up";
            this.rmssep_up_button.UseVisualStyleBackColor = false;
            this.rmssep_up_button.Click += new System.EventHandler(this.rmssep_up_Click);
            // 
            // rmsgain_cntrl_label
            // 
            this.rmsgain_cntrl_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmsgain_cntrl_label.Location = new System.Drawing.Point(12, 399);
            this.rmsgain_cntrl_label.Name = "rmsgain_cntrl_label";
            this.rmsgain_cntrl_label.Size = new System.Drawing.Size(162, 25);
            this.rmsgain_cntrl_label.TabIndex = 27;
            this.rmsgain_cntrl_label.Text = "RMS gain";
            this.rmsgain_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rmsgain_down_button
            // 
            this.rmsgain_down_button.BackColor = System.Drawing.Color.Transparent;
            this.rmsgain_down_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmsgain_down_button.Location = new System.Drawing.Point(373, 398);
            this.rmsgain_down_button.Name = "rmsgain_down_button";
            this.rmsgain_down_button.Size = new System.Drawing.Size(69, 25);
            this.rmsgain_down_button.TabIndex = 26;
            this.rmsgain_down_button.Text = "Down";
            this.rmsgain_down_button.UseVisualStyleBackColor = false;
            this.rmsgain_down_button.Click += new System.EventHandler(this.rmsgain_down_Click);
            // 
            // rmsgain_up_button
            // 
            this.rmsgain_up_button.BackColor = System.Drawing.Color.Transparent;
            this.rmsgain_up_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmsgain_up_button.Location = new System.Drawing.Point(282, 399);
            this.rmsgain_up_button.Name = "rmsgain_up_button";
            this.rmsgain_up_button.Size = new System.Drawing.Size(69, 25);
            this.rmsgain_up_button.TabIndex = 25;
            this.rmsgain_up_button.Text = "Up";
            this.rmsgain_up_button.UseVisualStyleBackColor = false;
            this.rmsgain_up_button.Click += new System.EventHandler(this.rmsgain_up_Click);
            // 
            // rms_alarm_plots
            // 
            this.rms_alarm_plots.Location = new System.Drawing.Point(496, 622);
            this.rms_alarm_plots.Name = "rms_alarm_plots";
            this.rms_alarm_plots.Size = new System.Drawing.Size(518, 250);
            this.rms_alarm_plots.TabIndex = 31;
            this.rms_alarm_plots.Text = "rms_alarm_plots";
            // 
            // refresh_button
            // 
            this.refresh_button.BackColor = System.Drawing.Color.Transparent;
            this.refresh_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refresh_button.Location = new System.Drawing.Point(23, 892);
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(90, 34);
            this.refresh_button.TabIndex = 32;
            this.refresh_button.Text = "Refresh";
            this.refresh_button.UseVisualStyleBackColor = false;
            this.refresh_button.Click += new System.EventHandler(this.refresh_button_Click);
            // 
            // changain_val
            // 
            this.changain_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changain_val.ForeColor = System.Drawing.Color.Gray;
            this.changain_val.Location = new System.Drawing.Point(162, 300);
            this.changain_val.Name = "changain_val";
            this.changain_val.Size = new System.Drawing.Size(106, 25);
            this.changain_val.TabIndex = 33;
            this.changain_val.Text = "0.0";
            this.changain_val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chansep_val
            // 
            this.chansep_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chansep_val.ForeColor = System.Drawing.Color.Gray;
            this.chansep_val.Location = new System.Drawing.Point(208, 341);
            this.chansep_val.Name = "chansep_val";
            this.chansep_val.Size = new System.Drawing.Size(60, 25);
            this.chansep_val.TabIndex = 34;
            this.chansep_val.Text = "0.0";
            this.chansep_val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rmsgain_val
            // 
            this.rmsgain_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmsgain_val.ForeColor = System.Drawing.Color.Gray;
            this.rmsgain_val.Location = new System.Drawing.Point(162, 399);
            this.rmsgain_val.Name = "rmsgain_val";
            this.rmsgain_val.Size = new System.Drawing.Size(106, 25);
            this.rmsgain_val.TabIndex = 35;
            this.rmsgain_val.Text = "0.0";
            this.rmsgain_val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rmssep_val
            // 
            this.rmssep_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmssep_val.ForeColor = System.Drawing.Color.Gray;
            this.rmssep_val.Location = new System.Drawing.Point(208, 440);
            this.rmssep_val.Name = "rmssep_val";
            this.rmssep_val.Size = new System.Drawing.Size(60, 25);
            this.rmssep_val.TabIndex = 36;
            this.rmssep_val.Text = "0.0";
            this.rmssep_val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // limbandpow_plots
            // 
            this.limbandpow_plots.Location = new System.Drawing.Point(477, 410);
            this.limbandpow_plots.Name = "limbandpow_plots";
            this.limbandpow_plots.Size = new System.Drawing.Size(1411, 189);
            this.limbandpow_plots.TabIndex = 37;
            this.limbandpow_plots.Text = "limbandpow_plots";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1900, 1000);
            this.Controls.Add(this.limbandpow_plots);
            this.Controls.Add(this.rmssep_val);
            this.Controls.Add(this.rmsgain_val);
            this.Controls.Add(this.chansep_val);
            this.Controls.Add(this.changain_val);
            this.Controls.Add(this.refresh_button);
            this.Controls.Add(this.rms_alarm_plots);
            this.Controls.Add(this.rmssep_cntrl_label);
            this.Controls.Add(this.rmssep_down_button);
            this.Controls.Add(this.rmssep_up_button);
            this.Controls.Add(this.rmsgain_cntrl_label);
            this.Controls.Add(this.rmsgain_down_button);
            this.Controls.Add(this.rmsgain_up_button);
            this.Controls.Add(this.chansep_cntrl_label);
            this.Controls.Add(this.chansep_down_button);
            this.Controls.Add(this.chansep_up_buttom);
            this.Controls.Add(this.changain_cntrl_label);
            this.Controls.Add(this.changain_down_button);
            this.Controls.Add(this.changain_up_button);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.clock);
            this.Controls.Add(this.clock_lab);
            this.Controls.Add(this.spectral_alarm1);
            this.Controls.Add(this.spectral_alarm2);
            this.Controls.Add(this.safety_spectral_lab);
            this.Controls.Add(this.chan_label2);
            this.Controls.Add(this.chan_label1);
            this.Controls.Add(this.channel_plots);
            this.Controls.Add(this.rms_plots);
            this.Controls.Add(this.spectral_plots);
            this.Controls.Add(this.rms_alarm2);
            this.Controls.Add(this.rms_alarm1);
            this.Controls.Add(this.safety_rms_lab);
            this.Controls.Add(this.log);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Visualizing RMS of EEG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel rms_alarm1; 
        private System.Windows.Forms.Panel rms_alarm2;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Label safety_rms_lab;
        private LiveCharts.WinForms.CartesianChart channel_plots;
        private LiveCharts.WinForms.CartesianChart rms_plots;
        private LiveCharts.WinForms.CartesianChart spectral_plots;
        private System.Windows.Forms.Label chan_label1;
        private System.Windows.Forms.Label chan_label2;
        private System.Windows.Forms.Panel spectral_alarm1;
        private System.Windows.Forms.Panel spectral_alarm2;
        private System.Windows.Forms.Label safety_spectral_lab;
        private System.Windows.Forms.Label clock_lab;
        private System.Windows.Forms.Label clock;
        
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Button changain_up_button;
        private System.Windows.Forms.Button changain_down_button;
        private System.Windows.Forms.Label changain_cntrl_label;
        private System.Windows.Forms.Label chansep_cntrl_label;
        private System.Windows.Forms.Button chansep_down_button;
        private System.Windows.Forms.Button chansep_up_buttom;
        private System.Windows.Forms.Label rmssep_cntrl_label;
        private System.Windows.Forms.Button rmssep_down_button;
        private System.Windows.Forms.Button rmssep_up_button;
        private System.Windows.Forms.Label rmsgain_cntrl_label;
        private System.Windows.Forms.Button rmsgain_down_button;
        private System.Windows.Forms.Button rmsgain_up_button;

        private LiveCharts.WinForms.CartesianChart rms_alarm_plots;
        private System.Windows.Forms.Button refresh_button;
        private System.Windows.Forms.Label changain_val;
        private System.Windows.Forms.Label chansep_val;
        private System.Windows.Forms.Label rmsgain_val;
        private System.Windows.Forms.Label rmssep_val;
        private LiveCharts.WinForms.CartesianChart limbandpow_plots;
    }
}

