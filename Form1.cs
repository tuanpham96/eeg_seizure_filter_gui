﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;

using LiveCharts;
using LiveCharts.Wpf; 
using LiveCharts.Configurations;

/** SEIZURE FILTER EEG PROGRAM
 * Started by Dominic DiCarlo September 2018

    + REQUIREMENTS: 

            This program requires communication with OpenViBE acquisition software
        as well as designer software.That software can be obtained from
        Brain Products, and is found on the flash drive Towle lab
        received from them.

    + SUMMARY OF PURPOSE:
    
            This program reads in data from a TCP server generated by OpenViBE Designer
        It takes in the data given at 512 Hz signal delivered in 4 sample chunks
        (or a 128 Hz signal delivering 4 samples at a time) and calculates
        the RMS to(theoretically) predict when a seizure might be coming on.

            If the RMS approaches certain upper or lower bounds, the graphical interface
        of the program will change the color of a bar to signify this, from green (safe)
        to yellow (caution), to red (danger). Right now, the upper and lower bounds of
        these ranges has been arbitrarily decided. To use this program for the
        visual prosthesis, these values will either have to be pre-determined
        or determined after experimentation has begun.We must decide whether to
        err on the side of safety or risk.

    + EXECUTION: 
        1)  Connect the V-Amplifier to the computer and install any drivers
            you may be prompted to. The Brain Products usb will also install some drivers
            to your PC; these are needed for the V-Amp (supposedly)

        2)  Open OpenViBE Acquisition Server. Select V-Amp/Fast Amp from the Driver drop
            down menu. Adjust drift tolerance to 2.00 ms or lower from the Preferences button.
            Set "Sample count per sent block" to 4.

        3)  Open OpenViBE Designer. If this is your first time, select
            an "Acquisition Client" box from the folder "Acquisition and network IO".
            connect this to a "Temporal filter" box from '"Signal Processing"->"Temporal Filtering"'.
            Connect the "Temporal filter" box to a "Signal display" box and a "Matrix sender" box.

            NOTE #1 : Assure the "Matrix sender" box has TCP port "1234". You can check this by
            double clicking it, and seeing the TCP port. 

            NOTE #2: Assure the "Acquisition client" box has the same acquisition server
            port as the Connection port number on OpenViBE Acquisition Server. You can check the box
            by double clicking it, and the actual Acquisition Server right on the front of the program

            All the program really needs from designer is the "Matrix Sender", as this is what it reads.
            Feel free to adjust the Temporal Filter to your exact frequency needs,
            as well as adjust the signal display parameters for your needs. 

            You can save your settings in OpenViBE Designer as a config file. Simply
            click Save As at the top or Save. 

        4)  Hit Start at the top of Visual Studio, or run this program in the packaged way
            you have decided. Then, hit the play arrow on top of OpenViBE designer. 
            Voila! The program should be running. If not, run through these steps again
            and ensure you have done everything right.


    + FUTURE CONSIDERTIONS:

        1)  It would be nice to be able to change the upper and lower bounds of the RMS
            safety window from the GUI. This change could be easily made

        2)  Adding more channels and the ability to manage multiple samples from the stream at once.
            - There is room in the logic to do this, and it would just look like more of the same code. 
            We would need to decide how many channels we want to look at in the first place.

        3)  Adding in EEG visualization
            - OpenViBE supplies this in the mean time, but creating our own EEG viewer
            would be optimal as we can modify it to suit our needs.

        4)  Some visualization of the RMS over time
            - This would be very useful, as right now we only get monitoring of the present
            state of the RMS, and we might want to look at the trends it is taking.

        5)  Writing the CSV of values from the experiment in a path that will work for all systems
            - Right now, the CSV path has to be edited in the code
            so that it works on other systems.What would be more ideal
            would be the user selecting a path before the program begins.
            This would be easy to program.

    The logic of everything is simplified right now to only work with one
    channel and one sample at a time.This is then a skeleton for more work
    depending on how much data you want to use.
*/

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public double danger_upperbound = 1.4;
        public double danger_lowerbound = 0.6;
        public double warning_upperbound = 1.2;
        public double warning_lowerbound = 0.8;

        public static Color danger_color = Color.FromArgb(255, 0, 0);
        public static Color warning_color = Color.FromArgb(255, 255, 0);
        public static Color normal_color = Color.FromArgb(0, 255, 0);

        private string hostname = "127.0.0.1"; // local 
        private int port = 1234; // default 

        private double Fs = 512.0;
        private int nmax_queue_total = 64;
        private int nsamp_per_block = 4;
        private int chan_idx2plt = 3;

        private int max_pnt_plt = 512; 

        private string file_name = @"C:\Users\Towle\Desktop\Tuan\data\testfile_TP.csv";
        private Thread logic_thread;
        public ChartValues<Model> ChartValues { get; set; }
        public class Model
        {
            public double XVal { get; set; }
            public double YVal { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            string result;
            using (Prompt prompt = new Prompt("text", "caption"))
            {
                result = prompt.Result;
            }
            Console.WriteLine("\t >>>>>>>> Dialog = " + result); 
            var mapper = Mappers.Xy<Model>()
                .X(model => model.XVal)   
                .Y(model => model.YVal);
            Charting.For<Model>(mapper);
            ChartValues = new ChartValues<Model>();

            cartesianChart1.Series = new SeriesCollection {  new LineSeries
            {
                Values = ChartValues,
                StrokeThickness = 1,
                PointGeometry = null,
                LineSmoothness = 0 
            }};         
            
            cartesianChart1.DisableAnimations = true; // for performance 
            
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logic_thread = new Thread(drawAndReport);
            logic_thread.Start();
        }       
        
        public Color return_indicated_color(double value)
        {
            if (value < danger_lowerbound | value > danger_upperbound) {
                return danger_color;
            } else if (value < warning_lowerbound | value > warning_upperbound) {
                return warning_color;
            } else {
                return normal_color;
            }
        }

        private string welcomeMessage()
        {
            string message = String.Format("Connected to HOST@{0} - PORT@{1}\r\n",
                        this.hostname, this.port);
            message += "\t+ Sampling frequency: " + Fs + "\r\n";
            message += "\t+ Number of samples/epoch:" + nsamp_per_block + "\r\n";
            message += "\t+ Channel to plot: #" + chan_idx2plt + "\r\n";
            message += String.Format("\t+ RMS window is: {0} points ({1:0} ms)\r\n",
                       nmax_queue_total, nmax_queue_total * 1000 / Fs); 

            return message; 
        }
        
        public void drawAndReport()
        {
            try 
            {
                TcpClient client = new TcpClient();
                client.Connect(this.hostname, this.port);
                log.Invoke(new Action(() =>
                {
                    log.Text = welcomeMessage();
                }));

                Byte[] bytes = new Byte[16384];
                Queue<double> data_queue = new Queue<double>();
                double oldest_val = 0, newest_val = 0;
                double current_rms_sq = 0;
                double current_rms = -1; // not initiated 
                
                while (true)
                {
                    int stream_read;
                    int count = 0;
                    Stream stream = client.GetStream();

                    string csvFilePath = this.file_name;
                    File.WriteAllText(csvFilePath, "Data;RMS\n");

                    while ((stream_read = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        string data = System.Text.Encoding.ASCII.GetString(bytes, 0, stream_read);
                        string[] current_data_chunk = data.Split(',');

                        double current_data_point;
                        double t;
                        int idx_chan_samp; 
                        for (int ix = 0; ix < nsamp_per_block; ix++)
                        {
                            idx_chan_samp = ix + chan_idx2plt * nsamp_per_block;
                            double.TryParse(current_data_chunk[idx_chan_samp], out current_data_point);
                            t = ((double)count) / Fs;
                            
                            ChartValues.Add(new Model
                            {
                                XVal = t,
                                YVal = current_data_point
                            });
                            if (ChartValues.Count > max_pnt_plt) {
                                ChartValues.RemoveAt(0);
                            }

                            
                            // queue data and calc rms 
                            data_queue.Enqueue(current_data_point);
                            if (count < nmax_queue_total - 1)
                            {
                                current_rms_sq += current_data_point * current_data_point;
                            }
                            else if (count == nmax_queue_total - 1)
                            {
                                current_rms = Math.Sqrt(current_rms_sq / nmax_queue_total);
                            }
                            else
                            {
                                oldest_val = data_queue.Dequeue();                                
                                oldest_val = oldest_val * oldest_val / nmax_queue_total;
                                newest_val = current_data_point * current_data_point / nmax_queue_total;
                                current_rms_sq = current_rms * current_rms; 
                                current_rms = Math.Sqrt(current_rms_sq - oldest_val + newest_val);
                            }

                            panel1.BackColor = return_indicated_color(current_rms);                            
                            
                            string nextLine = string.Format("{0};{1}\n", current_data_point, current_rms);
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
                Console.WriteLine("!!!!\t" + e.StackTrace);
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
