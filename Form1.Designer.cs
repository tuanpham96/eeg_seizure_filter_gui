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
            this.rms_lab = new System.Windows.Forms.Label();
            this.rms_val1 = new System.Windows.Forms.TextBox();
            this.rms_val2 = new System.Windows.Forms.TextBox();
            this.log = new System.Windows.Forms.TextBox();
            this.safety_lab = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChart2 = new LiveCharts.WinForms.CartesianChart();
            this.chan_label1 = new System.Windows.Forms.Label();
            this.chan_label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rms_lab
            // 
            this.rms_lab.AutoSize = true;
            this.rms_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rms_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_lab.Location = new System.Drawing.Point(93, 75);
            this.rms_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rms_lab.Name = "rms_lab";
            this.rms_lab.Size = new System.Drawing.Size(123, 26);
            this.rms_lab.TabIndex = 1;
            this.rms_lab.Text = "RMS Value";
            // 
            // rms_val1
            // 
            this.rms_val1.Location = new System.Drawing.Point(226, 83);
            this.rms_val1.Margin = new System.Windows.Forms.Padding(2);
            this.rms_val1.Name = "rms_val1";
            this.rms_val1.Size = new System.Drawing.Size(101, 20);
            this.rms_val1.TabIndex = 2;
            // 
            // rms_val2
            // 
            this.rms_val2.Location = new System.Drawing.Point(375, 83);
            this.rms_val2.Margin = new System.Windows.Forms.Padding(2);
            this.rms_val2.Name = "rms_val2";
            this.rms_val2.Size = new System.Drawing.Size(101, 20);
            this.rms_val2.TabIndex = 3;
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(74, 181);
            this.log.Margin = new System.Windows.Forms.Padding(2);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(402, 119);
            this.log.TabIndex = 4;
            // 
            // safety_lab
            // 
            this.safety_lab.AutoSize = true;
            this.safety_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.safety_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safety_lab.Location = new System.Drawing.Point(27, 120);
            this.safety_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.safety_lab.Name = "safety_lab";
            this.safety_lab.Size = new System.Drawing.Size(187, 26);
            this.safety_lab.TabIndex = 5;
            this.safety_lab.Text = "Seizure Threshold";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(226, 120);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(101, 31);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel2.Location = new System.Drawing.Point(375, 120);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(101, 31);
            this.panel2.TabIndex = 7;
            // 
            // cartesianChart1
            // 
            //this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Right;
            this.cartesianChart1.Location = new System.Drawing.Point(540, 20);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1300, 500);
            this.cartesianChart1.TabIndex = 8;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // cartesianChart2
            // 
            this.cartesianChart2.Location = new System.Drawing.Point(540, 600);
            this.cartesianChart2.Name = "cartesianChart2";
            this.cartesianChart2.Size = new System.Drawing.Size(1300, 300);
            this.cartesianChart2.TabIndex = 9;
            this.cartesianChart2.Text = "cartesianChart2"; 
            // 
            // chan_label1
            // 
            this.chan_label1.AutoSize = true;
            this.chan_label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label1.Location = new System.Drawing.Point(228, 37);
            this.chan_label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label1.Name = "chan_label1";
            this.chan_label1.Size = new System.Drawing.Size(99, 24);
            this.chan_label1.TabIndex = 10;
            this.chan_label1.Text = "Channel A";
            // 
            // chan_label2
            // 
            this.chan_label2.AutoSize = true;
            this.chan_label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chan_label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chan_label2.Location = new System.Drawing.Point(378, 37);
            this.chan_label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.chan_label2.Name = "chan_label2";
            this.chan_label2.Size = new System.Drawing.Size(98, 24);
            this.chan_label2.TabIndex = 11;
            this.chan_label2.Text = "Channel B";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1900, 1000);
            this.Controls.Add(this.chan_label2);
            this.Controls.Add(this.chan_label1);
            this.Controls.Add(this.cartesianChart1);
            this.Controls.Add(this.cartesianChart2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.safety_lab);
            this.Controls.Add(this.log);
            this.Controls.Add(this.rms_val2);
            this.Controls.Add(this.rms_val1);
            this.Controls.Add(this.rms_lab);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Visualizing RMS of EEG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label rms_lab;
        private System.Windows.Forms.TextBox rms_val1;
        private System.Windows.Forms.TextBox rms_val2;
        private System.Windows.Forms.Panel panel1; 
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Label safety_lab;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private LiveCharts.WinForms.CartesianChart cartesianChart2;
        private System.Windows.Forms.Label chan_label1;
        private System.Windows.Forms.Label chan_label2;
    }
}

