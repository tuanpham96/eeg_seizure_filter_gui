using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seizure_filter
{
    public class RMSCalculator
    {
        public Queue<double> data_queue;
        public double current_val;
        public double current_mean_sq, current_rms;
        public int[] rms_levels;
        public int nmax_queue_total;
        public int n_lvls; 
        public RMSCalculator(int nmax_queue_total, int n_lvls)
        {
            data_queue = new Queue<double>();
            current_mean_sq = 0;
            current_rms = -1;
            this.nmax_queue_total = nmax_queue_total;

            this.n_lvls = n_lvls;
            Reset_Level_Tally();
        }

        public bool ParseCurrentValue(string s)
        {
            bool parse_success = double.TryParse(s, out current_val);
            return parse_success;
        }

        public void CalculateRMS(int count) {
            double oldest_sq, newest_sq;
            data_queue.Enqueue(current_val);
            if (count < nmax_queue_total - 1)
            {
                current_mean_sq += current_val * current_val; 
            } else if (count == nmax_queue_total - 1)
            {
                current_rms = Math.Sqrt(current_mean_sq / nmax_queue_total); 
            } else
            {
                oldest_sq = data_queue.Dequeue();
                oldest_sq = oldest_sq * oldest_sq / nmax_queue_total;

                newest_sq = current_val * current_val / nmax_queue_total;
                current_mean_sq = current_rms * current_rms;

                current_rms = Math.Sqrt(current_mean_sq - oldest_sq + newest_sq); 
            }
        }

        public void Tally_Levels(int lvl)
        {
            if (lvl < n_lvls) { rms_levels[lvl]++; }
            else { throw new System.ArgumentOutOfRangeException(string.Format("`lvl` = {0} is not a valid level, " +
                    "needs to be < `n_lvl` = {1}", lvl, n_lvls));
            }        }

        public void Cumulative_From_Lower_Level()
        {
            for (int i = 0; i < (n_lvls - 1); i++)
            {
                rms_levels[i + 1] += rms_levels[i];
            }
        }
        public void Reset_Level_Tally()
        {
            rms_levels = new int[n_lvls];
            for (int i = 0; i < n_lvls; i++) { rms_levels[i] = 0; }
        }
    } 
}
