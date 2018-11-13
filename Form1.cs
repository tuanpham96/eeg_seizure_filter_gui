﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;

using LiveCharts;
using LiveCharts.WinForms; 
using LiveCharts.Wpf; 
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Geared;

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
        private AppInputParameters app_inp_prm;
        private Thread logic_thread;
        
        public STFTCalculator[] STFTCalcs;
        public RMSCalculator[] RMSCalcs;
        public GearedValues<ObservablePoint>[] STFTSeries { get; set; }
        public GearedValues<ObservablePoint>[] RMSSeries { get; set; }
        public GearedValues<ObservablePoint>[] ChannelSeries { get; set; }

        public GearedValues<ObservablePoint>[] RMSAlarmSeries { get; set; }

        public Form1()
        {
            //MiscellaneousTesting misctest = new MiscellaneousTesting();

            InitializeComponent();

            using (Prompt prompt = new Prompt("ENTER THE INPUT PARAMETERS", "Input parameters"))
            {
                app_inp_prm = prompt.Result;
            }
            app_inp_prm.CompleteInitialize();

            InitializeCalculators(); 
            InitializePlot();             
            this.Load += Form1_Load;
        }

        private void InitializeCalculators()
        {
            int n_lvls = 3;
            int nchan = app_inp_prm.nchan;
            STFTCalcs = new STFTCalculator[nchan];
            RMSCalcs = new RMSCalculator[nchan];
            for (int i = 0; i < nchan; i++)
            {
                RMSCalcs[i] = new RMSCalculator(this.app_inp_prm.nmax_queue_total, n_lvls);
                STFTCalcs[i] = new STFTCalculator(
                    app_inp_prm.Fs, 
                    app_inp_prm.n_epoch, 
                    app_inp_prm.n_skip,
                    new double[] { app_inp_prm.f_bandpower_lower, app_inp_prm.f_bandpower_upper }, 
                    STFTCalculator.WindowType.Hamming, 
                    app_inp_prm.output_file_name, 
                    app_inp_prm.stft_saving_option);
            }
        }
        private void InitializePlot()
        {
            var mapper = Mappers.Xy<ObservablePoint>()
                            .X(value => value.X)
                            .Y(value => value.Y);
            Charting.For<ObservablePoint>(mapper);

            ChannelSeries = new GearedValues<ObservablePoint>[3];

            string[] legends = {"Ch " + app_inp_prm.chan_idx2plt[0],
                                "Ch " + app_inp_prm.chan_idx2plt[1],
                                "Ch " + app_inp_prm.chan_idx2plt[0] + " - Ch " + app_inp_prm.chan_idx2plt[1]};

            for (int idx_obs = 0; idx_obs < 3; idx_obs++)
            {
                ChannelSeries[idx_obs] = new GearedValues<ObservablePoint>();
                ChannelSeries[idx_obs].Quality = app_inp_prm.display_quality; 
                channel_plots.Series.Add(new GLineSeries
                {
                    Values = ChannelSeries[idx_obs],
                    StrokeThickness = 1,
                    PointGeometry = DefaultGeometries.None,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = legends[idx_obs]
                });
            }

            int nchan = app_inp_prm.nchan;

            RMSSeries = new GearedValues<ObservablePoint>[nchan];
            STFTSeries = new GearedValues<ObservablePoint>[nchan];
            for (int ichan = 0; ichan < nchan; ichan++)
            {
                RMSSeries[ichan] = new GearedValues<ObservablePoint>();
                RMSSeries[ichan].Quality = app_inp_prm.display_quality;
                rms_plots.Series.Add(new GLineSeries
                {
                    Values = RMSSeries[ichan],
                    StrokeThickness = 1,
                    PointGeometry = DefaultGeometries.None,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = legends[ichan] + " RMS"
                });

                STFTSeries[ichan] = new GearedValues<ObservablePoint>();
                STFTSeries[ichan].Quality = app_inp_prm.display_quality;
                spectral_plots.Series.Add(new GLineSeries
                {
                    Values = STFTSeries[ichan],
                    StrokeThickness = 1,
                    PointGeometry = DefaultGeometries.None,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Title = legends[ichan] + " STFT"
                });
            }

            int n_lvls = 3;
            System.Windows.Media.SolidColorBrush[] brushes = {
                            System.Windows.Media.Brushes.Green,
                            System.Windows.Media.Brushes.Orange,
                            System.Windows.Media.Brushes.Red};
            string[] alarm_lvl_str = { "Normal", "Warning", "Red" }; 
            RMSAlarmSeries = new GearedValues<ObservablePoint>[n_lvls];
            for (int i = 0; i < n_lvls; i++)
            {
                RMSAlarmSeries[i] = new GearedValues<ObservablePoint>();
                RMSAlarmSeries[i].Quality = app_inp_prm.display_quality;
                SolidBrush sb = new SolidBrush(app_inp_prm.danger_color);
                rms_alarm_plots.Series.Add(new GLineSeries
                {
                    Values = RMSAlarmSeries[i],
                    StrokeThickness = 1,
                    //PointGeometry = DefaultGeometries.None,
                    LineSmoothness = 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Stroke = brushes[i],
                    Title = alarm_lvl_str[i]
                });
            }

            channel_plots.Text = "Channel bro";
            MaximizePlotPerformance(ref channel_plots);
            MaximizePlotPerformance(ref rms_plots);
            MaximizePlotPerformance(ref spectral_plots);
            MaximizePlotPerformance(ref rms_alarm_plots);
            /*
            channel_plots.DisableAnimations = true; // for performance 
            channel_plots.Hoverable = false;
            channel_plots.DataTooltip = null;
            channel_plots.LegendLocation = LegendLocation.Right;
            channel_plots.Invalidate();

            rms_plots.DisableAnimations = true;
            rms_plots.Hoverable = false;
            rms_plots.DataTooltip = null;
            rms_plots.LegendLocation = LegendLocation.Right;
            rms_plots.Invalidate();

            spectral_plots.DisableAnimations = true; 
            spectral_plots.Hoverable = false;
            spectral_plots.DataTooltip = null;
            spectral_plots.LegendLocation = LegendLocation.Right;
            spectral_plots.Invalidate();
            */
        }

        private void MaximizePlotPerformance(ref LiveCharts.WinForms.CartesianChart cart_chart)
        {
            cart_chart.DisableAnimations = true;
            cart_chart.Hoverable = false;
            cart_chart.DataTooltip = null;
            cart_chart.LegendLocation = LegendLocation.Right;
            cart_chart.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            logic_thread = new Thread(DrawAndReport);
            logic_thread.Start();
        }

        private void ReturnRMSLevel(double value, out Color color_res, out int level_res)
        {
            if (value < app_inp_prm.danger_rms_lowerbound | value > app_inp_prm.danger_rms_upperbound) {
                color_res = app_inp_prm.danger_color;
                level_res = 2;
            } else if (value < app_inp_prm.warning_rms_lowerbound | value > app_inp_prm.warning_rms_upperbound) {
                color_res = app_inp_prm.warning_color;
                level_res = 1;
            } else {
                color_res = app_inp_prm.normal_color;
                level_res = 0;
            }
        }

        #region Update functions for simple WF objects 
        private void UpdateTextBox(TextBox txtbx, string s)
        {
            if (InvokeRequired)
            {
                txtbx.BeginInvoke(new Action<TextBox, string>(UpdateTextBox), new object[] { txtbx, s });
                return; 
            }
            txtbx.Text = s; 
        }

        private void UpdateLabel(Label lbl, string s)
        {
            if (InvokeRequired)
            {
                lbl.BeginInvoke(new Action<Label, string>(UpdateLabel), new object[] { lbl, s });
                return;
            }
            lbl.Text = s;
        }

        private void UpdatePanelColor(Panel pnl, Color c, double t)
        {
            if (InvokeRequired)
            {
                pnl.BeginInvoke(new Action<Panel, Color, double>(UpdatePanelColor), new object[] { pnl, c, t });
                return;
            }
            pnl.Refresh();
            pnl.CreateGraphics().FillRectangle(new SolidBrush(c), 0, 0, pnl.Width, pnl.Height); 
        }

        #endregion

        #region Update functions for the plots

        private void RefreshableAxisLimits(double t, out double min_bound, out double max_bound)
        {
            double maxsec = this.app_inp_prm.nsec_plt;
            min_bound = (Math.Floor(t / maxsec) * maxsec);
            max_bound = (Math.Floor(t / maxsec) + 1) * maxsec;
        }

        private void UpdateChannelSeriesPlot(double t, double[] values)
        {
            for (int i_obs = 0; i_obs < values.Length; i_obs++)
            {
                ChannelSeries[i_obs].Add(new ObservablePoint(t, 
                    values[i_obs]*app_inp_prm.display_channel_gains[i_obs] - app_inp_prm.display_channel_sep*(i_obs)));
                if (ChannelSeries[i_obs].Count > app_inp_prm.max_pnt_plt)
                {
                    ChannelSeries[i_obs].RemoveAt(0);
                }
            }
            if (ChannelSeries[0].Count > 200)
            {
                channel_plots.AxisX[0].Title = "Time (seconds)"; 
            }
        }

        private void UpdateChannelSeriesPlot(int count, double t, double[] values)
        {
            double yval; 
            for (int i_obs = 0; i_obs < values.Length; i_obs++)
            {
                yval = values[i_obs] * app_inp_prm.display_channel_gains[i_obs] - app_inp_prm.display_channel_sep * (i_obs);
                if (ChannelSeries[i_obs].Count == app_inp_prm.max_pnt_plt)
                {
                    ChannelSeries[i_obs].Clear(); 
                }
                ChannelSeries[i_obs].Add(new ObservablePoint(t, yval));
            }

            RefreshableAxisLimits(t, out double min_bound, out double max_bound);

            if (ChannelSeries[0].Count > 200)
            {
                channel_plots.AxisX[0].Title = "Time (seconds)";
                channel_plots.AxisX[0].MinValue = min_bound;
                channel_plots.AxisX[0].MaxValue = max_bound;
            }

        }

        private void UpdateRMSSeriesPlot(double t, double[] values)
        {
            for (int i_obs = 0; i_obs < values.Length; i_obs++)
            {
               RMSSeries[i_obs].Add(new ObservablePoint(t,
                    values[i_obs] * app_inp_prm.display_rms_gains[i_obs] - app_inp_prm.display_rms_sep * (i_obs)));
                if (RMSSeries[i_obs].Count > app_inp_prm.max_pnt_plt)
                {
                    RMSSeries[i_obs].RemoveAt(0);
                }
            }
            if (RMSSeries[0].Count > 200)
            {
                rms_plots.AxisX[0].Title = "Time (seconds)";
            }
        }

        private void UpdateRMSSeriesPlot(int count, double t, double[] values)
        {
            double yval;
            for (int i_obs = 0; i_obs < values.Length; i_obs++)
            {
                yval = values[i_obs] * app_inp_prm.display_rms_gains[i_obs] - app_inp_prm.display_rms_sep * (i_obs);
                if (RMSSeries[i_obs].Count == app_inp_prm.max_pnt_plt)
                {
                    RMSSeries[i_obs].Clear();
                }
                RMSSeries[i_obs].Add(new ObservablePoint(t, yval));
            }

            RefreshableAxisLimits(t, out double min_bound, out double max_bound);

            if (RMSSeries[0].Count > 200)
            {
                rms_plots.AxisX[0].Title = "Time (seconds)";
                rms_plots.AxisX[0].MinValue = min_bound;
                rms_plots.AxisX[0].MaxValue = max_bound;
            }

        }
        private void UpdateBandPowerPlot(double t, double[] values)
        {
            for (int i = 0; i < app_inp_prm.nchan; i++)
            {
                if (STFTSeries[i].Count >= app_inp_prm.max_pnt_plt)
                {
                    STFTSeries[i].Clear();
                }
                STFTSeries[i].Add(new ObservablePoint(t, values[i]));
            }
            RefreshableAxisLimits(t, out double min_bound, out double max_bound);

            if (STFTSeries[0].Count > 10)
            {
                spectral_plots.AxisX[0].Title = "Time (seconds)";
                spectral_plots.AxisX[0].MinValue = min_bound;
                spectral_plots.AxisX[0].MaxValue = max_bound;
            }

        }

        private void UpdateSTFTPlot()
        {
            for (int ichan = 0; ichan < app_inp_prm.nchan; ichan++)
            {
                int n_valid = STFTCalcs[ichan].n_valid;
                STFTSeries[ichan].Clear();
                STFTCalcs[ichan].CalculatePSD();
                ObservablePoint[] stft_new = new ObservablePoint[n_valid];
                double[] freq = new double[n_valid], mag = new double[n_valid];
                Array.Copy(STFTCalcs[ichan].freq_vec, 0, freq, 0, n_valid);
                Array.Copy(STFTCalcs[ichan].mag_freq, 0, mag, 0, n_valid);
                for (int idat = 0; idat < n_valid; idat++)
                {
                    stft_new[idat] = new ObservablePoint(freq[idat], mag[idat]);
                }
                STFTSeries[ichan].AddRange(stft_new);
            }
            spectral_plots.AxisX[0].Title = "Frequency (Hz)";
            spectral_plots.AxisX[0].MinValue = 0;
            spectral_plots.AxisX[0].MaxValue = 40;
        }

        private void RefreshableAxisLimits(double t, double max_sec_plt, out double min_bound, out double max_bound)
        {
            min_bound = (Math.Floor(t / max_sec_plt) * max_sec_plt);
            max_bound = (Math.Floor(t / max_sec_plt) + 1) * max_sec_plt;
        } 

        private void UpdateRefreshablePlot( LiveCharts.WinForms.CartesianChart cart_chart, 
                                            GearedValues<ObservablePoint>[] series, 
                                            double t, 
                                            double[] values, 
                                            double max_sec_plt, 
                                            double max_pnt_plt, 
                                            double min_pnt_upd)
        {           
            if (InvokeRequired)
            {
                cart_chart.BeginInvoke(new Action<
                                            LiveCharts.WinForms.CartesianChart,
                                            GearedValues<ObservablePoint>[],
                                            double,
                                            double[],
                                            double,
                                            double,
                                            double>(UpdateRefreshablePlot),
                                                new object[] {
                                                    cart_chart,
                                                    series,
                                                    t,
                                                    values,
                                                    max_sec_plt,
                                                    max_pnt_plt,
                                                    min_pnt_upd});
                return; 
            }

            int n_values = series.Length; 

            for (int i = 0; i < n_values; i++)
            {
                if (series[i].Count >= max_pnt_plt)
                {
                    series[i].Clear();
                }
                series[i].Add(new ObservablePoint(t, values[i]));
            }

            RefreshableAxisLimits(t, max_sec_plt, out double min_bound, out double max_bound);

            if (series[0].Count > min_pnt_upd)
            { 
                cart_chart.AxisX[0].Title = "Time (seconds)";
                cart_chart.AxisX[0].MinValue = min_bound;
                cart_chart.AxisX[0].MaxValue = max_bound;
            } 

        }

        private void UpdateContinuosPlot(LiveCharts.WinForms.CartesianChart cart_chart,
                                    GearedValues<ObservablePoint>[] series,
                                    double t,
                                    double[] values,
                                    double max_pnt_plt,
                                    double min_pnt_upd)
        {
            if (InvokeRequired)
            {
                cart_chart.BeginInvoke(new Action<
                                            LiveCharts.WinForms.CartesianChart,
                                            GearedValues<ObservablePoint>[],
                                            double,
                                            double[],
                                            double,
                                            double>(UpdateContinuosPlot),
                                                new object[] {
                                                    cart_chart,
                                                    series,
                                                    t,
                                                    values,
                                                    max_pnt_plt,
                                                    min_pnt_upd});
                return;
            }

            int n_values = series.Length;

            for (int i = 0; i < n_values; i++)
            {
                if (series[i].Count >= max_pnt_plt)
                {
                    series[i].Clear();
                }
                series[i].Add(new ObservablePoint(t, values[i]));
            }
            
            if (series[0].Count > min_pnt_upd)
            {
                cart_chart.AxisX[0].Title = "Time (seconds)";
            }

        }
        #endregion

        private double[] Full_Deep_Copy(double[] source)
        {
            double[] destination = new double[source.Length];
            Array.Copy(source, destination, source.Length);
            return destination; 
        }
        public void DrawAndReport()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(this.app_inp_prm.hostname, app_inp_prm.port);

                UpdateTextBox(log, app_inp_prm.WelcomeMessage);
                UpdateLabel(chan_label1, "Channel " + app_inp_prm.chan_idx2plt[0]);
                UpdateLabel(chan_label2, "Channel " + app_inp_prm.chan_idx2plt[1]);

                Byte[] bytes = new Byte[1024];
                int[] chan_idx = { app_inp_prm.chan_idx2plt[0], app_inp_prm.chan_idx2plt[1]};
                int nchan = app_inp_prm.nchan;

                Panel[] panels = { rms_alarm1, rms_alarm2 } ;
                while (true)
                {
                    int stream_read;
                    int count = 0;
                    Stream stream = client.GetStream();

                    
                    string csvFilePath = this.app_inp_prm.output_file_name;
                    File.WriteAllText(csvFilePath, "Data_1;RMS_1;Level_1;Data_2;RMS_2;Level_2\n");

                    while ((stream_read = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        string data = System.Text.Encoding.ASCII.GetString(bytes, 0, stream_read);
                        string[] current_data_chunk = data.Split(',');

                        for (int ix = 0; ix < app_inp_prm.nsamp_per_block; ix++)
                        {
                            double t = ((double)count) / this.app_inp_prm.Fs;
                            for (int ich = 0; ich < nchan; ich++)
                            {
                                RMSCalcs[ich].ParseCurrentValue(current_data_chunk[ix + chan_idx[ich] * app_inp_prm.nsamp_per_block]);
                            }

                            double[] viz = {    RMSCalcs[0].current_val,
                                                RMSCalcs[1].current_val,
                                                RMSCalcs[0].current_val - RMSCalcs[1].current_val };

                            if (app_inp_prm.refresh_display)
                            {
                                channel_plots.BeginInvoke(new Action<int, double, double[]>(UpdateChannelSeriesPlot), count, t, viz);
                            } else {
                                channel_plots.BeginInvoke(new Action<double, double[]>(UpdateChannelSeriesPlot), t, viz);
                            }

                            TimeSpan tsp = TimeSpan.FromSeconds(t);

                            Color[] color_level_arr = new Color[nchan];
                            int[] level_idx_arr = new int[nchan];
                            double[] current_rms_arr = new double[nchan], current_val_arr = new double[nchan];

                            for (int ich = 0; ich < nchan; ich++)
                            {
                                RMSCalcs[ich].CalculateRMS(count);
                                current_rms_arr[ich] = RMSCalcs[ich].current_rms;
                                current_val_arr[ich] = RMSCalcs[ich].current_val;
                                ReturnRMSLevel(current_rms_arr[ich], out color_level_arr[ich], out level_idx_arr[ich]);
                                UpdatePanelColor(panels[ich], color_level_arr[ich], t);

                                RMSCalcs[ich].Tally_Levels(level_idx_arr[ich]); 
                            }

                            if (app_inp_prm.refresh_display)
                            {
                                rms_plots.BeginInvoke(new Action<int, double, double[]>(UpdateRMSSeriesPlot), count, t, current_rms_arr);
                            } else {
                                rms_plots.BeginInvoke(new Action<double, double[]>(UpdateRMSSeriesPlot), t, current_rms_arr);
                            }

                            for (int ich = 0; ich < nchan; ich++)
                            { 
                                STFTCalcs[ich].CalculateFFT(t, RMSCalcs[ich].current_val);                                
                            }
                            if (STFTCalcs[0].ready2plt)
                            {
                                double[] viz_bp = new double[nchan];
                                for (int ich = 0; ich < nchan; ich++) { STFTCalcs[ich].CalculatePSD(); viz_bp[ich] = STFTCalcs[ich].band_power; }
                                spectral_plots.BeginInvoke(new Action(UpdateSTFTPlot));
                                //spectral_plots.BeginInvoke(new Action<double, double[]>(UpdateBandPowerPlot), t, viz_bp);
                            }

                            if (count % app_inp_prm.rms_lvl_reset_point == 0 && count > 0)
                            {
                                int n_lvls = RMSCalcs[0].n_lvls;
                                double[] lvl_arrs = new double[n_lvls];
                                for (int ilvl = 0; ilvl < n_lvls; ilvl++) {
                                    lvl_arrs[ilvl] = 0;
                                    for (int ich = 0; ich < nchan; ich++)
                                    {
                                        lvl_arrs[ilvl] += RMSCalcs[ich].rms_levels[ilvl]; 
                                    }
                                }
                                /* rms_alarm_plots.BeginInvoke(new Action<
                                            LiveCharts.WinForms.CartesianChart,
                                            GearedValues<ObservablePoint>[],
                                            double,
                                            double[],
                                            double,
                                            double,
                                            double>(UpdateRefreshablePlot), 
                                                rms_alarm_plots, 
                                                RMSAlarmSeries, 
                                                t, 
                                                lvl_arrs,
                                                app_inp_prm.rms_lvl_max_sec, 
                                                app_inp_prm.rms_lvl_reset_point, 
                                                2); */

                                UpdateRefreshablePlot(
                                                rms_alarm_plots,
                                                RMSAlarmSeries,
                                                t,
                                                lvl_arrs,
                                                app_inp_prm.rms_lvl_max_sec,
                                                app_inp_prm.rms_lvl_reset_point,
                                                2); 
                                for (int ich = 0; ich < nchan; ich++)
                                {
                                    RMSCalcs[ich].Reset_Level_Tally(); 
                                }


                            }
                            UpdateLabel(clock, tsp.ToString(@"hh\:mm\:ss\:fff"));

                            string nextLine = string.Format("{0};{1};{2};{3};{4};{5}\n", 
                                current_val_arr[0], current_rms_arr[0], level_idx_arr[0],
                                current_val_arr[1], current_rms_arr[1], level_idx_arr[1]);
                            File.AppendAllText(csvFilePath, nextLine);
                            
                            count++;
                        }
                    }
                    client.Close();

                }
            }
            catch (Exception e)
            {                
                log.BeginInvoke(new Action(() =>
                {
                    log.Text += "Error..... " + e.StackTrace;
                }));
                Console.WriteLine("!!!!\t" + e.StackTrace);
                DrawAndReport();
            }
        }

        #region Events of Control Objects 
        private void exit_button_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(1); 
        }

        #region Channel gain and separation control for display 
        private void changain_up_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_Channel_Gain(1); 
        }


        private void changain_down_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_Channel_Gain(-1);
        }


        private void chansep_up_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_Channel_Separation(1);
        }

        private void chansep_down_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_Channel_Separation(-1);
        }
        #endregion
        #region RMS gain and separation control for display 
        private void rmsgain_up_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_RMS_Gain(1);
        }

        private void rmsgain_down_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_RMS_Gain(-1);
        }

        private void rmssep_up_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_RMS_Separation(1);
        }

        private void rmssep_down_Click(object sender, EventArgs e)
        {
            app_inp_prm.Control_RMS_Separation(-1);
        }
        #endregion
        #endregion
    }
}
