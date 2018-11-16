using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.IO;
using LiveCharts.Geared;

namespace WindowsFormsApp4
{
    using static STFTCalculator;
    public class AppInputParameters
    {
        #region Input parameters 
        public string hostname { get; set; }
        public int port { get; set; }

        public double Fs { get; set; }
        public int nmax_queue_total { get; set; }
        public int nsamp_per_block { get; set; }
        public int[] chan_idx2plt { get; set; }
        public string channels2plt { get; set; }

        public string output_folder { get; set; }
        public string output_file_name { get; set; }

        public double danger_rms_upperbound { get; set; }
        public double danger_rms_lowerbound { get; set; }
        public double warning_rms_upperbound { get; set; }
        public double warning_rms_lowerbound { get; set; }

        public Color danger_color { get; set; }
        public Color warning_color { get; set; }
        public Color normal_color { get; set; }

        public int max_pnt_plt { get; set; }
        public double max_sec_plt { get; set; }

        public double[] display_channel_gains { get; set; }
        public string channel_gain_str { get; set; }
        public double display_channel_sep { get; set; }

        public double[] display_rms_gains { get; set; }
        public double display_rms_sep { get; set; }

        public bool refresh_display { get; set; }

        public Quality display_quality { get; set; }

        public double nsec_fft { get; set; }
        public double per_overlap { get; set; }
        public int n_epoch { get; set; }
        public int n_skip { get; set; }
        public WindowType window_type { get; set; }
        public double f_bandpower_lower { get; set; }
        public double f_bandpower_upper { get; set; }
        public bool stft_saving_option { get; set; }

        public double danger_lbp_upperbound { get; set; }
        public double danger_lbp_lowerbound { get; set; }
        public double warning_lbp_upperbound { get; set; }
        public double warning_lbp_lowerbound { get; set; }

        public string WelcomeMessage { get; set; }
        public int nchan { get; set; }

        public double rms_lvl_reset_sec { get; set; }
        public double rms_lvl_max_sec { get; set; }
        public int rms_lvl_reset_point { get; set; }
        public int rms_lvl_max_point { get; set; }

        private double d_gain = 0.1, d_sep = 0.1;
        private double min_gain = 0.01, min_sep = 0; 
        #endregion

        #region Dictionaries for categorical options 
        public Dictionary<string, bool> display_refresh_options = new Dictionary<string, bool>
        {
            { "Refreshable", true },
            { "Continuous", false }
        };

        public Dictionary<string, Quality> gear_quality_dict = new Dictionary<string, Quality>
        {
            { "Low", Quality.Low },
            { "Medium", Quality.Medium },
            { "High", Quality.High },
            { "Highest", Quality.Highest }
        };

        public Dictionary<string, WindowType> wintype_dict = new Dictionary<string, STFTCalculator.WindowType>
        {
            { "Hanning", WindowType.Hanning },
            { "Hamming", WindowType.Hamming },
            { "Triangular", WindowType.Triangle },
            { "No window",  WindowType.Rectangle },
        };


        public Dictionary<string, bool> stft_saving_options = new Dictionary<string, bool>
        {
            { "Yes", true },
            { "No", false }
        };

        #endregion

        #region Dictionary for parameters to querry 
        public Dictionary<string, string> nameAndProp = new Dictionary<string, string>
        {
            { "Host name", "hostname" },
            { "Port", "port" },
            { "Sample frequency (Hz)", "Fs" },
            { "RMS window (#points)", "nmax_queue_total" },
            { "Number of samples / channel / epoch", "nsamp_per_block" },
            { "Channels to display", "channels2plt" },
            { "Output folder", "output_folder"},
            { "Output file name",  "output_file_name" },
            { "DANGER RMS upper bound",  "danger_rms_upperbound" },
            { "DANGER RMS lower bound",  "danger_rms_lowerbound" },
            { "WARNING RMS upper bound", "warning_rms_upperbound" },
            { "WARNING RMS lower bound", "warning_rms_lowerbound" },
            { "Display duration (seconds)", "max_sec_plt" },
            { "Display gains", "channel_gain_str" },
            { "Display separation", "display_channel_sep" },
            { "Display refreshed", "refresh_display" },
            { "Display quality", "display_quality" },
            { "STFT Length (s)", "nsec_fft" },
            { "STFT Overlap (%)", "per_overlap" },
            { "STFT Window", "window_type" },
            { "Bandpower lower bound (Hz)", "f_bandpower_lower" },
            { "Bandpower upper bound (Hz)", "f_bandpower_upper" },
            { "STFT saving options", "stft_saving_option" }
        };
        #endregion 

        /* !!! Need to check for conditions of Channel_Idx and Gains_Arr
         */
        #region Constructor with default values 
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

            danger_rms_upperbound = 1.4;
            danger_rms_lowerbound = 0.6;
            warning_rms_upperbound = 1.2;
            warning_rms_lowerbound = 0.8;

            danger_color = Color.FromArgb(255, 0, 0);
            warning_color = Color.FromArgb(255, 255, 0);
            normal_color = Color.FromArgb(0, 255, 0);

            max_sec_plt = 10; 
            channel_gain_str = "1;1;1";
            display_channel_sep = 5;
            display_rms_sep = 0; 

            refresh_display = true;
            display_quality = Quality.High;

            nsec_fft = 2.0;
            per_overlap = 90;   
            window_type = WindowType.Hanning;
            f_bandpower_lower = 2;
            f_bandpower_upper = 8;
            stft_saving_option = false;

            danger_lbp_upperbound = 6E-7;
            danger_lbp_lowerbound = -1.0;
            warning_lbp_upperbound = 4.5E-7;
            warning_lbp_lowerbound = 0.0; 


            rms_lvl_reset_sec = 2;
            rms_lvl_max_sec = 60 * 10;
            rms_lvl_reset_point = (int) (Fs * rms_lvl_reset_sec);
            rms_lvl_max_point = (int) (rms_lvl_max_sec / rms_lvl_reset_sec); 
        }
        #endregion

        #region Initialization after prompt results 
        public void CompleteInitialize()
        {
            InitializeWelcomeMessage();

            InitializeChannelIndex();

            InitializeRMSMessage(); 
            output_file_name = Path.Combine(output_folder, output_file_name);

            max_pnt_plt = (int)(Fs * max_sec_plt);

            InitializeDisplayGains();

            InitializeFFTParameters();
        }

        public void InitializeWelcomeMessage()
        {
            WelcomeMessage = String.Format("+ Connected to HOST@{0} - PORT@{1}\r\n", hostname, port);
            WelcomeMessage += "\t Sampling frequency: " + Fs + "\r\n";
            WelcomeMessage += "\t Number of samples/epoch:" + nsamp_per_block + "\r\n";
        }

        public void InitializeChannelIndex()
        {
            string[] chans = channels2plt.Split(';');
            chan_idx2plt = new int[chans.Length];
            for (int ich = 0; ich < chans.Length; ich++)
            {
                int.TryParse(chans[ich], out chan_idx2plt[ich]);
            }
            nchan = chan_idx2plt.Length;
            WelcomeMessage += "+ Channels to plot: Ch" + string.Join("   Ch", chan_idx2plt) + "\r\n";
        }

        public void InitializeRMSMessage()
        {
            WelcomeMessage += "+ RMS calculations: \r\n";
            WelcomeMessage += String.Format("\t Window = {0} points ({1:0} ms)\r\n",
                nmax_queue_total, nmax_queue_total * 1000 / Fs);
            WelcomeMessage += String.Format("\t Warning ({2} < x < {0}, {3} > x > {1}) \r\n\t Danger (x < {2}, x > {3})\r\n",
                        warning_rms_lowerbound, warning_rms_upperbound, danger_rms_lowerbound, danger_rms_upperbound);
        }

        public void InitializeDisplayGains()
        {
            string[] gains_arr = channel_gain_str.Split(';');
            display_channel_gains = new double[gains_arr.Length];
            for (int ig = 0; ig < gains_arr.Length; ig++)
            {
                double.TryParse(gains_arr[ig], out display_channel_gains[ig]);
            }

            display_rms_gains = new double[nchan]; 
            for (int ich = 0; ich < nchan; ich++)
            {
                display_rms_gains[ich] = 1;  
            }

        }
        
        public void InitializeFFTParameters()
        {
            WelcomeMessage += "+ Frequency spectral calculations: \r\n";
            n_epoch = (int) Math.Pow(2, (int)Math.Log(Fs * nsec_fft,2));
            WelcomeMessage += String.Format("\t Length {0} s - nearest base 2 power = {1} points\r\n", nsec_fft, n_epoch);
            if (per_overlap < 0 || per_overlap > 100)
            {
                throw new System.ArgumentException("`per_overlap` is percent overlap between " +
                    "segments to calculate Fourier Transform, needs to be between 0 and 100"); 
            }
            n_skip = (int)(Fs * nsec_fft * (100 - per_overlap) / 100);
            WelcomeMessage += String.Format("\t Overlap {0:0.0}% - hence skip {1} points\r\n", per_overlap, n_skip);
            if (stft_saving_option)
            {
                string warning_stft_save = "WARNING: `stft_saving_option` = TRUE is not recommended" +
                    " for online plotting, due to time interference with continued data saving steps. " +
                    "This was only meant for checking the accuracy of FT at early steps.";
                Console.WriteLine(warning_stft_save);
                WelcomeMessage += "\t " + warning_stft_save + "\r\n"; 
            }
        }
        #endregion
        #region Specific Set methods during running application 
        public void Control_Channel_Gain(double direction)
        {
            for (int i = 0; i < display_channel_gains.Length; i++)
            {
                display_channel_gains[i] += d_gain * direction;
                display_channel_gains[i] = Math.Max(display_channel_gains[i], min_gain);
            }
        }
        public void Control_Channel_Separation(double direction)
        {
            display_channel_sep += d_sep * direction;
            display_channel_sep = Math.Max(display_channel_sep, min_sep);
        }

        public void Control_RMS_Gain(double direction)
        {
            for (int i = 0; i < display_rms_gains.Length; i++)
            {
                display_rms_gains[i] += d_gain * direction;
                display_rms_gains[i] = Math.Max(display_rms_gains[i], min_gain); 
            }
        }
        public void Control_RMS_Separation(double direction)
        {
            display_rms_sep += d_sep * direction;
            display_rms_sep = Math.Max(display_rms_sep, min_sep);
        }
        #endregion
        #region Get and Set methods via reflection 
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
        #endregion 
    }
}
