using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace WindowsFormsApp4
{
    public class AppInputParameters
    {
        public string hostname { get; set; }
        public int port { get; set; }

        public double Fs { get; set; }
        public int nmax_queue_total { get; set; }
        public int nsamp_per_block { get; set; }
        public int chan_idx2plt { get; set; }

        public string output_file_name { get; set; }

        public double danger_upperbound { get; set; }
        public double danger_lowerbound { get; set; }
        public double warning_upperbound { get; set; }
        public double warning_lowerbound { get; set; }

        public Color danger_color { get; set; }
        public Color warning_color { get; set; }
        public Color normal_color { get; set; }

        public Dictionary<string, string> nameAndProp; 

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
            } catch (Exception e)
            {
                Console.WriteLine("Error in setting value: -> " + e.StackTrace); 
            }
        }

        public AppInputParameters()
        {
            nameAndProp = new Dictionary<string, string>
            {
                {"Host name", "hostname"},
                {"Port", "port" },
                {"Sample frequency (Hz)", "Fs" },
                {"RMS window (#points)", "nmax_queue_total" },
                {"Number of samples/channel/epoch", "nsamp_per_block" },
                {"Channel ID# to display", "chan_idx2plt" },
                {"Output file name",  "output_file_name"},
                {"DANGER upper bound",  "danger_upperbound"},
                {"DANGER lower bound",  "danger_lowerbound"},
                {"WARNING upper bound", "warning_upperbound" },
                {"WARNING lower bound", "warning_lowerbound" }
            };


            hostname = "127.0.0.1";
            port = 1234;

            Fs = 512.0;
            nmax_queue_total = 64;
            nsamp_per_block = 4;
            chan_idx2plt = 3;

            output_file_name = @"C:\Users\Towle\Desktop\Tuan\data\testfile_TP.csv";

            danger_upperbound = 1.4;
            danger_lowerbound = 0.6;
            warning_upperbound = 1.2;
            warning_lowerbound = 0.8;

            danger_color = Color.FromArgb(255, 0, 0);
            warning_color = Color.FromArgb(255, 255, 0);
            normal_color = Color.FromArgb(0, 255, 0);
        }
    }
}
