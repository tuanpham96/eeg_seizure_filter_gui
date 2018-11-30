using System;
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
                Font = tab_font,
                Appearance = TabAppearance.FlatButtons               
            };

            Dictionary<string, Dictionary<string, object>> radio_choice_dictionaries = new Dictionary<string, Dictionary<string, object>>(); 
            
            int number_tabs = input_params.OptionSections.Count;
            TabPage[] tabpages = new TabPage[number_tabs];

            Dictionary<string, Control> cntl_list = new Dictionary<string, Control>();
            List<RadioButton> radios = new List<RadioButton>();
        
            const int top_label_begin = 70, left_label = 10, width_label = 300;
            const int top_box_begin = 70, left_box = 320, width_box = 420;
            const int spacing = 32;

            for (int i = 0; i < number_tabs; i++)
            {                
                string section_name = input_params.OptionSections.Keys.ElementAt(i);
                Dictionary<string, AppInputParameters.PropertypAndFormType> section_options = input_params.OptionSections[section_name];
                
                tabpages[i] = new TabPage()
                {
                    Location = new Point(50, 50),
                    Size = new Size(700, 500),
                    Text = section_name
                };
                
                int number_options = section_options.Count;

                int top_label = top_box_begin;
                int top_box = top_label_begin;

                string prop_alias, prop_name; 
                for (int j = 0; j < number_options; j++)
                {
                    prop_name = section_options.Keys.ElementAt(j);
                    prop_alias = section_options[prop_name].prop_alias;
                    var prop_val = input_params.GetPropValue(prop_name);

                    cntl_list.Add("label_of_" + prop_name, new Label()
                    {
                        Left = left_label,
                        Top = top_label,
                        Text = prop_alias,
                        Width = width_label,
                        TextAlign = ContentAlignment.MiddleRight,
                        Font = lbl_font
                    });
                    tabpages[i].Controls.Add(cntl_list["label_of_" + prop_name]);
                    top_label += spacing;

                    if (section_options[prop_name].IsTextBox())
                    {
                        cntl_list.Add(prop_name, new TextBox()
                        {
                            Left = left_box,
                            Top = top_box,
                            Width = width_box,
                            Text = prop_val.ToString(),
                            Font = box_font,
                            BorderStyle = BorderStyle.FixedSingle
                        });

                    }
                    else if (section_options[prop_name].IsRadiobuttonGroup())
                    {
                        cntl_list.Add(prop_name, new Panel()
                        {
                            Location = new Point(left_box, top_box - 10),
                            Size = new Size(width_box, spacing + 5),
                            BorderStyle = BorderStyle.None
                        });

                        string dict_name = section_options[prop_name].dict_name;
                        /* Source 
                         * https://stackoverflow.com/questions/10206557/c-sharp-cast-dictionarystring-anytype-to-dictionarystring-object-involvin
                         */
                        IDictionary _dict_ = (IDictionary)input_params.GetPropValue(dict_name);
                        Dictionary<string, object> dict =
                            _dict_.Cast<dynamic>()
                            .ToDictionary(entry => (string)entry.Key, entry => entry.Value);
                        radio_choice_dictionaries.Add(prop_name, dict);

                        int default_idx = -1;
                        for (int i_rdbtr = 0; i_rdbtr < dict.Count; i_rdbtr++)
                        {
                            var i_key = dict.Keys.ElementAt(i_rdbtr).ToString();
                            radios.Add(new RadioButton()
                            {
                                Location = new Point(20 + 100 * i_rdbtr, 5),
                                Size = new Size(100, spacing),
                                Font = rdb_font,
                                Text = i_key
                            });
                            cntl_list[prop_name].Controls.Add(radios[radios.Count - 1]);

                            if (string.Compare(prop_val.ToString(), dict[i_key].ToString()) == 0)
                            {
                                default_idx = radios.Count - 1;
                            }
                        }
                        radios[default_idx].Checked = true;
                    }
                    else if (section_options[prop_name].IsColorButton())
                    {
                        cntl_list.Add(prop_name, new Button()
                        {
                            Left = left_box,
                            Top = top_box,
                            Width = width_box / 3,
                            Font = box_font,
                            BackColor = (Color)prop_val,
                            FlatStyle = FlatStyle.Flat,
                        });
                        (new ToolTip()).SetToolTip(cntl_list[prop_name], "Click to select " + prop_alias); 
                        ((Button)cntl_list[prop_name]).FlatAppearance.BorderSize = 0;
                        cntl_list[prop_name].Click += (sender, e) =>
                        {
                            ColorDialog colorDialog = new ColorDialog()
                            {
                                AllowFullOpen = true,
                                ShowHelp = false,
                                Color = (sender as Button).BackColor
                            };
                            if (colorDialog.ShowDialog() == DialogResult.OK)
                            {
                                (sender as Button).BackColor = colorDialog.Color;
                            }
                        };
                    }
                    tabpages[i].Controls.Add(cntl_list[prop_name]);
                    top_box += spacing;

                }
                tabcntl.Controls.Add(tabpages[i]);
            }

            prompt.Controls.Add(tabcntl);

            int folderdialog_horzshift = 200; 
            Button openfolderdialog = new Button()
            {
                Text = "Choose folder and file name",
                Height = cntl_list["output_file_name"].Height + 10,
                Left = left_box + folderdialog_horzshift,
                Width = width_box - folderdialog_horzshift,
                Top = cntl_list["output_file_name"].Top + spacing,
                Font = box_font,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };            
            openfolderdialog.FlatAppearance.MouseOverBackColor = Color.Silver;

            openfolderdialog.Click += (sender, e) => {
                // https://stackoverflow.com/questions/11624298/how-to-use-openfiledialog-to-select-a-folder by Daniel Ballinger 
                OpenFileDialog folderBrowser = new OpenFileDialog()
                {
                    ValidateNames = false,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    InitialDirectory = input_params.output_folder
                };
                folderBrowser.FileName = "Choose folder then enter desired file name here";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(folderBrowser.FileName);
                    string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                    cntl_list["output_folder"].Text = folderPath;
                    cntl_list["output_file_name"].Text = fileName;
                }
            };

            tabpages[0].Controls.Add(openfolderdialog);

            Button confirmation = new Button()
            {
                Text = "Continue",
                Left = tabcntl.Left + tabcntl.Width - 120*2 -10 ,
                Width = 120,
                Height = 30,
                Top = 600,
                DialogResult = DialogResult.OK,
                Font = lbl_font,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            confirmation.FlatAppearance.MouseOverBackColor = Color.Silver;

            confirmation.Click += (sender, e) => {
                prompt.Close();
            };
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            Button exitbutton = new Button()
            {
                Text = "Exit",
                Left = confirmation.Left + confirmation.Width + 10,
                Width = 120,
                Height = 30,
                Top = 600,
                Font = lbl_font,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            exitbutton.FlatAppearance.MouseOverBackColor = Color.Silver;
            exitbutton.Click += (sender, e) =>
            {
                prompt.Close();
                Environment.Exit(1);
            };
            prompt.Controls.Add(exitbutton);
            prompt.CancelButton = exitbutton;
            
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                string delim = "\t";
                string current_directorty = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                Console.WriteLine(current_directorty); 
                string config_file = @current_directorty + "/config_file.txt";
                File.WriteAllText(config_file, string.Format("Configuration, generated on {0:f}\n", DateTime.Now));
                File.AppendAllText(config_file, 
                    "Property" + delim + 
                    "Type" + delim +
                    "Value" + delim + 
                    "Description" + "\n");
                foreach (var item in cntl_list)
                {
                    string prop_name = item.Key;
                    Control cntl_obj = item.Value;
                    string radio_new = "<NAN>"; 
                    if (cntl_obj is TextBox)
                    {
                        input_params.SetPropValue(prop_name, cntl_obj.Text);
                    } 
                    else if (cntl_obj is Panel)
                    {
                        Dictionary<string, object> prop_dict = radio_choice_dictionaries[prop_name];
                        // a more efficient way to find checked values
                        // https://stackoverflow.com/questions/1797907/which-radio-button-in-the-group-is-checked by SLaks
                        var checkedButton = cntl_obj.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                        input_params.SetPropValue(prop_name, prop_dict[checkedButton.Text]);
                        radio_new = checkedButton.Text;
                    } 
                    else if (cntl_obj is Button)
                    {
                        input_params.SetPropValue(prop_name, cntl_obj.BackColor);
                    }
                    else 
                    {
                        continue; 
                    }
                    if (!prop_name.Contains("label_of"))
                    {
                        File.AppendAllText(config_file, 
                            prop_name + delim + 
                            input_params.GetPropValue(prop_name).GetType() + delim +
                            input_params.GetPropValue(prop_name) + delim +
                            radio_new + "\n");
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
