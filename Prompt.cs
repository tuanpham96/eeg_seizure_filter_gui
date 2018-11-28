﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
            Result = ParameterInputPrompt(text, caption); 

            //Result = ShowDialog(text, caption);
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
        
        private AppInputParameters ParameterInputPrompt(string title, string caption)
        {
            Font lbl_font = new Font(fontname, 10F, FontStyle.Bold, GraphicsUnit.Point);
            Font box_font = new Font(fontname, 10F, FontStyle.Regular, GraphicsUnit.Point);
            Font rdb_font = new Font(fontname, 8F, FontStyle.Regular, GraphicsUnit.Point);
            Font tab_font = new Font(fontname, 11F, FontStyle.Regular, GraphicsUnit.Point);
            Font ttl_font = new Font(fontname, 20F, FontStyle.Bold, GraphicsUnit.Point);
            prompt = new Form()
            {
                Width = 900,
                Height = 700,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };
          
            Label promptTitle = new Label()
            {
                Left = 50,
                Top = 20,
                Width = 600,
                Text = title,
                Height = 50,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = ttl_font
            };
            prompt.Controls.Add(promptTitle);

            TabControl tabcntl = new TabControl()
            {
                Location = new Point(50, 80),
                Size = new Size(800, 500),     
                Font = tab_font
            };

            Dictionary<string, Control> cntl_list = new Dictionary<string, Control>();

            bool IsTextbox(AppInputParameters.PropertypAndFormType.Form_Type _form_type)
            {
                return _form_type.CompareTo(AppInputParameters.PropertypAndFormType.Form_Type.Textbox) == 0;
            }

            Dictionary<string, Dictionary<string, object>> radio_choice_dictionaries = new Dictionary<string, Dictionary<string, object>>(); 
            
            int number_tabs = input_params.OptionSections.Count;
            TabPage[] tabpages = new TabPage[number_tabs];

            List<Label[]> labels = new List<Label[]>();
            List<TextBox[]> textboxes = new List<TextBox[]>();
            List<Panel[]> radio_groups = new List<Panel[]>();
            List<RadioButton[]>[] radios = new List<RadioButton[]>[number_tabs];
            for (int i = 0; i < number_tabs; i++)
            {
                
                string section_name = input_params.OptionSections.Keys.ElementAt(i);
                Dictionary<string, AppInputParameters.PropertypAndFormType> section_options = input_params.OptionSections[section_name];
                
                tabpages[i] = new TabPage()
                {
                    Location = new Point(50, 50),
                    Size = new Size(700, 500),
                    Text = section_name,     
                };

                int number_options = section_options.Count;
                int num_boxes = section_options.Where(x =>
                    IsTextbox(x.Value.form_type))
                    .ToArray().Length;
                int num_radios = number_options - num_boxes;


                labels.Add(new Label[number_options]);
                textboxes.Add(new TextBox[num_boxes]);
                radio_groups.Add(new Panel[num_radios]);
                radios[i] = new List<RadioButton[]>(); 
                int cur_box_cnt = 0, cur_rad_cnt = 0;
                
                int top_label = 70, left_label = 10, width_label = 300;
                int top_box = 70, left_box = 320, width_box = 420;
                int spacing = 32;

                string prop_alias, prop_name, prop_val;
                for (int j = 0; j < number_options; j++)
                {
                    prop_name = section_options.Keys.ElementAt(j);
                    prop_alias = section_options[prop_name].prop_alias;
                    prop_val = input_params.GetPropValue(prop_name).ToString();

                    labels[i][j] = new Label()
                    {
                        Left = left_label,
                        Top = top_label,
                        Text = prop_alias,
                        Width = width_label,
                        TextAlign = ContentAlignment.MiddleRight,
                        Font = lbl_font
                    };
                    tabpages[i].Controls.Add(labels[i][j]);
                    top_label += spacing;

                    if (IsTextbox(section_options[prop_name].form_type))
                    {
                        textboxes[i][cur_box_cnt] = new TextBox()
                        {
                            Left = left_box,
                            Top = top_box,
                            Width = width_box,
                            Text = prop_val,
                            Font = box_font
                        };

                        cntl_list.Add(prop_name, textboxes[i][cur_box_cnt]); 
                        tabpages[i].Controls.Add(textboxes[i][cur_box_cnt]);
                        cur_box_cnt++;
                    }
                    else // Radio groups 
                    {
                        radio_groups[i][cur_rad_cnt] = new Panel()
                        {
                            Location = new Point(left_box, top_box - 10),
                            Size = new Size(width_box, spacing + 5),
                            BorderStyle = BorderStyle.None
                        };

                        string dict_name = section_options[prop_name].dict_name;
                        /* Source 
                         * https://stackoverflow.com/questions/10206557/c-sharp-cast-dictionarystring-anytype-to-dictionarystring-object-involvin
                         */
                        IDictionary _dict_ = (IDictionary)input_params.GetPropValue(dict_name);
                        Dictionary<string, object> dict =
                            _dict_.Cast<dynamic>()
                            .ToDictionary(entry => (string)entry.Key, entry => entry.Value);
                        radio_choice_dictionaries.Add(prop_name, dict);

                        radios[i].Add(new RadioButton[dict.Count]);

                        int default_idx = -1;
                        for (int i_rdbtr = 0; i_rdbtr < dict.Count; i_rdbtr++)
                        {
                            var i_key = dict.Keys.ElementAt(i_rdbtr).ToString();
                            radios[i][cur_rad_cnt][i_rdbtr] = new RadioButton()
                            {
                                Location = new Point(20 + 100 * i_rdbtr, 5),
                                Size = new Size(100, spacing),
                                Font = rdb_font,
                                Text = i_key
                            };

                            radio_groups[i][cur_rad_cnt].Controls.Add(radios[i][cur_rad_cnt][i_rdbtr]);

                            if (string.Compare(prop_val.ToString(), dict[i_key].ToString()) == 0)
                            {
                                default_idx = i_rdbtr;
                            }
                        }
                        radios[i][cur_rad_cnt][default_idx].Checked = true;

                        cntl_list.Add(prop_name, radio_groups[i][cur_rad_cnt]);
                        tabpages[i].Controls.Add(radio_groups[i][cur_rad_cnt]);
                        cur_rad_cnt++;
                    }

                    top_box += spacing;

                }
                tabcntl.Controls.Add(tabpages[i]);
            }

            prompt.Controls.Add(tabcntl);

            /*
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
            */
            Button confirmation = new Button()
            {
                Text = "Continue",
                Left = 490,
                Width = 120,
                Height = 30,
                Top = 600,
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
                Top = 600,
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
                foreach (var item in cntl_list)
                {
                    string prop_name = item.Key;
                    Control cntl_obj = item.Value; 
                    if (cntl_obj is TextBox)
                    {
                        string set_new_val = cntl_obj.Text;
                        input_params.SetPropValue(prop_name, set_new_val);
                    } 
                    else if (cntl_obj is Panel)
                    {
                        Dictionary<string, object> prop_dict = radio_choice_dictionaries[prop_name];
                        // a more efficient way to find checked values
                        // https://stackoverflow.com/questions/1797907/which-radio-button-in-the-group-is-checked by SLaks
                        var checkedButton = cntl_obj.Controls.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.Checked);
                        string new_key = checkedButton.Text;
                        input_params.SetPropValue(prop_name, prop_dict[new_key]);
                    } 
                    else
                    {
                        throw new Exception("The Control object has to be either a `TextBox`" +
                            "or a `Panel` for radio groups, not" + cntl_obj.GetType().ToString());
                    }
                                        
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
