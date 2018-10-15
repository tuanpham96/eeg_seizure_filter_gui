﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;

// Tuan added _ begin 
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections;
using System.Collections.Generic;
// Tuan added _ end 

// Seizure Filter EEG Program 
// Started by Dominic DiCarlo September 2018

// This program requires communication with OpenViBE acquisition software
// as well as designer software. That software can be obtained from
// Brain Products, and is found on the flash drive Towle lab
// received from them.

// What this program does:
//
// This program reads in data from a TCP server generated by OpenViBE Designer 
// It takes in the data given at 512 Hz signal delivered in 4 sample chunks 
// (or a 128 Hz signal delivering 4 samples at a time) and calculates
// the RMS to (theoretically) predict when a seizure might be coming on.
// if the RMS approaches certain upper or lower bounds, the graphical interface
// of the program will change the color of a bar to signify this, from green (safe)
// to yellow (caution), to red (danger). Right now, the upper and lower bounds of
// these ranges has been arbitrarily decided. To use this program for the
// visual prosthesis, these values will either have to be pre-determined
// or determined after experimentation has begun. We must decide whether to
// err on the side of safety or risk. 

// How to Run the program:
// 1) Connect the V-Amplifier to the computer and install any drivers
//    you may be prompted to. The Brain Products usb will also install some drivers
//    to your PC; these are needed for the V-Amp (supposedly)
//
// 2) Open OpenViBE Acquisition Server. Select V-Amp/Fast Amp from the Driver drop
//    down menu. Adjust drift tolerance to 2.00 ms or lower from the Preferences button.
//    Set "Sample count per sent block" to 4.
//
// 3) Open OpenViBE Designer. If this is your first time, select
//    an "Acquisition Client" box from the folder "Acquisition and network IO".
//    connect this to a "Temporal filter" box from '"Signal Processing"->"Temporal Filtering"'.
//    Connect the "Temporal filter" box to a "Signal display" box and a "Matrix sender" box.
//
//    IMPORTANT: Assure the "Matrix sender" box has TCP port "1234". You can check this by
//    double clicking it, and seeing the TCP port. 
//
//    EQUALLY IMPORANT: Assure the "Acquisition client" box has the same acquisition server
//    port as the Connection port number on OpenViBE Acquisition Server. You can check the box
//    by double clicking it, and the actual Acquisition Server right on the front of the program
//
//    All the program really needs from designer is the "Matrix Sender", as this is what it reads.
//    Feel free to adjust the Temporal Filter to your exact frequency needs,
//    as well as adjust the signal display parameters for your needs. 
//
//    You can save your settings in OpenViBE Designer as a config file. Simply
//    click Save As at the top or Save. 
//
// 4) Hit Start at the top of Visual Studio, or run this program in the packaged way
//    you have decided. Then, hit the play arrow on top of OpenViBE designer. 
//    Voila! The program should be running. If not, run through these steps again
//    and ensure you have done everything right.


// Some things to consider integrating:
//
// 1) It would be nice to be able to change the upper and lower bounds of the RMS
//    safety window from the GUI. This change could be easily made
//
// 2) Adding more channels and the ability to manage multiple samples from the stream
//    at once.
//    - There is room in the logic to do this, and it would just look like more
//      of the same code. We would need to decide how many channels we want to 
//      look at in the first place.
//
// 3) Adding in EEG visualization
//      - OpenViBE supplies this in the mean time, but creating our own EEG viewer
//        would be optimal as we can modify it to suit our needs.
//
// 4) Some visualization of the RMS over time
//      - This would be very useful, as right now we only get monitoring of the present
//        state of the RMS, and we might want to look at the trends it is taking.
//
// 5) Writing the CSV of values from the experiment in a path that will work for
//    all systems
//      - Right now, the CSV path has to be edited in the code
//        so that it works on other systems. What would be more ideal
//        would be the user selecting a path before the program begins. 
//        This would be easy to program. 

// The logic of everything is simplified right now to only work with one
// channel and one sample at a time. This is then a skeleton for more work
// depending on how much data you want to use. 

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        // these are the publically defined range values
        // for the RMS. The Red range is for absolute danger
        // while the Yellow (ylw) range is for caution.
        public double redRangeRmsUp = 1.4;
        public double redRangeRmsLow = 0.6;
        public double ylwRangeRmsUp = 1.2;
        public double ylwRangeRmsLow = 0.8;

        public LineSeries rms_lineseries;
        public PlotView myRMSPlot;
        public PlotModel myRMSModel;

        public double Fs = 512.0; 
        public double x_val, y_val;

        private Thread logic_thread;
        private Thread plot_thread;

        public delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();
            init_RMSplot();
            this.Load += Form1_Load;            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logic_thread = new Thread(drawAndReport);
            plot_thread = new Thread(plotData);
            plot_thread.Start();
            logic_thread.Start();
        }       

        private void init_RMSplot()
        {
            myRMSPlot = new PlotView
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Location = new System.Drawing.Point(500, 500),
                Size = new System.Drawing.Size(500, 500),
                TabIndex = 1
            };

            myRMSModel = new PlotModel
            {
                Title = "RMS plot",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };

            var linearAxisX = new LinearAxis
            {
                Title = "Time (s)",
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 5
            };
            myRMSModel.Axes.Add(linearAxisX);

            var linearAxisY = new LinearAxis
            {
                Title = "RMS values",
                Position = AxisPosition.Left,
                AbsoluteMinimum = -5,
                AbsoluteMaximum = 5
            };
            myRMSModel.Axes.Add(linearAxisY);

            this.rms_lineseries = new LineSeries
            {
                Color = OxyColors.Black,
                MarkerStroke = OxyColors.Black,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1.5,
                LineStyle = LineStyle.Solid
            };
            myRMSModel.Series.Add(this.rms_lineseries);

            myRMSPlot.Model = myRMSModel;
            myRMSPlot.InvalidatePlot(true);
            this.Controls.Add(myRMSPlot);
        }
        
        public void plotData() 
        {
            Console.WriteLine("x val = " + x_val + "; y_val = " + y_val);
            this.rms_lineseries.Points.Add(new DataPoint(x_val, y_val));
            myRMSPlot.InvalidatePlot(true);
           
        }
        
        public void drawAndReport()
        {
            /**
             * Need to address magic numbers and organize then better
            */ 
            
            try 
            { 
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 1234);
                int ntotal_samp = 64;
                int nsamp_per_block = 4;
                int num_channel = -1;  // meaning has not been defined 
                int chan_idx2plt = 0; 
                log.Invoke(new Action(() =>
                {
                    log.Text = "Connected!";
                }));

                Byte[] bytes = new Byte[16384];
                Queue<double> data_queue = new Queue<double>();
                double oldest_val = 0, newest_val = 0; 

                while (true) {
                    int stream_read;
                    int count = 0;
                    double current_rms_sq = 0;
                    double current_rms = 0;
                    Stream stream = client.GetStream();

                    string csvFilePath = @"C:\Users\Towle\Desktop\testfile_TP.csv";
                    File.WriteAllText(csvFilePath, "");

                    while ((stream_read = stream.Read(bytes, 0, bytes.Length)) != 0) {
                        string data = System.Text.Encoding.ASCII.GetString(bytes, 0, stream_read);
                        log.Invoke(new Action(() => {
                            log.Text = data;
                        }));

                        string[] niceData = data.Split(',');
                        if (num_channel < 0) {
                            num_channel = (int)niceData.Length / nsamp_per_block;
                            Console.WriteLine("Number of channels = " + num_channel);
                            
                        }
                        double niceData_ix = 0;
                        for (int ix = 0; ix < nsamp_per_block; ix++) {
                            // This was somehow downsampled 
                            // need to take a look at 
                            // maybe the time it took to get thru each loop interfered with the timing 
                            // dont know yet how to solve this
                            // maybe it's best to read from the file output of signal display instead? 
                            // This is mostly due to plotting 
                            double.TryParse(niceData[ix + chan_idx2plt * num_channel], out niceData_ix);
                            x_val = ((double)count) / Fs;
                            y_val = niceData_ix;                           
                            
                            
                            //this.rms_lineseries.Points.Add(new DataPoint(x_val, y_val));
                            //myRMSPlot.InvalidatePlot(true);
                            
                            
                            data_queue.Enqueue(niceData_ix);

                            if (count < ntotal_samp) {
                                current_rms_sq += niceData_ix * niceData_ix;
                            }
                            else if (count == ntotal_samp) {
                                current_rms = Math.Sqrt(current_rms_sq / ntotal_samp);
                            }
                            else {
                                oldest_val = data_queue.Dequeue();
                                oldest_val = oldest_val * oldest_val / ntotal_samp;

                                newest_val = niceData_ix * niceData_ix / ntotal_samp;
                                current_rms = Math.Sqrt(current_rms - oldest_val + newest_val);
                            }

                            rms_val.Invoke(new Action(() => {
                                rms_val.Text = String.Format("{0}", current_rms);
                            }));
                            
                            myRMSPlot.BeginInvoke(new InvokeDelegate(plotData));

                            if (current_rms < redRangeRmsLow | current_rms > redRangeRmsUp) {
                                panel1.BackColor = Color.FromArgb(255, 0, 0);
                            }
                            else if (current_rms < ylwRangeRmsLow | current_rms > ylwRangeRmsUp) {
                                panel1.BackColor = Color.FromArgb(255, 255, 0);
                            }
                            else {
                                panel1.BackColor = Color.FromArgb(0, 255, 0);
                            }
                            


                            string nextLine = string.Format("{0}\n", niceData_ix);
                            File.AppendAllText(csvFilePath, nextLine);

                            count++;
                        }
                    }
                    client.Close();
                }
            }
            catch (Exception e)
            {
                log.Invoke(new Action(() =>
                {
                    log.Text += "Error..... " + e.StackTrace;
                }));
                drawAndReport();
            }
        }

        private void safety_lab_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
