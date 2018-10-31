using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.IO;

namespace WindowsFormsApp4
{
    public class STFTCalculator
    {

        public double[] array, freq_vec, mag_freq; 
        public int nfft, nskip;
        public double Rate, fft_maxf, fft_fspacing;
        public int current_count;
        public bool ready2plt;
        public string savedfile = @"C:\Users\Towle\Desktop\Tuan\general_towle\data\mystft.csv";
        public string delim = ";";
        public int col_count; 
        public STFTCalculator(double Fs)
        {
            Rate = Fs;
            nfft = (int)(Fs * 2);
            nskip = (int)(Fs * 0.2);

            fft_maxf = Fs / 2;
            fft_fspacing = Fs / nfft;
            current_count = 0;
            col_count = 0; 
            ready2plt = false;

            array = new double[nfft];

            GenerateFrequencyVector();
            File.WriteAllText(savedfile, "Freq\n");
            File.AppendAllLines(savedfile,
                freq_vec.Select(d => d.ToString()));       

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
            freq_vec = new double[(int)(nfft / 2)];            
            for (int i = 0; i < freq_vec.Length; i++)
            {
                freq_vec[i] = i * fft_fspacing;
            }
        }
        public void AddData(double d)
        {
            array[current_count] = d;
            current_count++;
        }
        public void ShiftArray()
        {
            Array.Copy(array, nskip, array, 0, nfft - nskip);
            current_count = nfft - nskip;
        }
        public void CalculateFFT(double t, double d)
        {
            if (current_count == nfft)
            {
                mag_freq = new double[freq_vec.Length];
                mag_freq = FFT(array);
                ready2plt = true;
                col_count++;
                WriteToFile(string.Format("t = {0:0.00}", t), mag_freq);
                ShiftArray();
            } else if (current_count < nfft)
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
            double[] fft = new double[(int)(data.Length/2)];
            System.Numerics.Complex[] fftComplex = new System.Numerics.Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                fftComplex[i] = new System.Numerics.Complex(data[i], 0.0);
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < fft.Length; i++)
                fft[i] = fftComplex[i].Magnitude;
            return fft;
        }


    }
}
