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
        public AppInputParameters Result { get; }
        public AppInputParameters input_params;

        public Prompt(string text, string caption)
        {
            input_params = new AppInputParameters();
            Result = ShowDialog(text, caption);
        }
        private AppInputParameters ShowDialog(string title, string caption)
        {
            prompt = new Form()
            {
                Width = 600,
                Height = 700,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };

            Label promptTitle = new Label() {
                Left = 50,
                Top = 10,
                Text = title,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter
            };
            prompt.Controls.Add(promptTitle);
            
            int number_inputs = input_params.nameAndProp.Count;
            TextBox[] textBoxes = new TextBox[number_inputs];
            Label[] inputLabels = new Label[number_inputs];
            string prop_alias, prop_name, prop_val;

            int top_label = 30, left_label = 10, width_label = 200;
            int top_box = 30, left_box = 220, width_box = 300;
            int spacing = 30; 

            for (int i_inp = 0; i_inp < number_inputs; i_inp++)
            {
                prop_alias = input_params.nameAndProp.Keys.ElementAt(i_inp);
                prop_name = input_params.nameAndProp[prop_alias].ToString();
                prop_val = input_params.GetPropValue(prop_name).ToString();

                inputLabels[i_inp] = new Label()
                {
                    Left = left_label,
                    Top = top_label,
                    Text = prop_alias,
                    Width = width_label, 
                    TextAlign = ContentAlignment.TopRight
                };
                top_label += spacing;
                textBoxes[i_inp] = new TextBox()
                {
                    Left = left_box,
                    Top = top_box,
                    Width = width_box, 
                    Text = prop_val
                };
                top_box += spacing;
                prompt.Controls.Add(inputLabels[i_inp]);
                prompt.Controls.Add(textBoxes[i_inp]);
            }
             

            Button confirmation = new Button() {
                Text = "Ok",
                Left = 100, Width = 
                100, Top = 500,
                DialogResult = DialogResult.OK
            };            
            confirmation.Click += (sender, e) => { prompt.Close(); };            
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                for (int i_inp = 0; i_inp < number_inputs; i_inp++)
                {
                    prop_alias = input_params.nameAndProp.Keys.ElementAt(i_inp);
                    prop_name = input_params.nameAndProp[prop_alias].ToString();                    
                    input_params.SetPropValue(prop_name, textBoxes[i_inp].Text); 
                }
            }
                return input_params; 
            // return prompt.ShowDialog() == DialogResult.OK ? input_params : input_params;
        }

        public void Dispose()
        {
            prompt.Dispose();
        }
    }
}
