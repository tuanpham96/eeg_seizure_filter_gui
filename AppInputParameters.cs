﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.IO;
using LiveCharts.Geared; 

namespace WindowsFormsApp4
{
    public class AppInputParameters
    {
        public string hostname { get; set; }
        public int port { get; set; }

        public double Fs { get; set; }
        public int nmax_queue_total { get; set; }
        public int nsamp_per_block { get; set; }
        public int[] chan_idx2plt { get; set; }
        public string channels2plt { get; set; }

        public string output_folder { get; set; }
        public string output_file_name { get; set; }

        public double danger_upperbound { get; set; }
        public double danger_lowerbound { get; set; }
        public double warning_upperbound { get; set; }
        public double warning_lowerbound { get; set; }

        public Color danger_color { get; set; }
        public Color warning_color { get; set; }
        public Color normal_color { get; set; }

        public int max_pnt_plt { get; set; }
        public double nsec_plt { get; set; }

        public double[] display_gains { get; set; }
        public string gain_str { get; set; }
        public double display_sep { get; set; }

        public bool refresh_display { get; set; }

        public Quality display_quality { get; set; }

        public Dictionary<string, bool> display_refresh_options = new Dictionary<string, bool> {
                {"Refreshable", true },
                {"Continuous", false }
        };

        public Dictionary<string, Quality> gear_quality_dict = new Dictionary<string, Quality>
        {
            {"Low", Quality.Low },
            {"Medium", Quality.Medium },
            {"High", Quality.High },
            {"Highest", Quality.Highest }
        };

        public Dictionary<string, string> nameAndProp = new Dictionary<string, string>
            {
                {"Host name", "hostname" },
                {"Port", "port" },
                {"Sample frequency (Hz)", "Fs" },
                {"RMS window (#points)", "nmax_queue_total" },
                {"Number of samples / channel / epoch", "nsamp_per_block" },
                {"Channels to display", "channels2plt" },
                {"Output folder", "output_folder"},
                {"Output file name",  "output_file_name" },
                {"DANGER upper bound",  "danger_upperbound" },
                {"DANGER lower bound",  "danger_lowerbound" },
                {"WARNING upper bound", "warning_upperbound" },
                {"WARNING lower bound", "warning_lowerbound" },
                {"Display duration (seconds)", "nsec_plt" },
                {"Display gains", "gain_str" },
                {"Display separation", "display_sep" },
                {"Display refreshed", "refresh_display" },
                {"Display quality", "display_quality" }
            };

        /* !!! Need to check for conditions of Channel_Idx and Gains_Arr
         */  
        public AppInputParameters()
        {            
            hostname = "127.0.0.1";
            port = 1234;

            Fs = 512.0;
            nmax_queue_total = 64;
            nsamp_per_block = 4;

            channels2plt = "0;1";

            output_folder = @"C:\Users\Towle\Desktop\Tuan\general_towle\data";
            output_file_name = "testfile_TP.csv";

            danger_upperbound = 1.4;
            danger_lowerbound = 0.6;
            warning_upperbound = 1.2;
            warning_lowerbound = 0.8;

            danger_color = Color.FromArgb(255, 0, 0);
            warning_color = Color.FromArgb(255, 255, 0);
            normal_color = Color.FromArgb(0, 255, 0);

            nsec_plt = 10; 
            gain_str = "1;1;1";
            display_sep = 5;

            refresh_display = true;
            display_quality = Quality.Low; 
        }
        
        public void CompleteInitialize()
        {
            InitializeChannelIndex();

            output_file_name = Path.Combine(output_folder, output_file_name);

            max_pnt_plt = (int)(Fs * nsec_plt);

            InitializeDisplayGains();        
        }

        public void InitializeChannelIndex()
        {
            string[] chans = channels2plt.Split(';');
            chan_idx2plt = new int[chans.Length];
            for (int ich = 0; ich < chans.Length; ich++)
            {
                int.TryParse(chans[ich], out chan_idx2plt[ich]);
            }
        }
        public void InitializeDisplayGains()
        {
            string[] gains_arr = gain_str.Split(';');
            display_gains = new double[gains_arr.Length];
            for (int ig = 0; ig < gains_arr.Length; ig++)
            {
                double.TryParse(gains_arr[ig], out display_gains[ig]);
            }
        }

        public object GetPropValue(string propName)
        {
            return this.GetType().GetRuntimeProperty(propName)?.GetValue(this);
        }

        public void SetPropValue(string propName, object newValue)
        {
            PropertyInfo propInfo = this.GetType().GetProperty(propName);
            try
            {
                propInfo.SetValue(this, Convert.ChangeType(newValue, propInfo.PropertyType));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in setting value: -> " + e.StackTrace);
            }
        }

    }
}
