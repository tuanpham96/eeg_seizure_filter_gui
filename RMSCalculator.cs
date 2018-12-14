using System;
using System.Collections.Generic;


namespace seizure_filter
{
    /* RMSCalculator: Root mean squared calculator of streaming data and tally of alarm levels 
     * 
     * Particularly the RMS calculation is done like a "sliding window" calculation, not of the
     * entire data (because this is streaming data). And the object also keeps tally of alarm levels 
     *
     * + Recommendations:
     *      - Write a method to write the RMS data to a file separately from the channel data and 
     *      save as "data_rms_and_level.csv" and "data_rms_alarmrate.csv" instead of the current method 
     *      of saving the data in `MainForm.DrawAndReport` 
     */  
    public class RMSCalculator
    {
        #region RMSCalculator attributes
        /* + nmax_queue_total:  max number of queue size, also the size of the window to 
         *                      calculate RMS of the data 
         * + data_queue:        private queue of incoming data to calculate "sliding" RMS, 
         *                      particularly would be of size `nmax_queue_total` after some point 
         * + current_val:       current data value (meaning the newly parsed data usually)
         * + current_mean_sq:   current mean squared of the window `nmax_queue_total` 
         * + current_rms:       current RMS of the window `nmax_queue_total`
         * + n_lvls:            number of alarm levels 
         * + rms_levels:        RMS alarm tally array, the length of which is `n_lvls` 
         */
        public int nmax_queue_total;
        private Queue<double> data_queue;
        public double current_val;
        public double current_mean_sq, current_rms;

        public int n_lvls;
        public int[] rms_levels;

        #endregion RMSCalculator attributes

        #region RMSCalculator constructor
        /* RMSCalculator initilization 
         * + LAYOUT: 
         *      - Intialize attributes for queue, calculations and alarm tally 
         * + INPUT:
         *      - nmax_queue_total:     max number of queue size, which is also the 
         *                              window size to calculate the "sliding RMS" 
         *      - n_lvls:               number of alarm levels, currently set as 3 throughout
         *                              the application for simplicity 
         */
        public RMSCalculator(int nmax_queue_total, int n_lvls)
        {
            data_queue = new Queue<double>();
            current_mean_sq = 0;
            current_rms = -1;
            this.nmax_queue_total = nmax_queue_total;

            this.n_lvls = n_lvls;
            Reset_Level_Tally();
        }
        #endregion RMSCalculator constructor

        #region RMS calculations 
        /* ParseCurrentValue: parse a string to the current value 
         * + INPUT:
         *      - s:    a string containing the current data value 
         * + OUTPUT:
         *      - whether the parse from string to double is successful 
         */  
        public bool ParseCurrentValue(string s)
        {
            bool parse_success = double.TryParse(s, out current_val);
            return parse_success;
        }

        /* CalculateRMS: calculate the current RMS value 
         * + LAYOUT: 
         *      - Queue the new value 
         *      - If the number of data has not reached the desired 
         *          window to calculate RMS, accumulate the "squares" 
         *      - If it has, calculate the actual RMS value 
         *      - If it has surpassed, pop out the oldest value and 
         *          update the current RMS value (see further documentation
         *          for details if needed explanation of how that works)
         *      - Briefly: if we define current RMS as R(i) and RMS window size N
         *              R(i)^2      = { x[i+1]^2 + x[i+2]^2 + ... +  x[N+i-1]^2 + x[N+i]^2 } / N
         *          The next RMS value would be R(i+1):
         *              R(i+1)^2    = { x[i+2]^2 + x[i+3]^2 + ... +  x[N+i]^2 + x[N+i+1]^2 } / N
         *                          = { x[i+1]^2 + x[i+2]^2 + ... + x[N+i]^2 } / N + { -x[i+1]^2 + x[N+i+1]^2 } / N
         *                          = R(i)^2    -   (x[i+1]^2 / N)    +     (x[N+i+1]^2 / N) 
         *          Hence new value is: sqrt( RMS_prev^2 - oldest^2/N + newest^2/N )
         * + INPUT: 
         *      - count:    the current count of the data that have been streamed
         */
        public void CalculateRMS(int count)
        {
            double oldest_sq, newest_sq;
            data_queue.Enqueue(current_val);
            if (count < nmax_queue_total - 1)
            {
                current_mean_sq += current_val * current_val;
            }
            else if (count == nmax_queue_total - 1)
            {
                current_rms = Math.Sqrt(current_mean_sq / nmax_queue_total);
            }
            else
            {
                oldest_sq = data_queue.Dequeue();
                oldest_sq = oldest_sq * oldest_sq / nmax_queue_total;

                newest_sq = current_val * current_val / nmax_queue_total;
                current_mean_sq = current_rms * current_rms;

                current_rms = Math.Sqrt(current_mean_sq - oldest_sq + newest_sq);
            }
        }
        #endregion RMS calculations 

        #region RMS alarm tally for RMS alarm rate
        /* Reset_Level_Tally: initialize the tally array or reset for the next tally round */  
        public void Reset_Level_Tally()
        {
            rms_levels = new int[n_lvls];
            for (int i = 0; i < n_lvls; i++) { rms_levels[i] = 0; }
        }
        /* Tally_Levels: tally the alarm level `lvl` to the tally array */ 
        public void Tally_Levels(int lvl)
        {
            if (lvl < n_lvls) { rms_levels[lvl]++; }
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
                rms_levels[i + 1] += rms_levels[i];
            }
        }

        #endregion RMS alarm tally for RMS alarm rate
    }
}
