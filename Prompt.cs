using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{

    /** Dialog promp implementation from Gideon Mulder in stakcoverflow
     * Source: https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
     */

    public class Prompt : IDisposable
    {
        private Form prompt { get; set; }
        public string Result { get; }
        public AppInputParameters input_params;

        public Prompt(string text, string caption)
        {
            input_params = new AppInputParameters();
            Result = ShowDialog(text, caption);
        }
        private string ShowDialog(string text, string caption)
        {
            prompt = new Form()
            {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            Label promptTitle = new Label() { Left = 50, Top = 20, Text = text, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };

            /*
            hostname = "127.0.0.1";
            port = 1234;

            Fs = 512.0;
            nmax_queue_total = 64;
            nsamp_per_block = 4;
            chan_idx2plt = 3; */

            int number_inputs = input_params.nameAndProp.Count;
            TextBox[] textBoxes = new TextBox[number_inputs];
            Label[] inputLabels = new Label[number_inputs];
            string name_i, prop_i;
            int top_label = 20, left_label = 50;
            int top_box = 20, left_box = 100, width_box = 100; 

            for (int i_inp = 0; i_inp < number_inputs; i_inp++)
            {
                name_i = input_params.nameAndProp.Keys.ElementAt(i_inp);
                Console.WriteLine("Name = " + name_i + "\t Prop = " + input_params.nameAndProp[name_i]); 
                prop_i = (string)input_params.GetPropValue(input_params.nameAndProp[name_i].ToString());

                inputLabels[i_inp] = new Label()
                {
                    Left = left_label,
                    Top = top_label,
                    Text = name_i,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                top_label += 20;
                textBoxes[i_inp] = new TextBox()
                {
                    Left = left_box,
                    Top = top_box,
                    Width = width_box, 
                    Text = prop_i
                };
                top_box += 20;
                prompt.Controls.Add(inputLabels[i_inp]);
                prompt.Controls.Add(textBoxes[i_inp]);
            }
            //TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 50, Text = (string)input_params.GetPropValue("hostname")};
             
            Button confirmation = new Button() { Text = "Ok", Left = 100, Width = 100, Top = 70, DialogResult = DialogResult.OK };            
            confirmation.Click += (sender, e) => { prompt.Close(); };

            //prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(promptTitle);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBoxes[0].Text : "";
        }

        public void Dispose()
        {
            prompt.Dispose();
        }
    }
}
