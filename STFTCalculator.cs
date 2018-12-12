using System;
using System.Linq;
using System.IO;
using System.Reflection;

namespace seizure_filter
{
    /* STFTCalculator: Short-time Fourier transform and periodically keep tally of spectral alarm levels
     * 
     * Calculate both the STFT, then the total power of a certain frequency (limited-band power or LBP) 
     * The alarm is from the LBP alarm levels      
     * 
     * + Recommendations:
     *      - Write a (better) method to write the STFT, LBP data to a file separately and then
     *      save as "data_stft.csv" and "data_lbp_and_level.csv" and "data_lbp_alarmrate.csv" 
     */
    public class STFTCalculator
    {
        #region STFTCalculator attributes
        /* + n_epoch:               number of data points to calculate FFT
         * + n_skip:                number of data points skipped between different FFT calculated epochs
         * + n_valid:               valid number of frequency points (half of `n_epoch`) 
         * + Rate:                  sampling rate (Hz)
         * + fft_fspacing:          spacing between frequency points
         * + bandpower_freqrange:   2-element array containing the frequency band to calculate total power 
         * + data_array:            array to keep the data to calculate FFT 
         * + window:                array containing the taper window to apply before FFT 
         * + psd_scale:             scale of power spectral density 
         * + freq_vec:              frequency vector 
         * + mag_freq:              corresponding magnitude of each frequency in `freq_vec` after FFT calculation 
         * + psd:                   power spectral density 
         * + per_mag:               percentage of the magnitude over the whole FFT
         * + band_power:            total power from the `psd` in the frequency range of `bandpower_freqrange` (LBP)
         * + current_count:         current count of the data being read into `data_array` 
         * + ready2plt:             boolean to tell if the FFT is calculated for it to be ready to plot          
         * + n_lvls:                number of alarm levels 
         * + lbp_levels:            LBP alarm tally array, the length of which is `n_lvls` 
         * + savedfile:             location to save the STFT data to 
         * + delim:                 delimiter between the different columns 
         * + saving_option:         whether the STFT will be saved (unrecommended for performance for the
         *                          current implementation to save the data by horizontally concatenating
         *                          -> implement by vertical concatenation might be more effective) 
         */
        public int n_epoch, n_skip, n_valid;
        public double Rate, fft_fspacing;
        public double[] bandpower_freqrange;

        private double[] data_array, window;
        private double psd_scale; 
        public double[] freq_vec, mag_freq, psd, per_mag;
        public double band_power;

        public int current_count;
        public bool ready2plt;

        public int n_lvls;
        public int[] lbp_levels;

        private string savedfile, delim = ";";
        private bool saving_option;


        #endregion STFTCalculator attributes

        #region Window functions
        /* Taper functions */ 
        public enum WindowType {
            Hanning, Hamming, Rectangle, Triangle,
            Blackman, Exact_Blackman, FlatTop, Nuttall,
            Blackman_Nutall, Blackman_Harris
        };
        /* WindowFunctions: containing the different functions implemented for different taper functions
         * in order to apply to the data before FFT calculations
         */  
        private class WindowFunctions
        {
            /* Many of the equations are from 
             * https://en.wikipedia.org/wiki/Window_function &
             * http://zone.ni.com/reference/en-XX/help/371361E-01/lvanlsconcepts/char_smoothing_windows/#Exact_Blackman
             */
            private const double pi = Math.PI; 
            public double Rectangle(int n, int N)   { return 1; }
            public double Triangle(int n, int N)    { return 1 - Math.Abs( (n - (N-1)*0.5) / (N/2) ); }
            public double Hanning(int n, int N)     { return General_Cosine(n, N, 0.5); }
            public double Hamming(int n, int N)     { return General_Cosine(n, N, 25.0 / 46.0); }

            public double Blackman(int n, int N)
            {
                return Generalized_Cosine_Sum(n, N, 
                    new double[] {
                        0.42,
                        0.50,
                        0.08
                    });
            }
            public double Exact_Blackman(int n, int N)
            {
                return Generalized_Cosine_Sum(n, N,
                    new double[] {
                        7938.0 / 18608.0,
                        9240.0 / 18608.0,
                        1430.0 / 18608.0
                    });
            }
            public double FlatTop(int n, int N)
            {
                return Generalized_Cosine_Sum(n, N,
                    new double[] {
                        0.215578948,
                        0.416631580,
                        0.277263158,
                        0.083578947,
                        0.006947368
                    });
            }
            public double Nuttall(int n, int N)
            {
                return Generalized_Cosine_Sum(n, N,
                    new double[] {
                        0.355768, 
                        0.487396, 
                        0.144232, 
                        0.012604
                    });
            }
            public double Blackman_Nutall(int n, int N)
            {
                return Generalized_Cosine_Sum(n, N,
                    new double[] {
                        0.3635819,
                        0.4891775,
                        0.1365995,
                        0.0106411
                    });
            }
            public double Blackman_Harris(int n, int N)
            {
                return Generalized_Cosine_Sum(n, N,
                    new double[] {
                        0.35875,
                        0.48829,
                        0.14128,
                        0.01168
                    });
            }

            private double General_Cosine(int n, int N, double a0)
            {
                return a0 - (1 - a0) * Math.Cos(2 * pi * n / (N - 1));
            }
            private double Generalized_Cosine_Sum(int n, int N, double[] a)
            {
                double w_n = a[0]; 
                for (int k = 1; k < a.Length; k++)
                {
                    double sign = k % 2 == 0 ? 1.0 : -1.0;
                    w_n += sign * a[k] * Math.Cos(2 * pi * k * n / (N - 1)); 
                }
                return w_n;
            }

            public MethodInfo function(string method)
            {
                return GetType().GetMethod(method); 
            }
            
        }
        #endregion Window functions

        #region STFTCalculator constructor
        /* STFTCalculator initialization 
         * + LAYOUT: 
         *      - Initialize the first few parameters necessary to calculate FFT
         *      - Check for validity of frequency range for LBP calculation 
         *      - Generate frequency vector 
         *      - Generate taper window
         *      - Initialize LBP alarm level array
         *      - Configure saving options 
         * + INPUT:
         *      - Fs:               sampling frequency 
         *      - n_epoch:          number of data points to calculate FFT
         *      - n_skip:           number of data points skipped between different FFT calculated epochs
         *      - n_lvls:           number of alarm levels 
         *      - BPFR:             range of frequency band to calculate power as LBP
         *      - win_type:         type of taper to use; [default]`WindowType.Rectangle` means no taper
         *      - scaling_psd:      option to scale power spectral density, [default]"true"
         *      - file_prefix:      prefix of file to save STFT calculation to, [default]"" (empty) 
         *      - saving_option:    option to save the STFT calculation to, [default]"false" for performance 
         *                          with the current implementation to save the file to 
         */
        public STFTCalculator(double Fs, int n_epoch, int n_skip, int n_lvls, double[] BPFR, 
                            WindowType win_type = WindowType.Rectangle, bool scaling_psd = true, 
                            string file_prefix = "", bool saving_option = false)
        {
            Configure_STFT_Parameters(Fs, n_epoch, n_skip);

            current_count = 0;
            ready2plt = false;
            data_array = new double[n_epoch];

            Configure_Frequency_Range(BPFR);             
            GenerateFrequencyVector();
            GenerateWindow(win_type, scaling_psd);

            this.n_lvls = n_lvls;
            Reset_Level_Tally();

            this.saving_option = saving_option;
            Configure_Saving_Options(file_prefix);

        }
        #endregion STFTCalculator constructor

        #region Initialization and configuration for constructor 
        /* Configure_STFT_Parameters: configure `Rate`, `n_epoch`, `n_skip`, `n_valid`, `fft_fspacing` */  
        private void Configure_STFT_Parameters(double Fs, int _n_epoch_, int _n_skip_)
        {
            Rate = Fs;
            n_epoch = _n_epoch_;
            n_skip = _n_skip_;
            n_valid = (int)(n_epoch / 2);

            fft_fspacing = Fs / n_epoch;
        }
        /* Configure_Frequency_Range: check for valid input for `bandpower_freqrange` */ 
        private void Configure_Frequency_Range(double[] d)
        {
            bandpower_freqrange = new double[2];
            string err_mess = "`bandpower_freqrange` can only have 2 values of increasing order";
            if (d.Length == 2)
            {
                if (d[0] < d[1])
                {
                    bandpower_freqrange[0] = d[0];
                    bandpower_freqrange[1] = d[1];
                }
                else
                {
                    throw new System.ArgumentException(err_mess);
                }
            }
            else
            {
                throw new System.IndexOutOfRangeException(err_mess);
            }
        }
        /* GenerateWindow: generate the taper window, if `scaling_psd` = false then set `psd_scale` = 1 */ 
        private void GenerateWindow(WindowType win_type, bool scaling_psd)
        {
            string win_name = win_type.ToString();
            WindowFunctions wf = new WindowFunctions();
            MethodInfo method = wf.function(win_name);

            window = new double[n_epoch];
            psd_scale = 0; 
            for (int i = 0; i < n_epoch; i++)
            {
                window[i] = Convert.ToDouble(method.Invoke(wf, new object[] { i, n_epoch }));
                psd_scale += window[i] * window[i];
            }

            psd_scale = 1 / (psd_scale * Rate);

            if (!scaling_psd)
            {
                psd_scale = 1.0;
            }
        }
        /* GenerateFrequencyVector: generate the frequency vector to be used for plotting after FFT calculation */ 
        private void GenerateFrequencyVector()
        {
            freq_vec = new double[n_valid];
            for (int i = 0; i < freq_vec.Length; i++)
            {
                freq_vec[i] = i * fft_fspacing;
            }
        }
        /* Configure_Saving_Options: configure the file name after input of `file_prefix` and start the file */ 
        private void Configure_Saving_Options(string file_prefix)
        {
            if (saving_option)
            {
                savedfile = file_prefix.Replace(".csv", "_stft.csv");
                File.WriteAllText(savedfile, "Freq\n");
                File.AppendAllLines(savedfile,
                    freq_vec.Select(d => d.ToString()));
            }
        }
        #endregion Initialization and configuration for constructor 

        #region Manipulating data 
        /* NormalizeData: normalize a double array by (x - mean(x))/sttdev(x) */
        public double[] NormalizeData(double[] data)
        {
            int len_dat = data.Length;
            double mean = data.Average();
            double stddev = 0;
            for (int i = 0; i < len_dat; i++)
            {
                stddev += (data[i] - mean) * (data[i] - mean);
            }
            stddev = Math.Sqrt(stddev / len_dat);
            double[] normz_dat = new double[len_dat];
            for (int i = 0; i < len_dat; i++)
            {
                normz_dat[i] = (data[i] - mean) / stddev;
            }

            return normz_dat;
        }
        /* AddData: add new data `d` to the `data_array` */
        public void AddData(double d)
        {
            data_array[current_count] = d;
            current_count++;
        }
        /* ShiftArray: shift the `data_array` by `n_skip` at the end of all FFT calculation*/
        public void ShiftArray()
        {
            Array.Copy(data_array, n_skip, data_array, 0, n_epoch - n_skip);
            current_count = n_epoch - n_skip;
        }
        /* WriteToFile: write the data to file 
         * + Unrecommended as stressed throughout the application documentation due to the
         * implementation is by horizontal concatenation. Initially, I used this just 
         * for testing FFT calculation, which was fine since I only needed a few seconds of
         * Sinus_Oscillator data to test offline. This slowed down the performance significantly 
         * after that because this method somehow interefered with the process. 
         * + Recommendations: maybe implement vertical concatenation instead of horizontal 
         * concatenation (haven't tried but possibly not gonna interefere)?
         */  
        private void WriteToFile(string s, double[] d)
        {
            var next_col = File.ReadLines(savedfile)
                .Select((line, index) => index == 0
                ? line + delim + s
                : line + delim + d[index - 1].ToString())
                .ToList();
            File.WriteAllLines(savedfile, next_col);
        }
        #endregion Manipulating data 

        #region Spectral-related calculations
        /* CalculateFFT: actually this method is to a general call for both add data when the 
         * data array is not of specified length, then calculate if it is
         * + LAYOUT:
         *      - Add data if the data array does not have sufficient number of data points for FFT calculation
         *      - If it does, then NORMALIZE the data then perform FFT to obtain the magnitude of each frequency; 
         *                      also set `ready2plt` = true and save the data if `saving_option` = true;
         *                      then shift the data array before adding the new data point 
         *      - If neither, throw an error
         * + INPUT:
         *      - t:            the current time point 
         *      - d:            the current data to add to 
         */      
        public void CalculateFFT(double t, double d)
        {
            if (current_count == n_epoch)
            {
                mag_freq = new double[n_valid];
                double[] normz_dat_arr = NormalizeData(data_array);
                mag_freq = FFT(normz_dat_arr);
                ready2plt = true;

                if (saving_option) { WriteToFile(string.Format("t = {0:0.00}", t), mag_freq); }

                ShiftArray();
                AddData(d);
            }
            else if (current_count < n_epoch)
            {
                AddData(d);
                ready2plt = false;
            }
            else
            {
                throw new System.Exception("Attribute `current_count` is greater than the allowed size of of the array");
            }
        }
        /* CalculatePSD: calculate the power spectral density and the limited band power */  
        public void CalculatePSD()
        {
            psd = new double[n_valid];
            band_power = 0;
            for (int i = 0; i < n_valid; i++)
            {
                psd[i] = psd_scale * mag_freq[i] * mag_freq[i];
                if (freq_vec[i] >= bandpower_freqrange[0] && freq_vec[i] <= bandpower_freqrange[1])
                {
                    band_power += psd[i]; 
                }
            }
        }
        /* CalculatePercentMagnitude: calculate the percentage of each magnitude relative to the whole spectrum */ 
        public void CalculatePercentMagnitude()
        {
            per_mag = new double[n_valid];
            double sum = 0; 
            for (int i = 0; i < n_valid; i++)
            {
                sum += mag_freq[i]; 
            }
            for (int i = 0; i < n_valid; i++)
            {
                per_mag[i] = mag_freq[i]/sum;
            }
        }
        /* FFT: actual FTT calculation of after taperring double array `data` with the taper window application      
         * Return the FFT of the windowed data 
         * Adapted from: 
         *  https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/18-09-19_microphone_FFT_revisited/ScottPlotMicrophoneFFT/ScottPlotMicrophoneFFT/Form1.cs
         */
        public double[] FFT(double[] data)
        {
            double[] fft = new double[(int)(data.Length / 2)];
            System.Numerics.Complex[] fftComplex = new System.Numerics.Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                fftComplex[i] = new System.Numerics.Complex(data[i] * window[i], 0.0); // multipled by the window functions 
            }
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < fft.Length; i++)
            {
                fft[i] = fftComplex[i].Magnitude;
            }
            return fft;
        }

        #endregion Spectral-related calculations

        #region LBP alarm tally for LBP alarm rate
        /* Reset_Level_Tally: initialize the tally array or reset for the next tally round */
        public void Reset_Level_Tally()
        {
            this.lbp_levels = new int[n_lvls];
            for (int i = 0; i < n_lvls; i++) { lbp_levels[i] = 0; }
        }
        /* Tally_Levels: tally the alarm level `lvl` to the tally array */
        public void Tally_Levels(int lvl)
        {
            if (lvl < n_lvls) { lbp_levels[lvl]++; }
            else
            {
                throw new System.ArgumentOutOfRangeException(string.Format("`lvl` = {0} is not a valid level, " +
                 "needs to be < `n_lvl` = {1}", lvl, n_lvls));
            }
        }
        /* Cumulative_From_Lower_Level: this is for the option of stacked series in alarm rate plot
         * meaning WARNING <- WARNING + NORMAL; DANGER <- DANGER + WARNING + NORMAL
         */  
        public void Cumulative_From_Lower_Level()
        {
            for (int i = 0; i < (n_lvls - 1); i++)
            {
                lbp_levels[i + 1] += lbp_levels[i];
            }
        }

        #endregion LBP alarm tally for LBP alarm rate

    }
}
