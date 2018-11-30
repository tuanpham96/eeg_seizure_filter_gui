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
        public string WelcomeMessage { get; set; }

        public string hostname { get; set; }
        public int port { get; set; }

        public double Fs { get; set; }
        public int nsamp_per_block { get; set; }
        public int[] chan_idx2plt { get; set; }
        public string channels2plt { get; set; }

        public int nchan { get; set; }  // to plot 
        public int total_nchan { get; set; }
        public int chunk_len { get; set; }

        public string output_folder { get; set; }
        public string output_file_name { get; set; }

        public double max_sec_plt { get; set; }
        public int nmax_queue_total { get; set; }
        public bool refresh_display { get; set; }
        public Quality display_quality { get; set; }

        public Color danger_color { get; set; }
        public Color warning_color { get; set; }
        public Color normal_color { get; set; }

        public double[] display_channel_vertgains { get; set; }
        public string channel_vertgain_str { get; set; }
        public double display_channel_vertoffset { get; set; }

        public int max_pnt_plt { get; set; }
        public string rms_vertgain_str { get; set; }
        public double[] display_rms_vertgains { get; set; }
        public double display_rms_vertoffset { get; set; }

        public double danger_rms_upperbound { get; set; }
        public double danger_rms_lowerbound { get; set; }
        public double warning_rms_upperbound { get; set; }
        public double warning_rms_lowerbound { get; set; }

        public double nsec_fft { get; set; }
        public double per_overlap { get; set; }
        public int n_epoch { get; set; }
        public int n_skip { get; set; }
        public WindowType window_type { get; set; }
        public double f_bandpower_lower { get; set; }
        public double f_bandpower_upper { get; set; }
        public bool scaling_psd { get; set; }
        public bool stft_saving_option { get; set; }

        public double danger_lbp_upperbound { get; set; }
        public double danger_lbp_lowerbound { get; set; }
        public double warning_lbp_upperbound { get; set; }
        public double warning_lbp_lowerbound { get; set; }

        public double rms_lvl_reset_sec { get; set; }
        public double rms_lvl_max_sec { get; set; }
        public int rms_lvl_reset_point { get; set; }
        public int rms_lvl_max_point { get; set; }

        public double lbp_lvl_reset_sec { get; set; }
        public double lbp_lvl_max_sec { get; set; }
        public int lbp_lvl_reset_point { get; set; }
        public int lbp_lvl_max_point { get; set; }

        public bool alarm_rate_plt_stack { get; set; }

        public double d_gain { get; set; }
        public double d_sep { get; set; }
        private double min_gain = 0.01, min_sep = 0;

        public int current_file_count { get; set; }
        public int save_every { get; set; }
        #endregion

        #region Dictionaries for categorical options 
        public Dictionary<string, bool> display_refresh_options { get; set; }

        public Dictionary<string, Quality> gear_quality_dict { get; set; }

        public Dictionary<string, WindowType> wintype_dict { get; set; }

        public Dictionary<string, bool> stft_saving_options { get; set; }

        public Dictionary<string, bool> scaling_psd_options { get; set; }

        public Dictionary<string, bool> alarm_plot_options { get; set; }

        public void InitializeDictionaries()
        {
            display_refresh_options = new Dictionary<string, bool>
            {
                { "Refreshable", true },
                { "Continuous", false }
            };

            gear_quality_dict = new Dictionary<string, Quality>
            {
                { "Low", Quality.Low },
                { "Medium", Quality.Medium },
                { "High", Quality.High },
                { "Highest", Quality.Highest }
            };

            wintype_dict = new Dictionary<string, WindowType>
            {
                { "Hanning", WindowType.Hanning },
                { "Hamming", WindowType.Hamming },
                { "Triangular", WindowType.Triangle },
                { "No window",  WindowType.Rectangle },
            };

            stft_saving_options = new Dictionary<string, bool>
            {
                { "Yes", true },
                { "No", false }
            };

            scaling_psd_options = new Dictionary<string, bool>
            {
                { "Yes", true },
                { "No", false }
            };

            alarm_plot_options = new Dictionary<string, bool>
            {
                { "Stacked Series", true },
                { "Line Series", false }
            };
        }
        #endregion

        #region Dictionary for parameters to querry 

        public struct PropertypAndFormType
        {
            public string prop_alias;
            public Form_Type form_type;
            public string dict_name; // for radiobutton option of the form type; will refer to the AppInputParameters parameter 
            public enum Form_Type { Textbox, RadiobuttonGroup, ColorButton };
            public PropertypAndFormType(string _prop_alias_, Form_Type _form_type_ = Form_Type.Textbox, string _dict_name_ = null)
            {
                prop_alias = _prop_alias_;
                form_type = _form_type_;
                dict_name = _dict_name_;
            }
            public bool IsTextBox()
            {
                return form_type.CompareTo(Form_Type.Textbox) == 0;
            }
            public bool IsRadiobuttonGroup()
            {
                return form_type.CompareTo(Form_Type.RadiobuttonGroup) == 0;
            }
            public bool IsColorButton()
            {
                return form_type.CompareTo(Form_Type.ColorButton) == 0;
            }
        }

        public Dictionary<string, Dictionary<string, PropertypAndFormType>> OptionSections = new Dictionary<string, Dictionary<string, PropertypAndFormType>>
        {
            { "Input and Output",       new Dictionary<string, PropertypAndFormType>{
                { "hostname",           new PropertypAndFormType("Host name") },
                { "port",               new PropertypAndFormType("Port") },
                { "Fs",                 new PropertypAndFormType("Sample frequency (Hz)") },
                { "total_nchan",        new PropertypAndFormType("Total number of channels") },
                { "nsamp_per_block",    new PropertypAndFormType("Number of samples / channel / epoch") },
                { "output_folder",      new PropertypAndFormType("Output folder") },
                { "output_file_name",   new PropertypAndFormType("Output folder") }
            }},
            { "General plot options",   new Dictionary<string, PropertypAndFormType> {
                { "channels2plt",       new PropertypAndFormType("Channels to display (choose only 2), separated by ;") },
                { "max_sec_plt",        new PropertypAndFormType("Display duration (seconds) [TimeSeries]") },
                { "refresh_display",    new PropertypAndFormType("Refresh display?", PropertypAndFormType.Form_Type.RadiobuttonGroup, "display_refresh_options") },
                { "display_quality",    new PropertypAndFormType("Display quality (affect performance)", PropertypAndFormType.Form_Type.RadiobuttonGroup, "gear_quality_dict") },
                { "d_gain",             new PropertypAndFormType("Display Vertical GAIN change step") },
                { "d_sep",              new PropertypAndFormType("Display Vertical OFFSET change step") },
                { "danger_color",       new PropertypAndFormType("Danger color", PropertypAndFormType.Form_Type.ColorButton) },
                { "warning_color",      new PropertypAndFormType("Warning color", PropertypAndFormType.Form_Type.ColorButton) },
                { "normal_color",       new PropertypAndFormType("Normal color", PropertypAndFormType.Form_Type.ColorButton) }
            }},
            { "Channel & RMS Plot",             new Dictionary<string, PropertypAndFormType> {
                { "channel_vertgain_str",       new PropertypAndFormType("Channel display vertical gains `ch0;ch1;ch0-ch1`") },
                { "display_channel_vertoffset", new PropertypAndFormType("Channel display vertical offset") },
                { "rms_vertgain_str",           new PropertypAndFormType("RMS display vertical gains `ch0;ch1`") },
                { "display_rms_vertoffset",     new PropertypAndFormType("RMS display vertical separation") },
                { "nmax_queue_total",           new PropertypAndFormType("RMS window (#points)") },
                { "danger_rms_upperbound",      new PropertypAndFormType("DANGER RMS upper bound") },
                { "danger_rms_lowerbound",      new PropertypAndFormType("DANGER RMS lower bound") },
                { "warning_rms_upperbound",     new PropertypAndFormType("WARNING RMS upper bound") },
                { "warning_rms_lowerbound",     new PropertypAndFormType("WARNING RMS lower bound") }
            }},
            { "Spectral Plot",              new Dictionary<string, PropertypAndFormType> {
                { "nsec_fft",               new PropertypAndFormType("STFT Length (s)") },
                { "per_overlap",            new PropertypAndFormType("STFT Overlap (%)") },
                { "window_type",            new PropertypAndFormType("STFT Window", PropertypAndFormType.Form_Type.RadiobuttonGroup, "wintype_dict") },
                { "f_bandpower_lower",      new PropertypAndFormType("Bandpower lower bound (Hz)") },
                { "f_bandpower_upper",      new PropertypAndFormType("Bandpower upper bound (Hz)") },
                { "stft_saving_option",     new PropertypAndFormType("STFT saving options", PropertypAndFormType.Form_Type.RadiobuttonGroup, "stft_saving_options") },
                { "scaling_psd",            new PropertypAndFormType("Scale PSD options",  PropertypAndFormType.Form_Type.RadiobuttonGroup, "stft_saving_options") },
                { "danger_lbp_upperbound",  new PropertypAndFormType("DANGER Band power upper bound") },
                { "danger_lbp_lowerbound",  new PropertypAndFormType("DANGER Band power lower bound") },
                { "warning_lbp_upperbound", new PropertypAndFormType("WARNING Band power upper bound") },
                { "warning_lbp_lowerbound", new PropertypAndFormType("WARNING Band power upper bound") }
            }},
            { "Alarm Tally Plot",           new Dictionary<string, PropertypAndFormType> {
                { "alarm_rate_plt_stack",   new PropertypAndFormType("Alarm rate plot options",  PropertypAndFormType.Form_Type.RadiobuttonGroup, "alarm_plot_options") },
                { "rms_lvl_reset_sec",      new PropertypAndFormType("Tally RMS Alarm every (s)") },
                { "rms_lvl_max_sec",        new PropertypAndFormType("Display length of RMS Alarm tally (s)") },
                { "lbp_lvl_reset_sec",      new PropertypAndFormType("Tally Spectral Alarm every (s)") },
                { "lbp_lvl_max_sec",        new PropertypAndFormType("Display length of Spectral Alarm tally (s)") },
            }}
                   

    };
        #endregion 

        /* !!! Need to check for conditions of Channel_Idx and Gains_Arr
         */
        #region Constructor with default values 
        public AppInputParameters()
        {
            InitializeDictionaries(); 
            hostname = "127.0.0.1";
            port = 1234;

            Fs = 512.0;
            nmax_queue_total = 64;
            nsamp_per_block = 4;
            total_nchan = 4;
            chunk_len = nsamp_per_block * total_nchan;
            channels2plt = "0;1";

            save_every = (int) (30 * 60 * Fs); // 30 mins 
            output_folder = @"C:\Users\Towle\Desktop\Tuan\general_towle\data";
            output_file_name = "testfile_TP.csv";

            normal_color = BrushToColor(System.Windows.Media.Brushes.Green);
            warning_color = BrushToColor(System.Windows.Media.Brushes.Gold);
            danger_color = BrushToColor(System.Windows.Media.Brushes.Red);

            danger_rms_upperbound = 1.4;
            danger_rms_lowerbound = 0.6;
            warning_rms_upperbound = 1.2;
            warning_rms_lowerbound = 0.8;

            max_sec_plt = 10; 
            channel_vertgain_str = "1;1;1";
            display_channel_vertoffset = 5;
            rms_vertgain_str = "1;1"; 
            display_rms_vertoffset = 0;

            d_gain = 0.1;
            d_sep = 25;

            refresh_display = true;
            display_quality = Quality.High;

            nsec_fft = 2.0;
            per_overlap = 90;   
            window_type = WindowType.Hanning;
            f_bandpower_lower = 2;
            f_bandpower_upper = 8;
            stft_saving_option = false;
            scaling_psd = true; 

            danger_lbp_upperbound = 6E-7;
            danger_lbp_lowerbound = -1.0;
            warning_lbp_upperbound = 4.5E-7;
            warning_lbp_lowerbound = 0.0;

            alarm_rate_plt_stack = false; 

            rms_lvl_reset_sec = 2;
            rms_lvl_max_sec = 60 * 10;
            rms_lvl_reset_point = (int) (Fs * rms_lvl_reset_sec);
            rms_lvl_max_point = (int) (rms_lvl_max_sec / rms_lvl_reset_sec);


            lbp_lvl_reset_sec = 10;
            lbp_lvl_max_sec = 60 * 10;
            lbp_lvl_reset_point = (int)(Fs * lbp_lvl_reset_sec);
            lbp_lvl_max_point = (int)(lbp_lvl_max_sec / lbp_lvl_reset_sec);

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
            string[] gains_arr_ch = channel_vertgain_str.Split(';');
            display_channel_vertgains = new double[gains_arr_ch.Length];
            for (int ig = 0; ig < gains_arr_ch.Length; ig++)
            {
                double.TryParse(gains_arr_ch[ig], out display_channel_vertgains[ig]);
            }

            string[] gains_arr_rms = rms_vertgain_str.Split(';');
            display_rms_vertgains = new double[gains_arr_rms.Length]; 
            if (gains_arr_rms.Length != nchan)
            {
                throw new System.ArgumentException("Number of elements in `rms_vertgain_str` needs to match `nchan` to plot"); 
            }
            for (int ig = 0; ig < gains_arr_rms.Length; ig++)
            {
                double.TryParse(gains_arr_rms[ig], out display_rms_vertgains[ig]);
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

        public static System.Drawing.Color BrushToColor(System.Windows.Media.SolidColorBrush br)
        {
            return Color.FromArgb(br.Color.A, br.Color.R, br.Color.G, br.Color.B);
        }

        public static System.Windows.Media.SolidColorBrush ColorToBrush(System.Drawing.Color color)
        {
            return new System.Windows.Media.SolidColorBrush
            {
                Color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)
            };
        }
        #endregion
        #region Specific Set methods during running application 
        public void Control_Channel_Gain(double direction)
        {
            for (int i = 0; i < display_channel_vertgains.Length; i++)
            {
                display_channel_vertgains[i] += d_gain * direction;
                display_channel_vertgains[i] = Math.Max(display_channel_vertgains[i], min_gain);
            }
        }
        public void Control_Channel_Separation(double direction)
        {
            display_channel_vertoffset += d_sep * direction;
            display_channel_vertoffset = Math.Max(display_channel_vertoffset, min_sep);
        }

        public void Control_RMS_Gain(double direction)
        {
            for (int i = 0; i < display_rms_vertgains.Length; i++)
            {
                display_rms_vertgains[i] += d_gain * direction;
                display_rms_vertgains[i] = Math.Max(display_rms_vertgains[i], min_gain); 
            }
        }
        public void Control_RMS_Separation(double direction)
        {
            display_rms_vertoffset += d_sep * direction;
            display_rms_vertoffset = Math.Max(display_rms_vertoffset, min_sep);
        }
        #endregion
        #region Get and Set methods via reflection 
        public object GetPropValue(string propName)
        {
            return this.GetType().GetRuntimeProperty(propName)?.GetValue(this);
        }
        public Type GetPropType(string propName)
        {
            return this.GetType().GetRuntimeProperty(propName)?.GetType();
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
