namespace seizure_filter
{
    partial class MainForm
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
            this.safety_rms_label = new System.Windows.Forms.Label();
            this.rms_alarm1 = new System.Windows.Forms.Panel();
            this.rms_alarm2 = new System.Windows.Forms.Panel();
            this.channel_plots = new LiveCharts.WinForms.CartesianChart();
            this.rms_plots = new LiveCharts.WinForms.CartesianChart();
            this.spectral_plots = new LiveCharts.WinForms.CartesianChart();
            this.chan_label1_rms_alarm = new System.Windows.Forms.Label();
            this.chan_label2_rms_alarm = new System.Windows.Forms.Label();
            this.clock_label = new System.Windows.Forms.Label();
            this.clock = new System.Windows.Forms.Label();
            this.exit_button = new System.Windows.Forms.Button();
            this.chan_vertgain_up_button = new System.Windows.Forms.Button();
            this.chan_vertgain_down_button = new System.Windows.Forms.Button();
            this.chan_vertgain_cntrl_label = new System.Windows.Forms.Label();
            this.chan_vertoffset_cntrl_label = new System.Windows.Forms.Label();
            this.chan_vertoffset_down_button = new System.Windows.Forms.Button();
            this.chan_vertoffset_up_button = new System.Windows.Forms.Button();
            this.rms_vertoffset_cntrl_label = new System.Windows.Forms.Label();
            this.rms_vertoffset_down_button = new System.Windows.Forms.Button();
            this.rms_vertoffset_up_button = new System.Windows.Forms.Button();
            this.rms_vertgain_cntrl_label = new System.Windows.Forms.Label();
            this.rms_vertgain_down_button = new System.Windows.Forms.Button();
            this.rms_vertgain_up_button = new System.Windows.Forms.Button();
            this.rms_alarm_plots = new LiveCharts.WinForms.CartesianChart();
            this.chan_vertgain_val = new System.Windows.Forms.Label();
            this.chan_vertoffset_val = new System.Windows.Forms.Label();
            this.rms_vertgain_val = new System.Windows.Forms.Label();
            this.rms_vertoffset_val = new System.Windows.Forms.Label();
            this.limbandpow_plots = new LiveCharts.WinForms.CartesianChart();
            this.app_title = new System.Windows.Forms.Label();
            this.lbp_alarm_plots = new LiveCharts.WinForms.CartesianChart();
            this.rms_alarm_plot_title = new System.Windows.Forms.Label();
            this.lbp_alarm_plot_title = new System.Windows.Forms.Label();
            this.channel_plot_title = new System.Windows.Forms.Label();
            this.rms_plot_title = new System.Windows.Forms.Label();
            this.lbp_plot_title = new System.Windows.Forms.Label();
            this.spectral_plot_title = new System.Windows.Forms.Label();
            this.chan_label2_spectral_alarm = new System.Windows.Forms.Label();
            this.chan_label1_spectral_alarm = new System.Windows.Forms.Label();
            this.lbp_alarm2 = new System.Windows.Forms.Panel();
            this.lbp_alarm1 = new System.Windows.Forms.Panel();
            this.safety_spectral_label = new System.Windows.Forms.Label();
            this.cntrl_gain_offset_label = new System.Windows.Forms.Label();
            this.log_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.BackColor = System.Drawing.SystemColors.Window;
            this.log.Location = new System.Drawing.Point(476, 1600);
            this.log.Margin = new System.Windows.Forms.Padding(2);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(535, 255);
            this.log.TabIndex = 4;
            // 
            // safety_rms_label
            // 
            this.safety_rms_label.AutoSize = true;
            this.safety_rms_label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.safety_rms_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safety_rms_label.Location = new System.Drawing.Point(25, 148);
            this.safety_rms_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.safety_rms_label.Name = "safety_rms_label";
            this.safety_rms_label.Size = new System.Drawing.Size(125, 26);
            this.safety_rms_label.TabIndex = 5;
            this.safety_rms_label.Text = "RMS Alarm";
            // 
            // rms_alarm1
            // 
            this.rms_alarm1.BackColor = System.Drawing.SystemColors.Control;
            this.rms_alarm1.Location = new System.Drawing.Point(170, 148);
            this.rms_alarm1.Margin = new System.Windows.Forms.Padding(2);
            this.rms_alarm1.Name = "rms_alarm1";
            this.rms_alarm1.Size = new System.Drawing.Size(101, 31);
            this.rms_alarm1.TabIndex = 6;
            // 
            // rms_alarm2
            // 
            this.rms_alarm2.BackColor = System.Drawing.SystemColors.Control;
            this.rms_alarm2.Location = new System.Drawing.Point(319, 148);
            this.rms_alarm2.Margin = new System.Windows.Forms.Padding(2);
            this.rms_alarm2.Name = "rms_alarm2";
            this.rms_alarm2.Size = new System.Drawing.Size(101, 31);
            this.rms_alarm2.TabIndex = 7;
            // 
            // channel_plots
            // 
            this.channel_plots.BackColor = System.Drawing.Color.Transparent;
            this.channel_plots.Location = new System.Drawing.Point(16, 606);
            this.channel_plots.Name = "channel_plots";
            this.channel_plots.Size = new System.Drawing.Size(1004, 203);
            this.channel_plots.TabIndex = 8;
            this.channel_plots.Text = "channel_plots";
            // 
            // rms_plots
            // 
            this.rms_plots.BackColor = System.Drawing.Color.Transparent;
            this.rms_plots.Location = new System.Drawing.Point(16, 852);
            this.rms_plots.Name = "rms_plots";
            this.rms_plots.Size = new System.Drawing.Size(1004, 203);
            this.rms_plots.TabIndex = 9;
            this.rms_plots.Text = "rms_plots";
            // 
            // spectral_plots
            // 
            this.spectral_plots.BackColor = System.Drawing.Color.Transparent;
            this.spectral_plots.Location = new System.Drawing.Point(264, 1358);
            this.spectral_plots.Name = "spectral_plots";
            this.spectral_plots.Size = new System.Drawing.Size(756, 168);
            this.spectral_plots.TabIndex = 10;
            this.spectral_plots.Text = "spectral_plots";
            // 
            // chan_label1_rms_alarm
            // 
            this.chan_label1_rms_alarm.AutoSize = true;
            this.chan_label1_rms_alarm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label1_rms_alarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label1_rms_alarm.Location = new System.Drawing.Point(172, 102);
            this.chan_label1_rms_alarm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label1_rms_alarm.Name = "chan_label1_rms_alarm";
            this.chan_label1_rms_alarm.Size = new System.Drawing.Size(99, 24);
            this.chan_label1_rms_alarm.TabIndex = 11;
            this.chan_label1_rms_alarm.Text = "Channel A";
            // 
            // chan_label2_rms_alarm
            // 
            this.chan_label2_rms_alarm.AutoSize = true;
            this.chan_label2_rms_alarm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label2_rms_alarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label2_rms_alarm.Location = new System.Drawing.Point(322, 102);
            this.chan_label2_rms_alarm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label2_rms_alarm.Name = "chan_label2_rms_alarm";
            this.chan_label2_rms_alarm.Size = new System.Drawing.Size(98, 24);
            this.chan_label2_rms_alarm.TabIndex = 12;
            this.chan_label2_rms_alarm.Text = "Channel B";
            // 
            // clock_label
            // 
            this.clock_label.AutoSize = true;
            this.clock_label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clock_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clock_label.Location = new System.Drawing.Point(719, 28);
            this.clock_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.clock_label.Name = "clock_label";
            this.clock_label.Size = new System.Drawing.Size(159, 26);
            this.clock_label.TabIndex = 16;
            this.clock_label.Text = "Recording time";
            // 
            // clock
            // 
            this.clock.AutoSize = true;
            this.clock.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clock.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clock.Location = new System.Drawing.Point(895, 28);
            this.clock.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.clock.Name = "clock";
            this.clock.Size = new System.Drawing.Size(138, 26);
            this.clock.TabIndex = 17;
            this.clock.Text = "00:00:00.000";
            this.clock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.Gainsboro;
            this.exit_button.FlatAppearance.BorderSize = 0;
            this.exit_button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.exit_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.exit_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit_button.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit_button.Location = new System.Drawing.Point(29, 1821);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(90, 34);
            this.exit_button.TabIndex = 18;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // chan_vertgain_up_button
            // 
            this.chan_vertgain_up_button.BackColor = System.Drawing.Color.Gainsboro;
            this.chan_vertgain_up_button.FlatAppearance.BorderSize = 0;
            this.chan_vertgain_up_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.chan_vertgain_up_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chan_vertgain_up_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertgain_up_button.Location = new System.Drawing.Point(178, 1620);
            this.chan_vertgain_up_button.Name = "chan_vertgain_up_button";
            this.chan_vertgain_up_button.Size = new System.Drawing.Size(30, 25);
            this.chan_vertgain_up_button.TabIndex = 19;
            this.chan_vertgain_up_button.Text = "▲";
            this.chan_vertgain_up_button.UseVisualStyleBackColor = false;
            this.chan_vertgain_up_button.Click += new System.EventHandler(this.changain_up_Click);
            // 
            // chan_vertgain_down_button
            // 
            this.chan_vertgain_down_button.BackColor = System.Drawing.Color.Gainsboro;
            this.chan_vertgain_down_button.FlatAppearance.BorderSize = 0;
            this.chan_vertgain_down_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.chan_vertgain_down_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chan_vertgain_down_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertgain_down_button.Location = new System.Drawing.Point(178, 1650);
            this.chan_vertgain_down_button.Name = "chan_vertgain_down_button";
            this.chan_vertgain_down_button.Size = new System.Drawing.Size(30, 25);
            this.chan_vertgain_down_button.TabIndex = 20;
            this.chan_vertgain_down_button.Text = "▼";
            this.chan_vertgain_down_button.UseVisualStyleBackColor = false;
            this.chan_vertgain_down_button.Click += new System.EventHandler(this.changain_down_Click);
            // 
            // chan_vertgain_cntrl_label
            // 
            this.chan_vertgain_cntrl_label.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertgain_cntrl_label.Location = new System.Drawing.Point(26, 1622);
            this.chan_vertgain_cntrl_label.Name = "chan_vertgain_cntrl_label";
            this.chan_vertgain_cntrl_label.Size = new System.Drawing.Size(140, 25);
            this.chan_vertgain_cntrl_label.TabIndex = 21;
            this.chan_vertgain_cntrl_label.Text = "Channel gain";
            this.chan_vertgain_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chan_vertoffset_cntrl_label
            // 
            this.chan_vertoffset_cntrl_label.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertoffset_cntrl_label.Location = new System.Drawing.Point(26, 1706);
            this.chan_vertoffset_cntrl_label.Name = "chan_vertoffset_cntrl_label";
            this.chan_vertoffset_cntrl_label.Size = new System.Drawing.Size(140, 25);
            this.chan_vertoffset_cntrl_label.TabIndex = 24;
            this.chan_vertoffset_cntrl_label.Text = "Channel offset";
            this.chan_vertoffset_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chan_vertoffset_down_button
            // 
            this.chan_vertoffset_down_button.BackColor = System.Drawing.Color.Gainsboro;
            this.chan_vertoffset_down_button.FlatAppearance.BorderSize = 0;
            this.chan_vertoffset_down_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.chan_vertoffset_down_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chan_vertoffset_down_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertoffset_down_button.Location = new System.Drawing.Point(177, 1736);
            this.chan_vertoffset_down_button.Name = "chan_vertoffset_down_button";
            this.chan_vertoffset_down_button.Size = new System.Drawing.Size(30, 25);
            this.chan_vertoffset_down_button.TabIndex = 23;
            this.chan_vertoffset_down_button.Text = "▼";
            this.chan_vertoffset_down_button.UseVisualStyleBackColor = false;
            this.chan_vertoffset_down_button.Click += new System.EventHandler(this.chanoffset_down_Click);
            // 
            // chan_vertoffset_up_button
            // 
            this.chan_vertoffset_up_button.BackColor = System.Drawing.Color.Gainsboro;
            this.chan_vertoffset_up_button.FlatAppearance.BorderSize = 0;
            this.chan_vertoffset_up_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.chan_vertoffset_up_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chan_vertoffset_up_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertoffset_up_button.Location = new System.Drawing.Point(177, 1706);
            this.chan_vertoffset_up_button.Name = "chan_vertoffset_up_button";
            this.chan_vertoffset_up_button.Size = new System.Drawing.Size(30, 25);
            this.chan_vertoffset_up_button.TabIndex = 22;
            this.chan_vertoffset_up_button.Text = "▲";
            this.chan_vertoffset_up_button.UseVisualStyleBackColor = false;
            this.chan_vertoffset_up_button.Click += new System.EventHandler(this.chanoffset_up_Click);
            // 
            // rms_vertoffset_cntrl_label
            // 
            this.rms_vertoffset_cntrl_label.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertoffset_cntrl_label.Location = new System.Drawing.Point(291, 1706);
            this.rms_vertoffset_cntrl_label.Name = "rms_vertoffset_cntrl_label";
            this.rms_vertoffset_cntrl_label.Size = new System.Drawing.Size(108, 25);
            this.rms_vertoffset_cntrl_label.TabIndex = 30;
            this.rms_vertoffset_cntrl_label.Text = "RMS offset";
            this.rms_vertoffset_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rms_vertoffset_down_button
            // 
            this.rms_vertoffset_down_button.BackColor = System.Drawing.Color.Gainsboro;
            this.rms_vertoffset_down_button.FlatAppearance.BorderSize = 0;
            this.rms_vertoffset_down_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.rms_vertoffset_down_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rms_vertoffset_down_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertoffset_down_button.Location = new System.Drawing.Point(410, 1736);
            this.rms_vertoffset_down_button.Name = "rms_vertoffset_down_button";
            this.rms_vertoffset_down_button.Size = new System.Drawing.Size(30, 25);
            this.rms_vertoffset_down_button.TabIndex = 29;
            this.rms_vertoffset_down_button.Text = "▼";
            this.rms_vertoffset_down_button.UseVisualStyleBackColor = false;
            this.rms_vertoffset_down_button.Click += new System.EventHandler(this.rmsoofset_down_Click);
            // 
            // rms_vertoffset_up_button
            // 
            this.rms_vertoffset_up_button.BackColor = System.Drawing.Color.Gainsboro;
            this.rms_vertoffset_up_button.FlatAppearance.BorderSize = 0;
            this.rms_vertoffset_up_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.rms_vertoffset_up_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rms_vertoffset_up_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertoffset_up_button.Location = new System.Drawing.Point(410, 1706);
            this.rms_vertoffset_up_button.Name = "rms_vertoffset_up_button";
            this.rms_vertoffset_up_button.Size = new System.Drawing.Size(30, 25);
            this.rms_vertoffset_up_button.TabIndex = 28;
            this.rms_vertoffset_up_button.Text = "▲";
            this.rms_vertoffset_up_button.UseVisualStyleBackColor = false;
            this.rms_vertoffset_up_button.Click += new System.EventHandler(this.rmsoffset_up_Click);
            // 
            // rms_vertgain_cntrl_label
            // 
            this.rms_vertgain_cntrl_label.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertgain_cntrl_label.Location = new System.Drawing.Point(291, 1622);
            this.rms_vertgain_cntrl_label.Name = "rms_vertgain_cntrl_label";
            this.rms_vertgain_cntrl_label.Size = new System.Drawing.Size(108, 25);
            this.rms_vertgain_cntrl_label.TabIndex = 27;
            this.rms_vertgain_cntrl_label.Text = "RMS gain";
            this.rms_vertgain_cntrl_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rms_vertgain_down_button
            // 
            this.rms_vertgain_down_button.BackColor = System.Drawing.Color.Gainsboro;
            this.rms_vertgain_down_button.FlatAppearance.BorderSize = 0;
            this.rms_vertgain_down_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.rms_vertgain_down_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rms_vertgain_down_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertgain_down_button.Location = new System.Drawing.Point(410, 1650);
            this.rms_vertgain_down_button.Name = "rms_vertgain_down_button";
            this.rms_vertgain_down_button.Size = new System.Drawing.Size(30, 25);
            this.rms_vertgain_down_button.TabIndex = 26;
            this.rms_vertgain_down_button.Text = "▼";
            this.rms_vertgain_down_button.UseVisualStyleBackColor = false;
            this.rms_vertgain_down_button.Click += new System.EventHandler(this.rmsgain_down_Click);
            // 
            // rms_vertgain_up_button
            // 
            this.rms_vertgain_up_button.BackColor = System.Drawing.Color.Gainsboro;
            this.rms_vertgain_up_button.FlatAppearance.BorderSize = 0;
            this.rms_vertgain_up_button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.rms_vertgain_up_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rms_vertgain_up_button.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertgain_up_button.Location = new System.Drawing.Point(410, 1620);
            this.rms_vertgain_up_button.Name = "rms_vertgain_up_button";
            this.rms_vertgain_up_button.Size = new System.Drawing.Size(30, 25);
            this.rms_vertgain_up_button.TabIndex = 25;
            this.rms_vertgain_up_button.Text = "▲";
            this.rms_vertgain_up_button.UseVisualStyleBackColor = false;
            this.rms_vertgain_up_button.Click += new System.EventHandler(this.rmsgain_up_Click);
            // 
            // rms_alarm_plots
            // 
            this.rms_alarm_plots.BackColor = System.Drawing.Color.Transparent;
            this.rms_alarm_plots.Location = new System.Drawing.Point(16, 255);
            this.rms_alarm_plots.Name = "rms_alarm_plots";
            this.rms_alarm_plots.Size = new System.Drawing.Size(456, 304);
            this.rms_alarm_plots.TabIndex = 31;
            this.rms_alarm_plots.Text = "rms_alarm_plots";
            // 
            // chan_vertgain_val
            // 
            this.chan_vertgain_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertgain_val.ForeColor = System.Drawing.Color.Gray;
            this.chan_vertgain_val.Location = new System.Drawing.Point(27, 1647);
            this.chan_vertgain_val.Name = "chan_vertgain_val";
            this.chan_vertgain_val.Size = new System.Drawing.Size(140, 25);
            this.chan_vertgain_val.TabIndex = 33;
            this.chan_vertgain_val.Text = "0.0";
            this.chan_vertgain_val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chan_vertoffset_val
            // 
            this.chan_vertoffset_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_vertoffset_val.ForeColor = System.Drawing.Color.Gray;
            this.chan_vertoffset_val.Location = new System.Drawing.Point(27, 1731);
            this.chan_vertoffset_val.Name = "chan_vertoffset_val";
            this.chan_vertoffset_val.Size = new System.Drawing.Size(140, 25);
            this.chan_vertoffset_val.TabIndex = 34;
            this.chan_vertoffset_val.Text = "0.0";
            this.chan_vertoffset_val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rms_vertgain_val
            // 
            this.rms_vertgain_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertgain_val.ForeColor = System.Drawing.Color.Gray;
            this.rms_vertgain_val.Location = new System.Drawing.Point(292, 1647);
            this.rms_vertgain_val.Name = "rms_vertgain_val";
            this.rms_vertgain_val.Size = new System.Drawing.Size(108, 25);
            this.rms_vertgain_val.TabIndex = 35;
            this.rms_vertgain_val.Text = "0.0";
            this.rms_vertgain_val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rms_vertoffset_val
            // 
            this.rms_vertoffset_val.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_vertoffset_val.ForeColor = System.Drawing.Color.Gray;
            this.rms_vertoffset_val.Location = new System.Drawing.Point(292, 1731);
            this.rms_vertoffset_val.Name = "rms_vertoffset_val";
            this.rms_vertoffset_val.Size = new System.Drawing.Size(108, 25);
            this.rms_vertoffset_val.TabIndex = 36;
            this.rms_vertoffset_val.Text = "0.0";
            this.rms_vertoffset_val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // limbandpow_plots
            // 
            this.limbandpow_plots.BackColor = System.Drawing.Color.Transparent;
            this.limbandpow_plots.Location = new System.Drawing.Point(16, 1101);
            this.limbandpow_plots.Name = "limbandpow_plots";
            this.limbandpow_plots.Size = new System.Drawing.Size(1004, 203);
            this.limbandpow_plots.TabIndex = 37;
            this.limbandpow_plots.Text = "limbandpow_plots";
            // 
            // app_title
            // 
            this.app_title.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.app_title.Location = new System.Drawing.Point(24, 9);
            this.app_title.Name = "app_title";
            this.app_title.Size = new System.Drawing.Size(662, 58);
            this.app_title.TabIndex = 38;
            this.app_title.Text = "EEG SEIZURE MONITORING SYSTEM";
            this.app_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbp_alarm_plots
            // 
            this.lbp_alarm_plots.BackColor = System.Drawing.Color.Transparent;
            this.lbp_alarm_plots.Location = new System.Drawing.Point(503, 255);
            this.lbp_alarm_plots.Name = "lbp_alarm_plots";
            this.lbp_alarm_plots.Size = new System.Drawing.Size(531, 304);
            this.lbp_alarm_plots.TabIndex = 39;
            this.lbp_alarm_plots.Text = "lbp_alarm_plots";
            // 
            // rms_alarm_plot_title
            // 
            this.rms_alarm_plot_title.BackColor = System.Drawing.Color.Transparent;
            this.rms_alarm_plot_title.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_alarm_plot_title.Location = new System.Drawing.Point(111, 194);
            this.rms_alarm_plot_title.Name = "rms_alarm_plot_title";
            this.rms_alarm_plot_title.Size = new System.Drawing.Size(287, 58);
            this.rms_alarm_plot_title.TabIndex = 40;
            this.rms_alarm_plot_title.Text = "RMS Alarm Rates";
            this.rms_alarm_plot_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rms_alarm_plot_title.UseMnemonic = false;
            // 
            // lbp_alarm_plot_title
            // 
            this.lbp_alarm_plot_title.BackColor = System.Drawing.Color.Transparent;
            this.lbp_alarm_plot_title.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbp_alarm_plot_title.Location = new System.Drawing.Point(631, 194);
            this.lbp_alarm_plot_title.Name = "lbp_alarm_plot_title";
            this.lbp_alarm_plot_title.Size = new System.Drawing.Size(339, 58);
            this.lbp_alarm_plot_title.TabIndex = 41;
            this.lbp_alarm_plot_title.Text = "Spectral Alarm Rates";
            this.lbp_alarm_plot_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbp_alarm_plot_title.UseMnemonic = false;
            // 
            // channel_plot_title
            // 
            this.channel_plot_title.BackColor = System.Drawing.Color.Transparent;
            this.channel_plot_title.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channel_plot_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.channel_plot_title.Location = new System.Drawing.Point(39, 564);
            this.channel_plot_title.Name = "channel_plot_title";
            this.channel_plot_title.Size = new System.Drawing.Size(303, 39);
            this.channel_plot_title.TabIndex = 42;
            this.channel_plot_title.Text = "EEG plots";
            this.channel_plot_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.channel_plot_title.UseMnemonic = false;
            // 
            // rms_plot_title
            // 
            this.rms_plot_title.BackColor = System.Drawing.Color.Transparent;
            this.rms_plot_title.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_plot_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rms_plot_title.Location = new System.Drawing.Point(35, 811);
            this.rms_plot_title.Name = "rms_plot_title";
            this.rms_plot_title.Size = new System.Drawing.Size(303, 39);
            this.rms_plot_title.TabIndex = 43;
            this.rms_plot_title.Text = "RMS plots";
            this.rms_plot_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rms_plot_title.UseMnemonic = false;
            // 
            // lbp_plot_title
            // 
            this.lbp_plot_title.BackColor = System.Drawing.Color.Transparent;
            this.lbp_plot_title.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbp_plot_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbp_plot_title.Location = new System.Drawing.Point(39, 1057);
            this.lbp_plot_title.Name = "lbp_plot_title";
            this.lbp_plot_title.Size = new System.Drawing.Size(303, 39);
            this.lbp_plot_title.TabIndex = 44;
            this.lbp_plot_title.Text = "Bandpower plots";
            this.lbp_plot_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbp_plot_title.UseMnemonic = false;
            // 
            // spectral_plot_title
            // 
            this.spectral_plot_title.BackColor = System.Drawing.Color.Transparent;
            this.spectral_plot_title.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spectral_plot_title.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.spectral_plot_title.Location = new System.Drawing.Point(287, 1316);
            this.spectral_plot_title.Name = "spectral_plot_title";
            this.spectral_plot_title.Size = new System.Drawing.Size(226, 39);
            this.spectral_plot_title.TabIndex = 45;
            this.spectral_plot_title.Text = "Spectral plots";
            this.spectral_plot_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.spectral_plot_title.UseMnemonic = false;
            // 
            // chan_label2_spectral_alarm
            // 
            this.chan_label2_spectral_alarm.AutoSize = true;
            this.chan_label2_spectral_alarm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label2_spectral_alarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label2_spectral_alarm.Location = new System.Drawing.Point(923, 107);
            this.chan_label2_spectral_alarm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label2_spectral_alarm.Name = "chan_label2_spectral_alarm";
            this.chan_label2_spectral_alarm.Size = new System.Drawing.Size(98, 24);
            this.chan_label2_spectral_alarm.TabIndex = 20;
            this.chan_label2_spectral_alarm.Text = "Channel B";
            // 
            // chan_label1_spectral_alarm
            // 
            this.chan_label1_spectral_alarm.AutoSize = true;
            this.chan_label1_spectral_alarm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label1_spectral_alarm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label1_spectral_alarm.Location = new System.Drawing.Point(773, 107);
            this.chan_label1_spectral_alarm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label1_spectral_alarm.Name = "chan_label1_spectral_alarm";
            this.chan_label1_spectral_alarm.Size = new System.Drawing.Size(99, 24);
            this.chan_label1_spectral_alarm.TabIndex = 19;
            this.chan_label1_spectral_alarm.Text = "Channel A";
            // 
            // lbp_alarm2
            // 
            this.lbp_alarm2.BackColor = System.Drawing.SystemColors.Control;
            this.lbp_alarm2.Location = new System.Drawing.Point(920, 148);
            this.lbp_alarm2.Margin = new System.Windows.Forms.Padding(2);
            this.lbp_alarm2.Name = "lbp_alarm2";
            this.lbp_alarm2.Size = new System.Drawing.Size(101, 31);
            this.lbp_alarm2.TabIndex = 14;
            // 
            // lbp_alarm1
            // 
            this.lbp_alarm1.BackColor = System.Drawing.SystemColors.Control;
            this.lbp_alarm1.Location = new System.Drawing.Point(771, 148);
            this.lbp_alarm1.Margin = new System.Windows.Forms.Padding(2);
            this.lbp_alarm1.Name = "lbp_alarm1";
            this.lbp_alarm1.Size = new System.Drawing.Size(101, 31);
            this.lbp_alarm1.TabIndex = 15;
            // 
            // safety_spectral_label
            // 
            this.safety_spectral_label.AutoSize = true;
            this.safety_spectral_label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.safety_spectral_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safety_spectral_label.Location = new System.Drawing.Point(591, 148);
            this.safety_spectral_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.safety_spectral_label.Name = "safety_spectral_label";
            this.safety_spectral_label.Size = new System.Drawing.Size(156, 26);
            this.safety_spectral_label.TabIndex = 13;
            this.safety_spectral_label.Text = "Spectral Alarm";
            // 
            // cntrl_gain_offset_label
            // 
            this.cntrl_gain_offset_label.BackColor = System.Drawing.Color.Transparent;
            this.cntrl_gain_offset_label.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cntrl_gain_offset_label.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cntrl_gain_offset_label.Location = new System.Drawing.Point(25, 1559);
            this.cntrl_gain_offset_label.Name = "cntrl_gain_offset_label";
            this.cntrl_gain_offset_label.Size = new System.Drawing.Size(335, 39);
            this.cntrl_gain_offset_label.TabIndex = 46;
            this.cntrl_gain_offset_label.Text = "Display gain and offset";
            this.cntrl_gain_offset_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cntrl_gain_offset_label.UseMnemonic = false;
            // 
            // log_label
            // 
            this.log_label.BackColor = System.Drawing.Color.Transparent;
            this.log_label.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.log_label.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.log_label.Location = new System.Drawing.Point(472, 1559);
            this.log_label.Name = "log_label";
            this.log_label.Size = new System.Drawing.Size(335, 39);
            this.log_label.TabIndex = 47;
            this.log_label.Text = "Log";
            this.log_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.log_label.UseMnemonic = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1053, 1889);
            this.Controls.Add(this.log_label);
            this.Controls.Add(this.cntrl_gain_offset_label);
            this.Controls.Add(this.rms_vertoffset_down_button);
            this.Controls.Add(this.rms_vertoffset_up_button);
            this.Controls.Add(this.rms_vertgain_down_button);
            this.Controls.Add(this.rms_vertgain_up_button);
            this.Controls.Add(this.chan_vertoffset_down_button);
            this.Controls.Add(this.chan_vertoffset_up_button);
            this.Controls.Add(this.chan_vertgain_down_button);
            this.Controls.Add(this.chan_vertgain_up_button);
            this.Controls.Add(this.spectral_plot_title);
            this.Controls.Add(this.lbp_plot_title);
            this.Controls.Add(this.chan_label2_spectral_alarm);
            this.Controls.Add(this.rms_plot_title);
            this.Controls.Add(this.chan_label1_spectral_alarm);
            this.Controls.Add(this.channel_plot_title);
            this.Controls.Add(this.lbp_alarm_plot_title);
            this.Controls.Add(this.rms_alarm_plot_title);
            this.Controls.Add(this.lbp_alarm_plots);
            this.Controls.Add(this.app_title);
            this.Controls.Add(this.limbandpow_plots);
            this.Controls.Add(this.rms_vertoffset_val);
            this.Controls.Add(this.rms_vertgain_val);
            this.Controls.Add(this.chan_vertoffset_val);
            this.Controls.Add(this.chan_vertgain_val);
            this.Controls.Add(this.rms_alarm_plots);
            this.Controls.Add(this.rms_vertoffset_cntrl_label);
            this.Controls.Add(this.rms_vertgain_cntrl_label);
            this.Controls.Add(this.chan_vertoffset_cntrl_label);
            this.Controls.Add(this.chan_vertgain_cntrl_label);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.clock);
            this.Controls.Add(this.clock_label);
            this.Controls.Add(this.lbp_alarm1);
            this.Controls.Add(this.lbp_alarm2);
            this.Controls.Add(this.safety_spectral_label);
            this.Controls.Add(this.chan_label2_rms_alarm);
            this.Controls.Add(this.chan_label1_rms_alarm);
            this.Controls.Add(this.channel_plots);
            this.Controls.Add(this.rms_plots);
            this.Controls.Add(this.spectral_plots);
            this.Controls.Add(this.rms_alarm2);
            this.Controls.Add(this.rms_alarm1);
            this.Controls.Add(this.safety_rms_label);
            this.Controls.Add(this.log);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EEG Seizure Monitoring System";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel rms_alarm1; 
        private System.Windows.Forms.Panel rms_alarm2;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Label safety_rms_label;
        private LiveCharts.WinForms.CartesianChart channel_plots;
        private LiveCharts.WinForms.CartesianChart rms_plots;
        private LiveCharts.WinForms.CartesianChart spectral_plots;
        private System.Windows.Forms.Label chan_label1_rms_alarm;
        private System.Windows.Forms.Label chan_label2_rms_alarm;
        private System.Windows.Forms.Label clock_label;
        private System.Windows.Forms.Label clock;
        
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Button chan_vertgain_up_button;
        private System.Windows.Forms.Button chan_vertgain_down_button;
        private System.Windows.Forms.Label chan_vertgain_cntrl_label;
        private System.Windows.Forms.Label chan_vertoffset_cntrl_label;
        private System.Windows.Forms.Button chan_vertoffset_down_button;
        private System.Windows.Forms.Button chan_vertoffset_up_button;
        private System.Windows.Forms.Label rms_vertoffset_cntrl_label;
        private System.Windows.Forms.Button rms_vertoffset_down_button;
        private System.Windows.Forms.Button rms_vertoffset_up_button;
        private System.Windows.Forms.Label rms_vertgain_cntrl_label;
        private System.Windows.Forms.Button rms_vertgain_down_button;
        private System.Windows.Forms.Button rms_vertgain_up_button;

        private LiveCharts.WinForms.CartesianChart rms_alarm_plots;
        private System.Windows.Forms.Label chan_vertgain_val;
        private System.Windows.Forms.Label chan_vertoffset_val;
        private System.Windows.Forms.Label rms_vertgain_val;
        private System.Windows.Forms.Label rms_vertoffset_val;
        private LiveCharts.WinForms.CartesianChart limbandpow_plots;
        private System.Windows.Forms.Label app_title;
        private LiveCharts.WinForms.CartesianChart lbp_alarm_plots;
        private System.Windows.Forms.Label rms_alarm_plot_title;
        private System.Windows.Forms.Label lbp_alarm_plot_title;
        private System.Windows.Forms.Label channel_plot_title;
        private System.Windows.Forms.Label rms_plot_title;
        private System.Windows.Forms.Label lbp_plot_title;
        private System.Windows.Forms.Label spectral_plot_title;
        private System.Windows.Forms.Label chan_label2_spectral_alarm;
        private System.Windows.Forms.Label chan_label1_spectral_alarm;
        private System.Windows.Forms.Panel lbp_alarm2;
        private System.Windows.Forms.Panel lbp_alarm1;
        private System.Windows.Forms.Label safety_spectral_label;
        private System.Windows.Forms.Label cntrl_gain_offset_label;
        private System.Windows.Forms.Label log_label;
    }
}

