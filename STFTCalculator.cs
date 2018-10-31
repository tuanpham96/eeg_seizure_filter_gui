﻿using System;
using System.Linq;
using System.IO;

namespace WindowsFormsApp4
{
    public class STFTCalculator
    {

        public double[] array, freq_vec, mag_freq; 
        public int n_epoch, n_skip, n_valid, n_display;
        public double Rate, fft_maxf, fft_fspacing;
        public int current_count;
        public bool ready2plt;
        public string savedfile, delim = ";";
        public bool saving_option;
        public double minf_display, maxf_display;
        public int lower_idx, upper_idx; 
        public STFTCalculator(double Fs, string file_prefix, bool saving_option, double minf_display, double maxf_display)
        {
            Rate = Fs;
            n_epoch = (int)(Fs * 2);
            n_skip = (int)(Fs * 0.2);
            n_valid = (int)(n_epoch / 2);
            n_display = n_valid; 

            fft_maxf = Fs / 2;
            fft_fspacing = Fs / n_epoch;
            current_count = 0;
            ready2plt = false;

            array = new double[n_epoch];

            this.minf_display = minf_display;
            this.maxf_display = maxf_display; 

            if (!(minf_display <= 0 && maxf_display >= fft_fspacing * (n_valid - 1))) {
                lower_idx = (int)(minf_display / fft_fspacing);
                upper_idx = (int)(maxf_display / fft_fspacing);
                n_display = upper_idx - lower_idx + 1; 
            }
            GenerateFrequencyVector();

            this.saving_option = saving_option;
            if (saving_option)
            {
                savedfile = file_prefix.Replace(".csv", "_stft.csv");
                File.WriteAllText(savedfile, "Freq\n");
                File.AppendAllLines(savedfile,
                    freq_vec.Select(d => d.ToString()));      
            }


        }
        
        public void WriteToFile(string s, double[] d)
        {
            var next_col = File.ReadLines(savedfile)
                .Select((line, index) => index == 0
                ? line + delim + s
                : line + delim + d[index - 1].ToString())
                .ToList();
            File.WriteAllLines(savedfile, next_col);
        }
        
        public void GenerateFrequencyVector()
        {
            freq_vec = new double[n_display];
            for (int i = 0; i < freq_vec.Length; i++)
            {
                freq_vec[i] = minf_display + i * fft_fspacing; 
            }
        }
        public void AddData(double d)
        {
            array[current_count] = d;
            current_count++;
        }
        public void ShiftArray()
        {
            Array.Copy(array, n_skip, array, 0, n_epoch - n_skip);
            current_count = n_epoch - n_skip;
        }
        public void CalculateFFT(double t, double d)
        {
            if (current_count == n_epoch)
            {
                mag_freq = new double[n_valid];
                mag_freq = FFT(array);
                ready2plt = true;
                if (saving_option) { WriteToFile(string.Format("t = {0:0.00}", t), mag_freq); }
                ShiftArray();
            } else if (current_count < n_epoch)
            {
                AddData(d);
                ready2plt = false; 
            } else
            {
                throw new System.Exception("Attribute `current_count` is greater than the allowed size of of the array"); 
            }
        }

        /*  Adapted from: 
         *  https://github.com/swharden/Csharp-Data-Visualization/blob/master/projects/18-09-19_microphone_FFT_revisited/ScottPlotMicrophoneFFT/ScottPlotMicrophoneFFT/Form1.cs
         */   
        public double[] FFT(double[] data)
        {
            double[] fft = new double[n_display];
            System.Numerics.Complex[] fftComplex = new System.Numerics.Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                fftComplex[i] = new System.Numerics.Complex(data[i], 0.0);
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < fft.Length; i++)
                fft[i] = fftComplex[i+lower_idx].Magnitude;
            return fft;
        }


    }
}
