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

        public double nsec_fft { get; set; }
        public double per_overlap { get; set; }
        public int n_epoch { get; set; }
        public int n_skip { get; set; }
        public WindowType window_type { get; set; }
        public double f_bandpower_lower { get; set; }
        public double f_bandpower_upper { get; set; }
        public bool stft_saving_option { get; set; }

        public string WelcomeMessage { get; set; }
        public int nchan { get; set; }
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
            { "DANGER upper bound",  "danger_upperbound" },
            { "DANGER lower bound",  "danger_lowerbound" },
            { "WARNING upper bound", "warning_upperbound" },
            { "WARNING lower bound", "warning_lowerbound" },
            { "Display duration (seconds)", "nsec_plt" },
            { "Display gains", "gain_str" },
            { "Display separation", "display_sep" },
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
            display_quality = Quality.High;

            nsec_fft = 2.0;
            per_overlap = 90;   
            window_type = WindowType.Hanning;
            f_bandpower_lower = 2;
            f_bandpower_upper = 8;
            stft_saving_option = false;
        }
        #endregion

        #region Initialization after prompt results 
        public void CompleteInitialize()
        {
            InitializeWelcomeMessage();

            InitializeChannelIndex();

            InitializeRMSMessage(); 
            output_file_name = Path.Combine(output_folder, output_file_name);

            max_pnt_plt = (int)(Fs * nsec_plt);

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
                        warning_lowerbound, warning_upperbound, danger_lowerbound, danger_upperbound);
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
