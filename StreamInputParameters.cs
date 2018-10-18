using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class StreamInputParameters
    {
        public double danger_upperbound { get; set; }
        public double danger_lowerbound { get; set; }
        public double warning_upperbound { get; set; }
        public double warning_lowerbound { get; set; }

        public static Color danger_color { get; set; }
        public static Color warning_color { get; set; }
        public static Color normal_color { get; set; }

        public string hostname { get; set; }
        public int port { get; set; }

        public double Fs { get; set; }
        public int nmax_queue_total { get; set; }
        public int nsamp_per_block { get; set; }
        public int chan_idx2plt { get; set; }

        public StreamInputParameters()
        {
            danger_upperbound = 1.4;
            danger_lowerbound = 0.6;
            warning_upperbound = 1.2;
            warning_lowerbound = 0.8;

            danger_color = Color.FromArgb(255, 0, 0);
            warning_color = Color.FromArgb(255, 255, 0);
            normal_color = Color.FromArgb(0, 255, 0);

            hostname = "127.0.0.1";
            port = 1234;

            Fs = 512.0;
            nmax_queue_total = 64;
            nsamp_per_block = 4;
            chan_idx2plt = 3;
        }
    }
}
