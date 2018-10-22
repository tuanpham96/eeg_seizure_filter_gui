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
            this.rms_val = new System.Windows.Forms.TextBox();
            this.log = new System.Windows.Forms.TextBox();
            this.safety_lab = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            //this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.cartesianCharts = new LiveCharts.WinForms.CartesianChart[3]; 
            this.SuspendLayout();
            // 
            // rms_lab
            // 
            this.rms_lab.AutoSize = true;
            this.rms_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rms_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rms_lab.Location = new System.Drawing.Point(104, 61);
            this.rms_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rms_lab.Name = "rms_lab";
            this.rms_lab.Size = new System.Drawing.Size(123, 26);
            this.rms_lab.TabIndex = 1;
            this.rms_lab.Text = "RMS Value";
            // 
            // rms_val
            // 
            this.rms_val.Location = new System.Drawing.Point(226, 69);
            this.rms_val.Margin = new System.Windows.Forms.Padding(2);
            this.rms_val.Name = "rms_val";
            this.rms_val.Size = new System.Drawing.Size(101, 20);
            this.rms_val.TabIndex = 2;
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(108, 190);
            this.log.Margin = new System.Windows.Forms.Padding(2);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(402, 146);
            this.log.TabIndex = 3;
            // 
            // safety_lab
            // 
            this.safety_lab.AutoSize = true;
            this.safety_lab.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.safety_lab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safety_lab.Location = new System.Drawing.Point(38, 106);
            this.safety_lab.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.safety_lab.Name = "safety_lab";
            this.safety_lab.Size = new System.Drawing.Size(187, 26);
            this.safety_lab.TabIndex = 5;
            this.safety_lab.Text = "Seizure Threshold";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(226, 106);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(143, 31);
            this.panel1.TabIndex = 6;
            // 
            // cartesianChart1
            // 
            /*
            this.cartesianChart1.Dock = System.Windows.Forms.DockStyle.Right;
            this.cartesianChart1.Location = new System.Drawing.Point(589, 0);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(911, 500);
            this.cartesianChart1.TabIndex = 1;
            this.cartesianChart1.Text = "cartesianChart1";
            */
            for (int idx = 0; idx < cartesianCharts.Length; idx++)
            {
                this.cartesianCharts[idx] = new LiveCharts.WinForms.CartesianChart();
                //this.cartesianCharts[idx].Dock = System.Windows.Forms.DockStyle.Right;
                this.cartesianCharts[idx].Location = new System.Drawing.Point(570, 100*idx);
                this.cartesianCharts[idx].Name = string.Format("cartesianChart%d", idx);
                this.cartesianCharts[idx].Size = new System.Drawing.Size(900, 150);
                this.cartesianCharts[idx].TabIndex = 1;
                this.cartesianCharts[idx].Text = string.Format("cartesianChart%d", idx);
                this.Controls.Add(this.cartesianCharts[idx]);
            }
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1500, 500);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.safety_lab);
            this.Controls.Add(this.log);
            this.Controls.Add(this.rms_val);
            this.Controls.Add(this.rms_lab);
            //this.Controls.Add(this.cartesianChart1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Visualizing RMS of EEG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label rms_lab;
        private System.Windows.Forms.TextBox rms_val;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Label safety_lab;
        private System.Windows.Forms.Panel panel1;
        //private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private LiveCharts.WinForms.CartesianChart[] cartesianCharts;
    }
}

