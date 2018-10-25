using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private string fontname { get; }
        public Prompt(string text, string caption)
        {
            fontname = "Arial"; 
            input_params = new AppInputParameters();
            Result = ShowDialog(text, caption);
        }
        private AppInputParameters ShowDialog(string title, string caption)
        {
            prompt = new Form()
            {
                Width = 670,
                Height = 700,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };

            Label promptTitle = new Label() {
                Left = 10,
                Top = 20,
                Width = 600,
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font(fontname, 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            };
            prompt.Controls.Add(promptTitle);
            
            int number_inputs = input_params.nameAndProp.Count;
            TextBox[] textBoxes = new TextBox[number_inputs];
            Label[] inputLabels = new Label[number_inputs];
            string prop_alias, prop_name, prop_val;

            int top_label = 70, left_label = 10, width_label = 200;
            int top_box = 70, left_box = 220, width_box = 420, mod_width_box;
            int spacing = 32;

            int idx_output_folder = -1, idx_output_file_name = -1; 

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
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new System.Drawing.Font(fontname, 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
                };
                top_label += spacing;
                

                if (string.Compare(prop_name, "output_folder") == 0) 
                {
                    mod_width_box = width_box - 150;
                    idx_output_folder = i_inp;
                } else if (string.Compare(prop_name, "output_file_name") == 0)
                {
                    mod_width_box = width_box - 150;
                    idx_output_file_name = i_inp;
                } else 
                {
                    mod_width_box = width_box;
                }
                textBoxes[i_inp] = new TextBox()
                {
                    Left = left_box,
                    Top = top_box,
                    Width = mod_width_box, 
                    Text = prop_val,
                    Font = new System.Drawing.Font(fontname, 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
                };
                top_box += spacing;
                prompt.Controls.Add(inputLabels[i_inp]);
                prompt.Controls.Add(textBoxes[i_inp]);
            }

            Button openfolderdialog = new Button()
            {
                Text = "Choose folder and \n file name",
                Height = 58,
                Left = textBoxes[idx_output_folder].Left + textBoxes[idx_output_folder].Width + 10,
                Width = 140,
                Top = textBoxes[idx_output_folder].Top,
                Font = new System.Drawing.Font(fontname, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            };
            openfolderdialog.Click += (sender, e) => {
                // https://stackoverflow.com/questions/11624298/how-to-use-openfiledialog-to-select-a-folder by Daniel Ballinger 
                OpenFileDialog folderBrowser = new OpenFileDialog();
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckFileExists = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.InitialDirectory = input_params.output_folder;
                folderBrowser.FileName = "Choose folder then enter desired file name here";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(folderBrowser.FileName); 
                    string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                    textBoxes[idx_output_folder].Text = folderPath;
                    textBoxes[idx_output_file_name].Text = fileName; 
                }
            };
            prompt.Controls.Add(openfolderdialog);

            Button confirmation = new Button() {
                Text = "Continue",
                Left = 520,
                Width = 120,
                Height = 40, 
                Top = 600,
                DialogResult = DialogResult.OK,
                Font = new System.Drawing.Font(fontname, 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
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
