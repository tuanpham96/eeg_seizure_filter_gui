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
                Width = 800,
                Height = 1000,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };

            Label promptTitle = new Label()
            {
                Left = 100,
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

            Panel rdbt_refresh_group, rdbt_quality_group, rdbt_wintype_group, rdbt_stftsave_group; 
            RadioButton[] rdbt_refresh = new RadioButton[input_params.display_refresh_options.Count];
            string rdbt_refresh_choice = "";

            RadioButton[] rdbt_quality = new RadioButton[input_params.gear_quality_dict.Count];
            string rdbt_quality_choice = "";

            RadioButton[] rdbt_wintype = new RadioButton[input_params.wintype_dict.Count];
            string rdbt_wintype_choice = "";

            RadioButton[] rdbt_stftsave = new RadioButton[input_params.stft_saving_options.Count];
            string rdbt_stftsave_choice = "";

            int top_label = 70, left_label = 10, width_label = 300;
            int top_box = 70, left_box = 320, width_box = 420, mod_width_box;
            int spacing = 32;

            void radiobutton_change(object sender, EventArgs e, ref string rdbt_choice)
            {
                RadioButton rb = sender as RadioButton;
                if (rb == null) { MessageBox.Show("Sender is not a RadioButton"); return; }
                if (rb.Checked) { rdbt_choice = rb.Text; }
            }

            Dictionary<string, int> name_idx_dict = new Dictionary<string, int>();
            string prop_alias, prop_name, prop_val;
            for (int i_inp = 0; i_inp < number_inputs; i_inp++)
            {
                prop_alias = input_params.nameAndProp.Keys.ElementAt(i_inp);
                prop_name = input_params.nameAndProp[prop_alias].ToString();
                prop_val = input_params.GetPropValue(prop_name).ToString();

                name_idx_dict.Add(prop_name, i_inp);

                inputLabels[i_inp] = new Label()
                {
                    Left = left_label,
                    Top = top_label,
                    Text = prop_alias,
                    Width = width_label,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new System.Drawing.Font(fontname, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
                };
                prompt.Controls.Add(inputLabels[i_inp]);
                top_label += spacing;

                if (string.Compare(prop_name, "output_folder") == 0 || string.Compare(prop_name, "output_file_name") == 0)
                {
                    mod_width_box = width_box - 150;
                }
                else
                {
                    mod_width_box = width_box;
                }

                if (string.Compare(prop_name, "refresh_display") == 0)
                {
                    rdbt_refresh_group = new Panel()
                    {
                        Location = new System.Drawing.Point(left_box, top_box - 5),
                        Size = new System.Drawing.Size(mod_width_box, spacing + 5),
                        BorderStyle = BorderStyle.None
                    };

                    int default_idx = -1;
                    for (int i_rdbtr = 0; i_rdbtr < input_params.display_refresh_options.Count; i_rdbtr++)
                    {
                        var i_key = input_params.display_refresh_options.Keys.ElementAt(i_rdbtr);
                        rdbt_refresh[i_rdbtr] = new RadioButton()
                        {
                            Location = new System.Drawing.Point(20 + 100 * i_rdbtr, 5),
                            Size = new System.Drawing.Size(100, spacing), 
                            Text = i_key
                        };
                        rdbt_refresh[i_rdbtr].CheckedChanged += new EventHandler((sender, e) => { radiobutton_change(sender, e, ref rdbt_refresh_choice); });

                        rdbt_refresh_group.Controls.Add(rdbt_refresh[i_rdbtr]);

                        if (string.Compare(prop_val.ToString(), input_params.display_refresh_options[i_key].ToString()) == 0)
                        {
                            default_idx = i_rdbtr;
                        }
                    }
                    rdbt_refresh[default_idx].Checked = true;
                    prompt.Controls.Add(rdbt_refresh_group);
                    top_box += spacing;
                    continue;
                }

                if (string.Compare(prop_name, "display_quality") == 0)
                {
                    rdbt_quality_group = new Panel()
                    {
                        Location = new System.Drawing.Point(left_box, top_box - 5),
                        Size = new System.Drawing.Size(mod_width_box, spacing + 5),
                        BorderStyle = BorderStyle.None
                    };

                    int default_idx = -1;
                    for (int i_rdbtr = 0; i_rdbtr < input_params.gear_quality_dict.Count; i_rdbtr++)
                    {
                        var i_key = input_params.gear_quality_dict.Keys.ElementAt(i_rdbtr);
                        rdbt_quality[i_rdbtr] = new RadioButton()
                        {
                            Location = new System.Drawing.Point(20 + 100 * i_rdbtr, 5),
                            Size = new System.Drawing.Size(100, spacing),
                            Text = i_key
                        };
                        rdbt_quality[i_rdbtr].CheckedChanged += new EventHandler((sender, e) => { radiobutton_change(sender, e, ref rdbt_quality_choice); });

                        rdbt_quality_group.Controls.Add(rdbt_quality[i_rdbtr]);
                        
                        if (string.Compare(prop_val.ToString(), input_params.gear_quality_dict[i_key].ToString()) == 0)
                        {
                            default_idx = i_rdbtr;
                        } 
                    }
                    rdbt_quality[default_idx].Checked = true;
                    prompt.Controls.Add(rdbt_quality_group);
                    top_box += spacing;
                    continue;
                }

                if (string.Compare(prop_name, "window_type") == 0)
                {
                    rdbt_wintype_group = new Panel()
                    {
                        Location = new System.Drawing.Point(left_box, top_box - 5),
                        Size = new System.Drawing.Size(mod_width_box, spacing + 5),
                        BorderStyle = BorderStyle.None
                    };
                    int default_idx = -1;
                    for (int i_rdbtr = 0; i_rdbtr < input_params.wintype_dict.Count; i_rdbtr++)
                    {
                        var i_key = input_params.wintype_dict.Keys.ElementAt(i_rdbtr);
                        rdbt_wintype[i_rdbtr] = new RadioButton()
                        {
                            Location = new System.Drawing.Point(20 + 100 * i_rdbtr, 5),
                            Size = new System.Drawing.Size(100, spacing),
                            Text = i_key
                        };
                        rdbt_wintype[i_rdbtr].CheckedChanged += new EventHandler((sender, e) => { radiobutton_change(sender, e, ref rdbt_wintype_choice); });

                        rdbt_wintype_group.Controls.Add(rdbt_wintype[i_rdbtr]);

                        if (string.Compare(prop_val.ToString(), input_params.wintype_dict[i_key].ToString()) == 0)
                        {
                            default_idx = i_rdbtr;
                        }
                    }
                    rdbt_wintype[default_idx].Checked = true;
                    prompt.Controls.Add(rdbt_wintype_group);
                    top_box += spacing;
                    continue;
                }

                if (string.Compare(prop_name, "stft_saving_option") == 0)
                {
                    rdbt_stftsave_group = new Panel()
                    {
                        Location = new System.Drawing.Point(left_box, top_box - 5),
                        Size = new System.Drawing.Size(mod_width_box, spacing + 5),
                        BorderStyle = BorderStyle.None
                    };
                    int default_idx = -1;
                    for (int i_rdbtr = 0; i_rdbtr < input_params.stft_saving_options.Count; i_rdbtr++)
                    {
                        var i_key = input_params.stft_saving_options.Keys.ElementAt(i_rdbtr);
                        rdbt_stftsave[i_rdbtr] = new RadioButton()
                        {
                            Location = new System.Drawing.Point(20 + 100 * i_rdbtr, 5),
                            Size = new System.Drawing.Size(100, spacing),
                            Text = i_key
                        };
                        rdbt_stftsave[i_rdbtr].CheckedChanged += new EventHandler((sender, e) => { radiobutton_change(sender, e, ref rdbt_stftsave_choice); });

                        rdbt_stftsave_group.Controls.Add(rdbt_stftsave[i_rdbtr]);

                        if (string.Compare(prop_val.ToString(), input_params.stft_saving_options[i_key].ToString()) == 0)
                        {
                            default_idx = i_rdbtr;
                        }
                    }
                    rdbt_stftsave[default_idx].Checked = true;
                    prompt.Controls.Add(rdbt_stftsave_group);
                    top_box += spacing;
                    continue;
                }

                textBoxes[i_inp] = new TextBox()
                {
                    Left = left_box,
                    Top = top_box,
                    Width = mod_width_box,
                    Text = prop_val,
                    Font = new System.Drawing.Font(fontname, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
                };
                top_box += spacing;
                prompt.Controls.Add(textBoxes[i_inp]);
            }

            int idx_output_folder = name_idx_dict["output_folder"], idx_output_file_name = name_idx_dict["output_file_name"];
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

            Button confirmation = new Button()
            {
                Text = "Continue",
                Left = 490,
                Width = 120,
                Height = 30,
                Top = 900,
                DialogResult = DialogResult.OK,
                Font = new System.Drawing.Font(fontname, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            };
            confirmation.Click += (sender, e) => {
                prompt.Close();
            };
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;


            Button exitbutton = new Button()
            {
                Text = "Exit",
                Left = 620,
                Width = 120,
                Height = 30,
                Top = 900,
                Font = new System.Drawing.Font(fontname, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            };
            exitbutton.Click += (sender, e) =>
            {
                prompt.Close();
                Environment.Exit(1);
            }; 
            prompt.Controls.Add(exitbutton);
            prompt.CancelButton = exitbutton; 
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                for (int i_inp = 0; i_inp < number_inputs; i_inp++)
                {
                    prop_alias = input_params.nameAndProp.Keys.ElementAt(i_inp);
                    prop_name = input_params.nameAndProp[prop_alias].ToString();
                    if (string.Compare(prop_name, "refresh_display") == 0)
                    {                      
                        input_params.SetPropValue(prop_name, input_params.display_refresh_options[rdbt_refresh_choice]);
                        continue; 
                    }

                    if (string.Compare(prop_name, "display_quality") == 0)
                    {
                        input_params.SetPropValue(prop_name, input_params.gear_quality_dict[rdbt_quality_choice]);
                        continue;
                    }

                    if (string.Compare(prop_name, "window_type") == 0)
                    {
                        input_params.SetPropValue(prop_name, input_params.wintype_dict[rdbt_wintype_choice]);
                        continue;
                    }

                    if (string.Compare(prop_name, "stft_saving_option") == 0)
                    {
                        input_params.SetPropValue(prop_name, input_params.stft_saving_options[rdbt_stftsave_choice]);
                        continue;
                    }

                    string set_new_val = textBoxes[i_inp].Text;
                    input_params.SetPropValue(prop_name, set_new_val);
                }
            }

            return input_params;
        }

        public void Dispose()
        {
            prompt.Dispose();
        }
    }
}
