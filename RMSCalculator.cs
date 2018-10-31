using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    class RMSCalculator
    {
        public Queue<double> data_queue;
        public double current_val;
        public double current_mean_sq, current_rms;
        public int nmax_queue_total;
        public RMSCalculator(int nmax_queue_total)
        {
            this.data_queue = new Queue<double>();
            this.current_mean_sq = 0;
            this.current_rms = -1;
            this.nmax_queue_total = nmax_queue_total;
        }
        public void ParseCurrentValue(string s)
        {
            double.TryParse(s, out this.current_val);
        }
        public void CalculateRMS(int count) {
            double oldest_sq, newest_sq;
            this.data_queue.Enqueue(this.current_val);
            if (count < this.nmax_queue_total - 1)
            {
                this.current_mean_sq += this.current_val * this.current_val; 
            } else if (count == this.nmax_queue_total - 1)
            {
                this.current_rms = Math.Sqrt(this.current_mean_sq / this.nmax_queue_total); 
            } else
            {
                oldest_sq = this.data_queue.Dequeue();
                oldest_sq = oldest_sq * oldest_sq / this.nmax_queue_total;

                newest_sq = current_val * current_val / this.nmax_queue_total;
                this.current_mean_sq = this.current_rms * this.current_rms;

                this.current_rms = Math.Sqrt(this.current_mean_sq - oldest_sq + newest_sq); 
            }
        } 
       
    }
}
