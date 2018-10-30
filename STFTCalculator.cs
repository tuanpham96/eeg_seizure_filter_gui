using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace WindowsFormsApp4
{
    public class STFTCalculator
    {

        public double[] array, freq_vec, mag_freq; 
        public int nfft, nskip;
        public double Rate, fft_maxf, fft_fspacing;
        public int current_count;
        public bool ready2plt; 
        public STFTCalculator(double Fs)
        {
            Rate = Fs;
            nfft = (int)(Fs * 2);
            nskip = (int)(Fs * 0.2);
            array = new double[nfft];
            fft_maxf = Fs / 2;
            fft_fspacing = Fs / nfft;
            current_count = 0;
            ready2plt = false; 
            GenerateFrequencyVector(); 
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
        public void CalculateFFT(double d)
        {
            if (current_count == nfft)
            {
                mag_freq = new double[freq_vec.Length];
                mag_freq = FFT(array);
                ready2plt = true; 
                ShiftArray();
            } else
            {
                AddData(d);
                ready2plt = false; 
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
