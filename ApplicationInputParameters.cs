using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.IO;
using LiveCharts.Geared;

namespace seizure_filter
{    
    using static STFTCalculator; // To use the `STFTCalculator.WindowType` enum 

    /* ApplicationInputParameters: (application input parameters) contain configuration parameters to start the program 
     * 
     * In particular, `Prompt` is usually started to load configuration file or to create one.
     * Then `MainForm` would have an `APP_INP_PRM` object containing the current configuration 
     * parameters to start the program.
     * 
     * See `Prompt.AppInpPrm`, `Prompt.Result` and `MainForm.APP_INP_PRM` for more information  
     */
    public class ApplicationInputParameters
    {
        #region Application Input Parameters 
        /* Some notes and abbreviations  
         * + OpenVibe_Designer: OVDes
         * + OpenVibe_Acquisition: OVAcq
         * + OVDes's Sinus_Oscillator refers to testing simulations 
         * while OVAcq or OVDes's Acquisition_Client or OVDes's Channel_Selector
         * refers to actual set up 
         * + (own)  ->  this means that the parameter's value is created within 
         *              this file and not the following cases 
         * + (set)  ->  this means that the parameter is SET from the Prompt 
         *              or the configuration file 
         * + (inf)  ->  this means that the parameter is INFERED from another 
         *              parameter in the object 
         */

        /* Configuration file          
         * + config_path (set or inf):              configuration file path, usually set in the `Prompt` 
         * + default_config_path (own):             default configuration file path, preferable set to `private` 
         *                                          the default can be found in "_config_file.txt" in the working directory 
         * + current_directory (own):               current working directory as this application 
         * + config_delim (own):                    the configration deliminiter to separate between different columns 
         *                                          when a new configuration is being written in `WriteConfigurationFile`
         * + comment_str (own):                     the string to signify the commented line in the configuration file, 
         *                                          for both writing in `WriteConfigurationFile` and reading `ParseConfigurationFile`
         */
        public string config_path { get; set; }
        private string default_config_path;
        public string current_directory { get; }
        public string config_delim = "\t";
        public string comment_str = "#";

        /* Input connection parameters
         * + hostname (set):                        a string to describe host name, local would be: "127.0.0.1"
         * + port (set):                            an integer that matches with the port number from 
         *                                          OVDes's Matrix_Sender[TCP_Port], usually set to: 1234 
         * + Fs (set) :                             the sampling frequency (in Hz), usually set to 512 Hz;
         *                                          needs to match with either:
         *                                          * OVAcq->Driver_Properties[Sampling Frequency] or 
         *                                          * OVDes's Sinus_Oscillator[Sampling Frequency]                              
         * + nsamp_per_block (set):                 number of samples of each channel per block/chunk of data sent 
         *                                          from OVDes's Matrix_Sender; usually set to 4; 
         *                                          this needs to match with either:
         *                                          * OVAcq[Sample count per sent block] 
         *                                          * OVDes's Sinus_Oscillator[Generated epoch sample count]
         * + total_nchan (set):                     total number of channels being sent to OVDes's Matrix_Sender 
         *                                          usually set to 4; needs to match with either: 
         *                                          * Number of channels in OVDes's Channel_Selector[Channel List] 
         *                                          * OVDes's Sinus_Oscillator[Channel Count 
         * + chunk_len (inf):                       total length of each sent block/chunk
         *                                          basically = nsamp_per_block x total_nchan
         *                                          usually would be 16 
         */
        public string hostname { get; set; }
        public int port { get; set; }

        public double Fs { get; set; }
        public int nsamp_per_block { get; set; }
        public int total_nchan { get; set; }
        public int chunk_len { get; set; }

        /* Output file path and directory 
         * The data are saved in this file, refer to `MainForm.DrawAndReport` for more information
         * Change if desired a different format and different set of data to be saved 
         * + output_folder (set):                   output folder of the saved data 
         * + output_file (set):                     output file name of the saved data 
         */
        public string output_folder { get; set; }
        public string output_file_name { get; set; }

        /* Channel indices to plot  
         * + channels2plt (set):                    the string that contains the array of channel indices to plot,
         *                                          separated by a ";", usually set to "0;1" 
         * + chan_idx2plt (inf):                    array of channel indices to plot, currently set to only 2 channels 
         *                                          due to the difference plotting. but could be set for a more 
         *                                          general case but would need to change a few things. please refer 
         *                                          to `MainForm.InitializePlotSeries` for the recommendation. 
         *                                          also remember that these indices refer to the indices when the data
         *                                          are read in chunks, meaning the first channel would have index = 0, 
         *                                          even if the first channel's index in OVDes is 2 or something else
         * + nchan (inf):                           the number of channels to plot, not the total number of channels 
         */
        public string channels2plt { get; set; }
        public int nchan { get; set; }  // to plot, not the total 
        public int[] chan_idx2plt { get; set; }

        /* General plot options 
         * + max_sec_plt (set):                     maximum seconds of plotting for streaming time-series data, 
         *                                          usually around 10s is optimal; in particular for `channel_plots`, 
         *                                          `rms_plots` and `limbandpow_plots`. however, not for the alarm rate/tally 
         *                                          plots;  similar settings for the them are `rms_lvl_max_sec` and `lbp_lvl_max_sec`
         * + max_pnt_plt (inf):                     infered from `max_sec_plt` to have maximum number of data points to plot 
         * + refresh_display (set):                 refreshable option for the time-series plots (exccept for the alarm plots) 
         *                                          refer also to `display_refresh_options`, usually set to "true" (Refereshable) for performance 
         *                                          * Refereshable (true)   ->  the xlim would be updated only every `max_sec_plt`,
         *                                                                      like a scanning mode of streaming time series 
         *                                          * Continuous (false)    ->  xlim would be updated by LiveCharts automatically, 
         *                                                                      like a continuously streaming time series 
         *                                          for the alarm rate plots, this is set to Continuous (false) (refer to discussion in `MainForm.DrawAndReport`) 
         * + display_quality (set):                 display quality of all plots in the program, so that `LiveCharts.Geared` would know,
         *                                          usually setting to either "Medium" or "High" is optimal for performance purposes, 
         *                                          refer also to `gear_quality_dict` and see `MainForm.InitializePlotSeries`
         * + d_gain (set):                          the step of vertical display gain changes, 
         *                                          adjust based on observation of the streaming data range 
         * + d_offset (set):                        the step of vertical display offset changes, 
         *                                          adjust based on observation of the streaming data range 
         * + min_gain (own):                        the minimum allowed value for display vertical gain 
         * + min_offset (own):                      the minimum allowed value for display vertical gain 
         */
        public double max_sec_plt { get; set; }
        public int max_pnt_plt { get; set; }
        public bool refresh_display { get; set; }
        public Quality display_quality { get; set; }
        public double d_gain { get; set; }
        public double d_offset { get; set; }
        private double min_gain = 0.01, min_offset = 0;

        /* General alarm options 
         * + n_lvls (own):                          number of levels (Normal=0, Warning=1, Danger=2) 
         * + danger_color (set):                    the color representing Danger, usually red
         * + warning_color (set):                   the color representing Warning, usually yellow/gold
         * + normal_color (set):                    the color representing Normal, usually green
         */
        public int n_lvls = 3;
        public Color danger_color { get; set; }
        public Color warning_color { get; set; }
        public Color normal_color { get; set; }

        /* Channel time series display options for `channel_plots` 
         * refer also to `InitializeDisplayGains`, `Control_Channel_Verical_Gain`, `Control_Channel_Verical_Offset` 
         * + channel_vertgain_str (set):            the vertical display gains for channel time series plots, 
         *                                          including also the difference data (hence 3 values), 
         *                                          separated by ";" like "1;1;1" - meaning the difference data 
         *                                          can be set to display with a higher gain for clearer inspection  
         * + display_channel_vertgains (inf):       the array containing the values parsed from `channel_vertgain_str` 
         * + display_channel_vertoffset (set):      the value of vertical display offset to easily separate the display 
         *                                          of the data in the `channel_plots` 
         */
        public string channel_vertgain_str { get; set; }
        public double[] display_channel_vertgains { get; set; }
        public double display_channel_vertoffset { get; set; }

        /* Root-mean-square (RMS) calculation and plot options for `rms_plots`, `rms_alarm--` 
         * refer also to `InitializeDisplayGains`, `Control_RMS_Verical_Gain`, `Control_RMS_Verical_Offset` 
         * + nmax_queue_total (set):                number of points for `RMSCalculator.data_queue`, also used 
         *                                          for the sliding window size of RMS calculations; for example:
         *                                          with 512 Hz, 64 points means 125ms and 128 points means 0.25s 
         * + rms_vertgain_str (set):                the vertical display gains for RMS time series plots, the values
         *                                          are separated by ";" like "1;1". The length of this should match with 
         *                                          `nchan` to plot; also for initial plotting to decide the alarm 
         *                                          threshold, set this "1;1" and gain=0 to inspect the accurate thresholds 
         * + display_rms_vertgains (inf):           the array containing the values parsed from `rms_vertgain_str` 
         * + display_rms_vertoffset (set):          the value of vertical display offset to easily separate the display 
         *                                          of the data in `rms_plots`
         */
        public int nmax_queue_total { get; set; }
        public string rms_vertgain_str { get; set; }
        public double[] display_rms_vertgains { get; set; }
        public double display_rms_vertoffset { get; set; }

        /* RMS Alarm thresholds:
         * + danger_rms_upperbound (set):           upperbound of RMS danger range 
         * + danger_rms_lowerbound (set):           lowerbound of RMS danger range 
         * + warning_rms_upperbound (set):          upperbound of RMS warning range 
         * + warning_rms_lowerbound (set):          lowerbound of RMS warning range 
         * The requirement of the range is (checked in `CheckForAlarmRange`):         
         * + danger_lower < warning_lower < warning_upper < danger_upper
         * The results are (refer to `MainForm.ReturnRMSLevel`): 
         * >> NORMAL: anything within [warning_lower, warning_upper] 
         * >> WARNING: anything out of that range, but still within [danger_lower, danger_upper] 
         * >> DANGER: otherwise    
         */
        public double danger_rms_upperbound { get; set; }
        public double danger_rms_lowerbound { get; set; }
        public double warning_rms_upperbound { get; set; }
        public double warning_rms_lowerbound { get; set; }

        /* Short-time Fourier Transform (STFT) and Limited-band power (LBP) calculation 
         * refer also to `InitializeSpectralParameters` and `STFTCalculator` constructor 
         * + nsec_fft (set):                        the length of the data to perform FFT on, in seconds
         * + n_epoch (inf):                         the actual length of the data (in # data points) to perform
         *                                          FFT on, which needs to be a power of 2 closest to `nsec_fft`
         * + per_overlap (set):                     percent of overlap between the epochs to calculate STFT on
         * + n_skip (inf):                          the number of points to skip between the epochs, infered from `per_overlap`
         * + window_type (set):                     the type of taper window to apply on the epoch data before
         *                                          calculating FFT on, refer to `STFTCalculator.WindowType` and `wintype_dict`; 
         *                                          usually set "Hanning" or "Hamming" 
         * + f_bandpower_lower (set):               the lower frequency of the band to calculate the power from; 
         *                                          usually set to 2 or 3Hz in the interest of detecting seizure 
         * + f_bandpower_upper (set):               the upper frequency of the band to calculate the power from; 
         *                                          usually set to 7 or 8Hz in the interest of detecting seizure 
         * + scaling_psd (set):                     the option to scale the power spectrum density when calculting power; 
         *                                          refer to `scaling_psd_options`; be careful with this because this would
         *                                          affect the actual values of threshold selections 
         * + stft_saving_option (set):              the option to save the STFT calculated, should just set to "false" 
         *                                          (`stft_saving_options`) because the way of implementing saving it right now
         *                                          would intervene with the plotting. should set to "true" only if want to 
         *                                          check for the first few calculated epochs; or implement another way to 
         *                                          save the spectral of the data (refer to `STFTCalculator.WriteToFile`) 
         */
        public double nsec_fft { get; set; }
        public int n_epoch { get; set; }
        public double per_overlap { get; set; }
        public int n_skip { get; set; }
        public WindowType window_type { get; set; }
        public double f_bandpower_lower { get; set; }
        public double f_bandpower_upper { get; set; }
        public bool scaling_psd { get; set; }
        public bool stft_saving_option { get; set; }

        /* LBP Alarm thresholds:
         * + danger_lbp_upperbound (set):           upperbound of LBP danger range 
         * + danger_lbp_lowerbound (set):           lowerbound of LBP danger range 
         * + warning_lbp_upperbound (set):          upperbound of LBP warning range 
         * + warning_lbp_lowerbound (set):          lowerbound of LBP warning range 
         * The requirement of the range is (checked in `CheckForAlarmRange`):         
         * + danger_lower < warning_lower < warning_upper < danger_upper
         * The results are (refer to `MainForm.ReturnLBPLevel`): 
         * >> NORMAL: anything within [warning_lower, warning_upper] 
         * >> WARNING: anything out of that range, but still within [danger_lower, danger_upper] 
         * >> DANGER: otherwise    
         */
        public double danger_lbp_upperbound { get; set; }
        public double danger_lbp_lowerbound { get; set; }
        public double warning_lbp_upperbound { get; set; }
        public double warning_lbp_lowerbound { get; set; }

        /* Alarm rate plot options 
         * + alarm_rate_plt_stack (set):            the option to either plot the alarm rate/tally (refer to `alarm_plot_options`) as either:
         *                                          * Stacked Series (true): on top of each other (Normal bottom, Warning middle, Danger top) or 
         *                                          * Line Series (false): just as regular line plot like other plots 
         *                                          refer also to `MainForm.InitializePlotSeries` for details of discussion and implementation
         * RMS alarm rate plot options 
         * + rms_lvl_reset_sec (set):               the amount of time (in seconds) to reset the tally of RMS alarms, this means that the epochs 
         *                                          to tally of RMS alarms are not overlapping with each other like RMS or STFT calculations 
         * + rms_lvl_reset_point (inf):             number of data points infered from `rms_lvl_reset_sec` 
         * + rms_lvl_max_sec (set):                 the maximum duration to plot in `rms_alarm_plots`
         * + rms_lvl_max_point (inf):               number of data points infered from `rms_lvl_max_sec`   
         * LBP alarm rate plot options 
         * + lbp_lvl_reset_sec (set):               the amount of time (in seconds) to reset the tally of LBP alarms, this means that the epochs 
         *                                          to tally of LBP alarms are not overlapping with each other like RMS or STFT calculations 
         * + lbp_lvl_reset_point (inf):             number of data points infered from `lbp_lvl_reset_sec` 
         * + lbp_lvl_max_sec (set):                 the maximum duration to plot in `lbp_alarm_plots`
         * + lbp_lvl_max_point (inf):               number of data points infered from `lbp_lvl_max_sec` 
         * 
         */
        public bool alarm_rate_plt_stack { get; set; }

        public double rms_lvl_reset_sec { get; set; }
        public double rms_lvl_max_sec { get; set; }
        public int rms_lvl_reset_point { get; set; }
        public int rms_lvl_max_point { get; set; }

        public double lbp_lvl_reset_sec { get; set; }
        public double lbp_lvl_max_sec { get; set; }
        public int lbp_lvl_reset_point { get; set; }
        public int lbp_lvl_max_point { get; set; }

        /* Welcoming message summarizing the above parameters 
         * is displayed in the `log` of `MainForm`
         */
        public string WelcomeMessage { get; set; }
        #endregion Application Input Parameters 

        #region Dictionaries for categorical options 
        /* These are option dictionaries for some of the parameters that should not 
         * just accept any arbitrary values, and in the `Prompt` they will be 
         * presented as `RadioButton` groups, see `OptionSections` and `Promp.MainPrompt` 
         * + display_refresh_options:               refreshable display options, 
         *                                          -> `refresh_display`
         * + gear_quality_dict:                     display qualities from `LiveCharts.Geared.Quality`,
         *                                          -> `display_quality`
         * + wintype_dict:                          taper window type options from `STFTCalculator.WindowType`, 
         *                                          -> `window_type`
         * + scaling_psd_options:                   scaling power spectral density options, 
         *                                          -> `scaling_psd`
         * + stft_saving_options:                   stft calculations saving options, 
         *                                          -> `stft_saving_option`
         * + alarm_plot_options:                    alarm rate/tally plot options, 
         *                                          -> `alarm_rate_plt_stack`
         */
        public Dictionary<string, bool> display_refresh_options { get; set; }
        public Dictionary<string, Quality> gear_quality_dict { get; set; }
        public Dictionary<string, WindowType> wintype_dict { get; set; }
        public Dictionary<string, bool> scaling_psd_options { get; set; }
        public Dictionary<string, bool> stft_saving_options { get; set; }
        public Dictionary<string, bool> alarm_plot_options { get; set; }

        // Initilize the option dictionaries  
        public void InitializeOptionDictionaries()
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
            // For other implemented `WindowType` options 
            foreach (WindowType _wintype_ in Enum.GetValues(typeof(WindowType)))
            {
                if (!wintype_dict.ContainsValue(_wintype_))
                {
                    wintype_dict.Add(_wintype_.ToString().Replace('_', '-'), _wintype_);
                }
            }
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
        #endregion Dictionaries for categorical options 

        #region Dictionary for parameters to querry in `Prompt.MainPrompt` 

        /* PropertypAndFormType: struct to specifiy the querried parameter's description, form type and option dictionary name
         * see `OptionSections` for usage
         * + prop_alias:        property alias/description that will appear on the `Prompt.MainPrompt` left side
         * + form_type:         the form type for the property, currently 3 options:
         *                      * Textbox[default]: most of the options are `System.Windows.Forms.TextBox` objects, whose
         *                                          `Text` property records the value to be set for the corresponding querried parameter 
         *                      * RadiobuttonGroup: the parameters whose values are from the option dictionaries (like `gear_quality_dict`), 
         *                                          will be presented in `Prompt.MainPrompt` as a group/panel of `System.Windows.Forms.RadioButtons` objects
         *                      * ColorButton:      only for color parameters like `danger_color`, which would appear as a `System.Windows.Forms.Button` 
         *                                          object in `Prompt.MainPrompt` to prompt a `ColorDialog` to choose color from 
         * + dict_name:         the dictionary name corresponding to the correct option dictionary name, for example: 
         *                      if the querried parameter is `window_type` then this would be the string "wintype_dict" 
         *                      this means that only `form_type=RadiobuttonGroup` would have meaningful `dict_name`
         *                      while the other 2 `Form_Type` options would lead to a `null` [default] value for `dict_name` 
         */
        public struct PropertypAndFormType
        {
            public string prop_alias;
            public Form_Type form_type;
            public string dict_name; // for radiobutton option of the form type; will refer to one of the ApplicationInputParameters option dictionaries  
            public enum Form_Type { Textbox, RadiobuttonGroup, ColorButton };

            public PropertypAndFormType(string _prop_alias_, Form_Type _form_type_ = Form_Type.Textbox, string _dict_name_ = null)
            {
                prop_alias = _prop_alias_;
                form_type = _form_type_;
                dict_name = _dict_name_;
            }

            // These functions below are just for easier checking in `Prompt.ParameterInputPrompt`
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

        /* OptionSections: a nested dictionary for querried parameters (set), split up in sections 
         * --- Key -------- Value { --- Nested key ---- Nest value: PropertypAndFormType ----------------------------- } ---
         *                                              |---> prop_alias:   Description name for the querried parameter
         * Section_Name ---> Querried_Parameter_Name ---|---> form_type:    The form type for the querried parameter
         *                                              |---> dict_name:    The corresponding name of the option dictionary                  
         */
        public Dictionary<string, Dictionary<string, PropertypAndFormType>> OptionSections = new Dictionary<string, Dictionary<string, PropertypAndFormType>>
        {
            { "Input and Output",       new Dictionary<string, PropertypAndFormType>{
                { "config_path",                new PropertypAndFormType("Configuration path") },
                { "hostname",                   new PropertypAndFormType("Host name") },
                { "port",                       new PropertypAndFormType("Port") },
                { "Fs",                         new PropertypAndFormType("Sample frequency (Hz)") },
                { "total_nchan",                new PropertypAndFormType("Total number of channels") },
                { "nsamp_per_block",            new PropertypAndFormType("Number of samples / channel / epoch") },
                { "output_folder",              new PropertypAndFormType("Output folder") },
                { "output_file_name",           new PropertypAndFormType("Output folder") }
            }},
            { "General plot options",   new Dictionary<string, PropertypAndFormType> {
                { "channels2plt",               new PropertypAndFormType("Channels to display (choose only 2), separated by ;") },
                { "max_sec_plt",                new PropertypAndFormType("Display duration (seconds) [TimeSeries]") },
                { "refresh_display",            new PropertypAndFormType("Refresh display?", PropertypAndFormType.Form_Type.RadiobuttonGroup, "display_refresh_options") },
                { "display_quality",            new PropertypAndFormType("Display quality (affect performance)", PropertypAndFormType.Form_Type.RadiobuttonGroup, "gear_quality_dict") },
                { "d_gain",                     new PropertypAndFormType("Display Vertical GAIN change step") },
                { "d_offset",                   new PropertypAndFormType("Display Vertical OFFSET change step") },
                { "danger_color",               new PropertypAndFormType("Danger color", PropertypAndFormType.Form_Type.ColorButton) },
                { "warning_color",              new PropertypAndFormType("Warning color", PropertypAndFormType.Form_Type.ColorButton) },
                { "normal_color",               new PropertypAndFormType("Normal color", PropertypAndFormType.Form_Type.ColorButton) }
            }},
            { "Channel & RMS Plot",     new Dictionary<string, PropertypAndFormType> {
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
            { "Spectral Plot",          new Dictionary<string, PropertypAndFormType> {
                { "nsec_fft",                   new PropertypAndFormType("STFT Length (s)") },
                { "per_overlap",                new PropertypAndFormType("STFT Overlap (%)") },
                { "window_type",                new PropertypAndFormType("STFT Window", PropertypAndFormType.Form_Type.RadiobuttonGroup, "wintype_dict") },
                { "f_bandpower_lower",          new PropertypAndFormType("Bandpower lower bound (Hz)") },
                { "f_bandpower_upper",          new PropertypAndFormType("Bandpower upper bound (Hz)") },
                { "stft_saving_option",         new PropertypAndFormType("STFT saving options", PropertypAndFormType.Form_Type.RadiobuttonGroup, "stft_saving_options") },
                { "scaling_psd",                new PropertypAndFormType("Scale PSD options",  PropertypAndFormType.Form_Type.RadiobuttonGroup, "stft_saving_options") },
                { "danger_lbp_upperbound",      new PropertypAndFormType("DANGER Band power upper bound") },
                { "danger_lbp_lowerbound",      new PropertypAndFormType("DANGER Band power lower bound") },
                { "warning_lbp_upperbound",     new PropertypAndFormType("WARNING Band power upper bound") },
                { "warning_lbp_lowerbound",     new PropertypAndFormType("WARNING Band power upper bound") }
            }},
            { "Alarm Tally Plot",       new Dictionary<string, PropertypAndFormType> {
                { "alarm_rate_plt_stack",       new PropertypAndFormType("Alarm rate plot options",  PropertypAndFormType.Form_Type.RadiobuttonGroup, "alarm_plot_options") },
                { "rms_lvl_reset_sec",          new PropertypAndFormType("Tally RMS Alarm every (s)") },
                { "rms_lvl_max_sec",            new PropertypAndFormType("Display length of RMS Alarm tally (s)") },
                { "lbp_lvl_reset_sec",          new PropertypAndFormType("Tally Spectral Alarm every (s)") },
                { "lbp_lvl_max_sec",            new PropertypAndFormType("Display length of Spectral Alarm tally (s)") },
            }}
        };

        #endregion Dictionary for parameters to querry in `Prompt.MainPrompt` 

        #region Application Input Parameters: object constructor with configuration file path 
        /* ApplicationInputParameters constructor: initialize option dictionaries and parse configuration file 
         * + LAYOUT: 
         *      - Initialize option dictionaries 
         *      - Read a the configuration file, and change the identity if needed 
         * + INPUT:
         *      - create_new_config:    the option to create a new configuration file 
         *                              * if "true" then parse the default configuration file, save to the new path `_config_path_`
         *                              * if "false" then parse the `_config_path_` file 
         *      - _config_path_:        the configuration path to either save the new configuration to, or modify an 
         *                              existing configuration path 
         */
        public ApplicationInputParameters(bool create_new_config, string _config_path_)
        {
            InitializeOptionDictionaries();

            current_directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            default_config_path = current_directory + "\\_config_file.txt";

            if (create_new_config)
            {
                ParseConfigurationFile(default_config_path);
                config_path = _config_path_;
            }
            else
            {
                config_path = _config_path_;
                ParseConfigurationFile(config_path);
            }
            // To avoid overwriting default configuration file 
            if (string.Compare(config_path, default_config_path) == 0)
            {
                config_path = current_directory + "\\customized_config_file.txt";
            }
        }
        #endregion Application Input Parameters: object constructor with configuration file path 

        #region Configuration file writing and reading 
        /** Configuration file template: given comment starts with "#" (not evaluated) and column separated by tab "/t" 
         * + An example below: 
         *              # Configuration, generated on Monday, December 10, 2018 3:18 PM
         *              # Change the Value column to induce effect
         *              # Property              Type	                    Value	    Description
         *              hostname	            System.String	            127.0.0.1	<NAN>
         *              refresh_display	        System.Boolean	            True	    Refreshable
         *              display_quality	        LiveCharts.Geared.Quality	High	    High
         *              danger_color	        System.Drawing.Color	    #FF0080	    Color [A=255, R=255, G=0, B=128]
         *              danger_lbp_upperbound	System.Double	            6E-07	    <NAN>
         * + Explanation of the columns:
         *  (nec) : necessary to parse the parameter
         *  (add) : not necessary when parsing the parameter, only there to provide additional information 
         *      - Property (nec):       the name of the querried parameter, all the ones in `OptionSections` 
         *      - Type (add):           the type information of the parameter's value
         *      - Value (nec):          the actual value of the parameter, 
         *                              * usually via `whatever_the_value_of_parameter.ToString()` 
         *                              * if the parameter's value is from an option dictionary, this is one of the 
         *                                      VALUES in the dictionary (not the KEYS: the descriptor) 
         *                              * if this is a color, this value is a string representation of the Html color
         *      - Description (add):    this is just additional information on the value:
         *                              * [default] <NAN>: meaning there's no additional description here
         *                              * if the parameter's value is from an option dictionary, this is one of the 
         *                                      KEYS (descriptor) in the dictionary (not the VALUES)  
         *                              * if this is a color, the method is `Color.ToString()` to get either
         *                                      the name of the color, or an ARGB array of it
         * 
         */

        /* WriteConfigurationFile: write to a new configuration file or overwrite an existing one 
         * refer to the previous description of the File Template for more details of the file format 
         * + LAYOUT:
         *      - Write headers to describe:
         *          * When the configuration file was generated
         *          * The names of the columns 
         *      - Write the querried parameters: name, type, value, additional description
         * + INPUT:
         *      - _config_path_:        the configuration file path to write the parameters to 
         *      - description_dict:     a dictionary, whose keys are the names of the querried parameters,
         *                              and values are the additional description (see discussion of "Description"
         *                              column in the File Template), refer also to `Prompt.ParameterInputPrompt` 
         *                              for how this is constructed/implemented 
         */
        public void WriteConfigurationFile(string _config_path_, Dictionary<string, string> description_dict)
        {
            string cmt = comment_str + " ";
            // Header 
            File.WriteAllText(_config_path_, cmt + string.Format("Configuration, generated on {0:f}\n", DateTime.Now));
            File.AppendAllText(_config_path_, cmt + "Change the Value column to induce effect\n");
            File.AppendAllText(_config_path_, cmt + "Property" + config_delim +
                                                    "Type" + config_delim +
                                                    "Value" + config_delim +
                                                    "Description" + "\n");
            foreach (var item in description_dict)
            {
                string prop_name = item.Key;
                string description = description_dict[prop_name];
                Type _type_ = GetPropType(prop_name);
                var _val_ = GetPropValue(prop_name);

                // Write the NAME, TYPE, VALUE, DESCRIPTION of the parameter 
                File.AppendAllText(_config_path_, prop_name + config_delim +
                                                _type_ + config_delim +
                                                (_type_ == typeof(Color) ? ColorTranslator.ToHtml((Color)_val_) : _val_) + config_delim +
                                                description + "\n");

            }
        }
        /* ParseConfigurationFile: parse the input configuration file to the parameters 
         * refer to the previous description of the File Template for more details of the file format 
         * + LAYOUT: 
         *      - Read the valid lines (not beginning with the comment tag) 
         *      - FOR each valid line, 
         *          * Obtain the parameter's name (1st column) and value (3rd column) 
         *          * Then set the value for the property with the same name in the object
         * + INPUT: 
         *      - _config_path_:        the configuration path to read the file from in order to
         *                              parse the parameters  
         */
        public void ParseConfigurationFile(string _config_path_)
        {
            foreach (var line in File.ReadLines(_config_path_))
            {
                if (!line.StartsWith(comment_str))
                {
                    var str_arr = line.Split(new string[] { config_delim }, StringSplitOptions.RemoveEmptyEntries);
                    string prop_name = str_arr[0];
                    string prop_val = str_arr[2];
                    Type prop_type = GetPropType(prop_name);

                    try
                    {
                        if (prop_type.IsEnum)
                        {
                            SetPropValue(prop_name, Enum.Parse(prop_type, prop_val));
                        }
                        else if (prop_type == typeof(Color))
                        {
                            SetPropValue(prop_name, ColorTranslator.FromHtml(prop_val));
                        }
                        else
                        {
                            SetPropValue(prop_name, prop_val);
                        }
                    } 
                    catch (Exception e)
                    {
                        throw new Exception(string.Format(
                            "Exception thrown in ParseConfigurationFile, " +
                            "could not parse `{0}` with value=`{1}`. \n " +
                            "The original exception is {2} \n", prop_name, prop_val, e.StackTrace));
                    }
                }
            }
        }

        #endregion Configuration file writing and reading 

        #region Initialization after prompt results 
        /* CompleteInitialize: complete the initialization of an ApplicationInputParameters object 
         * Call this before actually running the application to get all infered parameters 
         * + LAYOUT:
         *      - Initialize welcome message for `MainForm.log`
         *      - Initialize channel indices 
         *      - Check for both RMS and LBP alarm ranges 
         *      - Initialize RMS calculations and plotting options 
         *      - Complete display gain initialization 
         *      - Complete spectral related parameter initialization and checking 
         *      - Complete any other additional initialization 
         */
        public void CompleteInitialize()
        {
            InitializeWelcomeMessage();
            InitializeChannelIndex();
            CheckForAlarmRange();
            InitializeRMSParameters(); 

            // A few more modifcations and inferred set ups for parameters 
            output_file_name = Path.Combine(output_folder, output_file_name);
            chunk_len = nsamp_per_block * total_nchan;
            max_pnt_plt = (int)(Fs * max_sec_plt);

            InitializeDisplayGains();
            InitializeSpectralParameters();            
        }

        /* InitializeWelcomeMessage: initialize the welcome message for `MainForm.log` 
         * + LAYOUT: 
         *      - Initialize `WelcomeMessage`
         *      - Concatenate to it: `hostname`, `port`, `Fs`, `nsamp_per_block` 
         */
        public void InitializeWelcomeMessage()
        {
            WelcomeMessage = String.Format("+ Connected to HOST@{0} - PORT@{1}\r\n", hostname, port);
            WelcomeMessage += "\t Sampling frequency: " + Fs + " Hz\r\n";
            WelcomeMessage += "\t Number of samples/epoch:" + nsamp_per_block + "\r\n";
        }

        /* InitializeChannelIndex: initialize the channel indices used for plotting 
         * + LAYOUT: 
         *      - Parse the `channels2plt` string to the double array `chan_idx2plt`
         *      - Initialize `nchan`
         *      - Concatenate to `WelcomeMessage`: `chan_idx2plt` 
         */
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

        /* CheckForAlarmRange: check for the alarm threshold range 
         * + LAYOUT: 
         *      - The conditions to check are: 
         *          danger_lower < warning_lower < warning_upper < danger_upper
         *      - If that's satisfied: the classifications would be 
         *          * NORMAL: anything within [warning_lower, warning_upper] 
         *          * WARNING: anything out of that range, but still within [danger_lower, danger_upper] 
         *          * DANGER: otherwise
         */
        public void CheckForAlarmRange()
        {
            // Just for easier management of check performing, as well as error message generation 
            Dictionary<string, double[]> ranges = new Dictionary<string, double[]>()
            {
                { "RMS", new double[]
                    { danger_rms_lowerbound, warning_rms_lowerbound, warning_rms_upperbound, danger_rms_upperbound } },
                { "LBP", new double[]
                    { danger_lbp_lowerbound, warning_lbp_lowerbound, warning_lbp_upperbound, danger_lbp_upperbound } }, 
            };

            // Check if an array is strictly ascending ordered set
            // meaning d[i+1] > d[i] for all i 
            bool IsStrictlyAscending(double[] d)
            {
                bool it_is = true;
                for (int i = 0; i < d.Length-1; i++)
                {
                    if (d[i] >= d[i+1]) { it_is = false; break; }
                }
                return it_is; 
            }

            // Check for: danger_lower < warning_lower < warning_upper < danger_upper
            foreach (var item in ranges)
            {
                string value_type = item.Key;
                var range_dict = item.Value; 
                if (!IsStrictlyAscending(range_dict))
                {
                    throw new System.ArgumentException(
                        string.Format("The array of {0} would need to follow a strict ascending order (not equals)", value_type)); 
                }
            }
        }

        /* InitializeRMSParameters: complete initialization of RMS-related parameters and update `WelcomeMessage`
         * + LAYOUT: 
         *      - Convert `rms_lvl_reset_sec` and `rms_lvl_max_sec` to the equivalent in number of points 
         *      - Concatenate to `WelcomeMessage`: `nmax_queue_total`, {RMS alarm thresholds}, `rms_lvl_reset_sec`, `rms_lvl_max_sec`
         */
        public void InitializeRMSParameters()
        {
            WelcomeMessage += "+ RMS calculations: \r\n";
            WelcomeMessage += String.Format("\t Window = {0} points ({1:0} ms)\r\n",
                nmax_queue_total, nmax_queue_total * 1000 / Fs);

            // RMS alarm thresholds and plot options 
            WelcomeMessage += String.Format("\t Warning ({2} < x < {0}, {3} > x > {1}) \r\n\t Danger (x < {2}, x > {3})\r\n",
                        warning_rms_lowerbound, warning_rms_upperbound, danger_rms_lowerbound, danger_rms_upperbound);
            rms_lvl_reset_point = (int)(Fs * rms_lvl_reset_sec);
            rms_lvl_max_point = (int)(rms_lvl_max_sec / rms_lvl_reset_sec);
            WelcomeMessage += String.Format("\t The alarm rate plot is updated every {0} seconds (non-overlapping)," +
                        " with maximum plotting limit of {1} minutes \r\n", rms_lvl_reset_sec, rms_lvl_max_sec / 60.0); 
        }

        /* InitializeDisplayGains: parse the strings containing vertical gains to vertical gain arrays 
         * + LAYOUT:
         *      - Parse `channel_vertgain_str` to `display_channel_vertgains`
         *      - Parse `rms_vertgain_str` to `display_rms_vertgains` 
         */
        public void InitializeDisplayGains()
        {
            // Display gains for `channel_plots`
            string[] gains_arr_ch = channel_vertgain_str.Split(';');
            display_channel_vertgains = new double[gains_arr_ch.Length];
            for (int ig = 0; ig < gains_arr_ch.Length; ig++)
            {
                double.TryParse(gains_arr_ch[ig], out display_channel_vertgains[ig]);
            }

            // Display gains for `rms_plots`  
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

        /* InitializeSpectralParameters: complete initialization of spectral-related parameters and update `WelcomeMessage`
         * + LAYOUT: 
         *      - Find the closet power of 2 to `nsec_fft` to become `n_epoch` 
         *      - Infer `n_skip` from `per_overlap` 
         *      - Convert `lbp_lvl_reset_sec` and `lbp_lvl_max_sec` to the equivalent in number of points 
         *      - Concatenate to `WelcomeMessage`: 
         *              nsec_fft, n_epoch, per_overlap, n_skip
         *              window_type, stft_saving_option, f_bandpower_lower, f_bandpower_upper, scaling_psd
         *              {LBP alarm thresholds} 
         *              lbp_lvl_reset_sec, lbp_lvl_max_sec 
         */
        public void InitializeSpectralParameters()
        {
            WelcomeMessage += "+ Frequency spectral calculations: \r\n";
            // Number of data points for each epoch to perform FFT on 
            n_epoch = (int) Math.Pow(2, (int)Math.Log(Fs * nsec_fft,2));
            WelcomeMessage += String.Format("\t Length {0} s - nearest base 2 power = {1} points\r\n", nsec_fft, n_epoch);

            // Number of skipped points between STFT epochs 
            if (per_overlap < 0 || per_overlap > 100)
            {
                throw new System.ArgumentException("`per_overlap` is percent overlap between " +
                    "segments to calculate Fourier Transform, needs to be between 0 and 100"); 
            }
            n_skip = (int)(Fs * nsec_fft * (100 - per_overlap) / 100);
            WelcomeMessage += String.Format("\t Overlap {0:0.0}% - hence skip {1} points\r\n", per_overlap, n_skip);

            // Taper window 
            WelcomeMessage += String.Format("\t The taper is the {0} windows functions\r\n", window_type.ToString());

            // STFT saving option 
            if (stft_saving_option)
            {
                string warning_stft_save = "WARNING: `stft_saving_option` = TRUE is not recommended" +
                    " for online plotting, due to time interference with continued data saving steps. " +
                    "This was only meant for checking the accuracy of FT at early steps.";
                Console.WriteLine(warning_stft_save);
                WelcomeMessage += "\t " + warning_stft_save + "\r\n"; 
            }

            // Parameters involing the band power calculations 
            WelcomeMessage += String.Format("\t The bandpower is calculated between {0} - {1} Hz, with{2} scaling\r\n", 
                    f_bandpower_lower, f_bandpower_upper, scaling_psd ? "" : "out" );

            // LBP alarm thresholds and plot options 
            lbp_lvl_reset_point = (int)(Fs * lbp_lvl_reset_sec);
            lbp_lvl_max_point = (int)(lbp_lvl_max_sec / lbp_lvl_reset_sec);
            WelcomeMessage += String.Format("\t Warning ({2} < x < {0}, {3} > x > {1}) \r\n\t Danger (x < {2}, x > {3})\r\n",
                    warning_lbp_lowerbound, warning_lbp_upperbound, danger_lbp_lowerbound, danger_lbp_upperbound);
            WelcomeMessage += String.Format("\t The alarm rate plot is updated every {0} seconds (non-overlapping)," +
                    " with maximum plotting limit of {1} minutes\r\n", lbp_lvl_reset_sec, lbp_lvl_max_sec / 60.0);
        }

        #endregion Initialization after prompt results 

        #region Conversion between `Color` and `SolidColorBrush` static helper functions
        /* BrushToColor: return a `System.Drawing.Color` from `System.Windows.Media.SolidColorBrush` input 
         */
        public static System.Drawing.Color BrushToColor(System.Windows.Media.SolidColorBrush br)
        {
            return Color.FromArgb(br.Color.A, br.Color.R, br.Color.G, br.Color.B);
        }

        /* ColorToBrush: return a `System.Windows.Media.SolidColorBrush` from `System.Drawing.Color` input  
         */
        public static System.Windows.Media.SolidColorBrush ColorToBrush(System.Drawing.Color color)
        {
            return new System.Windows.Media.SolidColorBrush
            {
                Color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)
            };
        }
        #endregion Conversion between `Color` and `SolidColorBrush` helper functions

        #region Specific Set methods during running application 
        /* The functions below are similar, either to update the vertical 
         * display gains or display offset for either `channel_plots` or `rms_plots`.
         * These functions are used in the `MainForm.XY_Z_Click` functions, please 
         * also refer to them for demonstration of use 
         * + LAYOUT:
         *      - Update the gain(or offset) with the appropriate step size and direction of update
         *      - Then assign the appropriate allowed minimum if that value is smaller
         * + INPUT: 
         *      - direction:            scale direction to update: 
         *                              * +1:   increase the gain(or offset) 
         *                              * -1:   decrease the gain(or offset) 
         *                              or by scalar of the update step
         */ 
        public void Control_Channel_Verical_Gain(double direction)
        {
            for (int i = 0; i < display_channel_vertgains.Length; i++)
            {
                display_channel_vertgains[i] += d_gain * direction;
                display_channel_vertgains[i] = Math.Max(display_channel_vertgains[i], min_gain);
            }
        }
        public void Control_Channel_Vertical_Offset(double direction)
        {
            display_channel_vertoffset += d_offset * direction;
            display_channel_vertoffset = Math.Max(display_channel_vertoffset, min_offset);
        }

        public void Control_RMS_Vertical_Gain(double direction)
        {
            for (int i = 0; i < display_rms_vertgains.Length; i++)
            {
                display_rms_vertgains[i] += d_gain * direction;
                display_rms_vertgains[i] = Math.Max(display_rms_vertgains[i], min_gain); 
            }
        }
        public void Control_RMS_Vertical_Offset(double direction)
        {
            display_rms_vertoffset += d_offset * direction;
            display_rms_vertoffset = Math.Max(display_rms_vertoffset, min_offset);
        }
        #endregion Specific Set methods during running application 

        #region Get and Set methods via reflection 
        /* + GetPropValue:  get the property value from the string of corresponding name 
         *   [source]:      https://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp
         * + GetPropType:   get the property type from the string of corresponding name
         * + SetPropValue:  set the property from the string of the corresponding name and the new value 
         *   [source]:      https://stackoverflow.com/questions/1089123/setting-a-property-by-reflection-with-a-string-value
         */
        public object GetPropValue(string propName)
        {
            return GetType().GetRuntimeProperty(propName)?.GetValue(this);
        }
        public Type GetPropType(string propName)
        {
            return GetType().GetProperty(propName)?.PropertyType;
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
        #endregion Get and Set methods via reflection 
    }
}
