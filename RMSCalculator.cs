﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
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
            this.data_queue = new Queue<double>();
            this.current_mean_sq = 0;
            this.current_rms = -1;
            this.nmax_queue_total = nmax_queue_total;

            this.n_lvls = n_lvls;
            Reset_Level_Tally();
        }

        public bool ParseCurrentValue(string s)
        {
            bool parse_success = double.TryParse(s, out this.current_val);
            return parse_success;
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
            this.rms_levels = new int[n_lvls];
            for (int i = 0; i < n_lvls; i++) { rms_levels[i] = 0; }
        }
    } 
}
