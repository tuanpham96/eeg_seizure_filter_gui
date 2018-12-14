using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace seizure_filter
{

    /* Prompt: this prompts a dialog to set up the application input parameters, 
     * necessarily the configuration for the application before plotting the streaming 
     * data in `MainForm`. 
     * 
     * Particularly, the main goal is to create an `ApplicationInputParameters`
     * object to save into `MainForm.APP_INP_PRM`; by either load an existing configuration 
     * file (`ConfigPrompt`) then edit in `MainPrompt` or create an entirely new configuration
     * file by editing the parameter fields in `MainPrompt`. 
     * 
     * The dialog prompt implementation is much inspired fron Gideon Mulder in stackoverflow
     * Source: https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
     */
    public class Prompt : IDisposable
    {
        #region Prompt attributes 
        /* + ConfigPrompt:      configuration prompt, for prompting whether to load a configuration file and plot, modify one or create one 
         * + MainPrompt:        main dialog prompt to edit the configuration parameters, or even for double-checking before plotting 
         * + Result:            resulting `ApplicationInputParameters` instance for `MainForm.APP_INP_PRM` configuration 
         * + AppInpPrm:         private `ApplicationInputParameters` object to modify within `Prompt` 
         * + fontname:          name of font to use for the dialogs 
         */
        private Form ConfigPrompt { get; set; }
        private Form MainPrompt { get; set; }
        public ApplicationInputParameters Result { get; }
        private ApplicationInputParameters AppInpPrm;
        private string fontname { get; }
        #endregion  Prompt attributes 

        #region Configuration options and warnings 
        /* Config_Options: enum to represent 3 configuration options, currently:
         * (1) CREATE_NEW:          create a completely new configuration file from the default file
         *                          named `_config_file.txt` in the folder. Then it would prompt 
         *                          `MainPrompt` to start editing parameters. The new file would be named
         *                          `customized_config_file.txt`, or you could edit the name later on 
         *                          in `MainPrompt`. However, please don't name it `_config_file.txt` 
         *                          because `CREATE_NEW` would necessarily rely on this file to instantiate 
         *                          `AppInpPrm` in this object.
         * (2) LOAD_AND_PLOT:       load an existing configuration file and plot, and without starting `MainPrompt` 
         *                          to edit any parameters at all. This means that, if the configuration file
         *                          is loaded successfully, the program will go straight to `MainForm` to plot. 
         * (3) LOAD_AND_MODIFY:     load an exisiting configuration but modify the parameters in the `MainPrompt` 
         *                          as one desires before moving on to plotting in `MainForm`. 
         */
        private enum Config_Options { CREATE_NEW, LOAD_AND_PLOT, LOAD_AND_MODIFY }
        /* LoadConfig_Warning: enum to represent the warnings about configuration file loading issues 
         * (1) NO_WARNING:                  no warning 
         * (2) NO_EXISTING_FILE_TO_PLOT:    would happen with `LOAD_AND_PLOT` but the file does not exist
         * (3) NO_EXISTING_FILE_TO_MODIFY:  would happen with `LOAD_AND_MODIFY` 
         * (4) POSSIBLE_OVERWRITE_FILE:     would happen with either `CREATE_NEW` or `LOAD_AND_MODIFY` if 
         *                                  there's already an existing configuration file 
         * 
         * + NOTE:  this was added on almost last and I hadn't put much thought in it so the way this is 
         *          being used is not quite as though out yet. The initial thought was to throw errors 
         *          if the file does not exist and stop the program; and print a message if an existing file
         *          could be overwritten in `MainPrompt` but I just did not have the time to implement this. 
         *          In short, much can be improved 
         */
        private enum LoadConfig_Warning { NO_WARNING, NO_EXISTING_FILE_TO_PLOT, NO_EXISTING_FILE_TO_MODIFY, POSSIBLE_OVERWRITE_FILE }
        /* Config_Description: a struct to store the description of the configuration prompt options
         * + LABEL:     configuration prompt option, from `Config_Options` enum 
         * + TOOLTIP:   a more detailed description for the tooltip when the mouse hovers over the option 
         */
        private struct Config_Description
        {
            public Config_Options LABEL;
            public string TOOLTIP;
            public Config_Description(Config_Options _label_, string _tooltip_)
            {
                LABEL = _label_;
                TOOLTIP = _tooltip_;
            }
        }
        #endregion Configuration options and warnings 

        #region Prompt constructor 
        /* Prompt initialization 
         * + LAYOUT: 
         *      - Call the configuration option dialog: `ConfigPrompt` 
         *      - Evaluate the configuration loading options 
         *      - Initialize `AppInpPrm` for application input parameters
         *      - Either return the instance of `AppInpPrm` as `Result` from a configuration file 
         *          OR edit the parameters in `MainPrompt` 
         * + INPUT: 
         *      - title:        title for `MainPrompt`
         *      - caption:      caption for `MainPrompt`
         */
        public Prompt(string title, string caption)
        {
            fontname = "Arial";

            // Call the configuration option dialog: `ConfigPrompt` 
            ConfigurationOptionPrompt(out Config_Options config_option, out string config_file_path, out LoadConfig_Warning warning);

            // Evaluate the configuration loading options 
            bool create_new_config = false; 
            if (config_option == Config_Options.CREATE_NEW)
            {
                create_new_config = true; 
            }
            if ((warning == LoadConfig_Warning.NO_EXISTING_FILE_TO_PLOT && config_option == Config_Options.LOAD_AND_PLOT) ||
                (warning == LoadConfig_Warning.NO_EXISTING_FILE_TO_MODIFY && config_option == Config_Options.LOAD_AND_MODIFY))
            {
                throw new Exception("Cannot start the program because the configuration file does not exist!");
            }

            // Initialize `AppInpPrm` for application input parameters
            AppInpPrm = new ApplicationInputParameters(create_new_config, config_file_path); 

            if (config_option == Config_Options.LOAD_AND_PLOT)
            {
                // Return the instance of `AppInpPrm` as `Result` from a configuration file 
                // if choose `LOAD_AND_PLOT` option 
                Result = AppInpPrm;
                Result.CompleteInitialize(); 
            } 
            else
            {
                // Edit the parameters in `MainPrompt` 
                Result = ParameterInputPrompt(title, caption);
            }      
        }
        #endregion Prompt Constructor 

        #region Configuration option prompt 
        
        private void ConfigurationOptionPrompt(out Config_Options config_option, out string config_file_path, out LoadConfig_Warning warning)
        {
            Font font_style = new Font(fontname, 12F, FontStyle.Regular, GraphicsUnit.Point);
            ConfigPrompt = new Form()
            {
                Size = new Size(400, 400),
                FormBorderStyle = FormBorderStyle.Sizable,
                Text = "Configuration loading option" ,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };
            ConfigPrompt.Controls.Add(new Label {
                Location = new Point(20, 20), 
                Size = new Size(400, 50), 
                Text = "Would you wish to?",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = font_style
            });

            Dictionary<string, Config_Description> radio_options = new Dictionary<string, Config_Description>
            {
                {  "Create a new configuration file", new Config_Description() {
                    LABEL = Config_Options.CREATE_NEW,
                    TOOLTIP = "The Main Prompt will appear, with values from the preset default configuration file.\n" +
                    "You can specify the location to save a new configuration file in the Main Prompt"
                }},
                { "Load an existing configuration file and plot", new Config_Description() {
                    LABEL = Config_Options.LOAD_AND_PLOT,
                    TOOLTIP = "The Main Prompt will NOT appear.\n" +
                    "But you would need to specify the location of the configuration file."
                }},
                { "Load and change a configuration file", new Config_Description() {
                    LABEL = Config_Options.LOAD_AND_MODIFY, 
                    TOOLTIP = "The Main Prompt will appear.\n" +
                    "You would need to specific the location of the configuration file.\n" +
                    "You can change the configuration parameters.\n" +
                    "You can then either save your changes to the existing file, or in a new file. "
                }}
            };

            RadioButton[] Radio_List = new RadioButton[radio_options.Count];
            int cur_radio = 0; 
            int left_radio = 20, width_radio = 350, top_radio = 65, height_radio = 30, spacing = 10; 
            foreach (var item in radio_options) 
            {
                string option = item.Key;
                string description = item.Value.TOOLTIP;
                Radio_List[cur_radio] = new RadioButton
                {
                    Location = new Point(left_radio, top_radio),
                    Size = new Size(width_radio, height_radio),
                    Font = font_style, 
                    Text = option 
                };
                ConfigPrompt.Controls.Add(Radio_List[cur_radio]);
                top_radio += height_radio + spacing;

                (new ToolTip()).SetToolTip(Radio_List[cur_radio], description);
            }
            Radio_List[0].Checked = true;

            string current_directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            Button FolderDialogButton = new Button()
            {
                Text = "Choose configuration file",
                Location = new Point(left_radio, top_radio + 10),
                Size = new Size(width_radio, 30),
                Font = font_style,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            TextBox ConfigFileText = new TextBox()
            {
                Text = current_directory + "\\customized_config_file.txt", 
                Location = new Point(left_radio, FolderDialogButton.Bottom + 10),
                Size = new Size(width_radio, 30),
                Font = font_style,
                BackColor = Color.WhiteSmoke
            }; 
            FolderDialogButton.FlatAppearance.MouseOverBackColor = Color.Silver;
            FolderDialogButton.Click += (sender, e) => {
                OpenFileDialog folderBrowser = new OpenFileDialog()
                {
                    ValidateNames = false,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    InitialDirectory = current_directory
                };
                folderBrowser.FileName = "Choose the configuration file here";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    ConfigFileText.Text = Path.GetFullPath(folderBrowser.FileName);
                }
            };
            ConfigPrompt.Controls.Add(FolderDialogButton);
            ConfigPrompt.Controls.Add(ConfigFileText);

            Button ConfirmationButton = new Button()
            {
                Text = "Continue",
                Location = new Point(20, ConfigFileText.Bottom + 50), 
                Size = new Size(100, 30),
                DialogResult = DialogResult.OK,
                Font = font_style,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            ConfirmationButton.FlatAppearance.BorderSize = 1;
            ConfirmationButton.FlatAppearance.MouseOverBackColor = Color.Silver;

            ConfirmationButton.Click += (sender, e) => {
                ConfigPrompt.Close();
            };
            ConfigPrompt.Controls.Add(ConfirmationButton);
            ConfigPrompt.AcceptButton = ConfirmationButton;

            Button ExitButton = new Button()
            {
                Text = "Exit",
                Location = new Point(FolderDialogButton.Right - ConfirmationButton.Width, ConfirmationButton.Top),
                Size = new Size(100, 30),
                BackColor = Color.WhiteSmoke,
                Font = font_style, 
                FlatStyle = FlatStyle.Flat
            }; 
            ExitButton.FlatAppearance.BorderSize = 1;
            ExitButton.FlatAppearance.MouseOverBackColor = Color.Silver;
            ExitButton.Click += (sender, e) =>
            {
                ConfigPrompt.Close();
                Environment.Exit(1);
            };
            ConfigPrompt.Controls.Add(ExitButton);
            ConfigPrompt.CancelButton = ExitButton;

            config_option = Config_Options.CREATE_NEW;
            config_file_path = ConfigFileText.Text;
            warning = LoadConfig_Warning.NO_WARNING; 
            if (ConfigPrompt.ShowDialog() == DialogResult.OK)
            {
                var checkedButton = ConfigPrompt.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                config_option = radio_options[checkedButton.Text].LABEL;
                config_file_path = ConfigFileText.Text; 
                if ( (config_option == Config_Options.CREATE_NEW || config_option == Config_Options.LOAD_AND_MODIFY) && File.Exists(config_file_path))
                {
                    warning = LoadConfig_Warning.POSSIBLE_OVERWRITE_FILE; 
                } 
                if (config_option == Config_Options.LOAD_AND_PLOT && !File.Exists(config_file_path))
                {
                    warning = LoadConfig_Warning.NO_EXISTING_FILE_TO_PLOT; 
                }
                if (config_option == Config_Options.LOAD_AND_MODIFY && !File.Exists(config_file_path))
                {
                    warning = LoadConfig_Warning.NO_EXISTING_FILE_TO_MODIFY;
                }
            }
        }
        #endregion Configuration option prompt 

        #region Main prompt dialog to edit the parameters 
        private ApplicationInputParameters ParameterInputPrompt(string title, string caption)
        {
            Size prompt_size = new Size(1000, 700);
            Point title_coord = new Point(50, 20);
            Size title_size = new Size(600, 50); 

            Point tabcntl_coord = new Point(50, 80);
            Size tabcntl_size = new Size(900, 500); 
            Point tabpage_coord = new Point(50, 20);
            Size tabpage_size = new Size(800, 500);
            const int max_radio_col = 4;
            const int top_label_begin = 30, left_label = 10, width_label = 300;
            const int top_box_begin = top_label_begin, left_box = 320, width_box = 500;
            const int width_clrbt = width_box / 3;
            const int general_spacing = 32;
            const int top_offset_radiogroup = 10, top_offset_radiobutton = 5, left_offset_radiobutton = 20, width_radio = 120;
            const int radio_spacing = 25;

            const int horz_offset_folderdialog = 200, vert_offset_folderdialog = 10;

            Size end_button_size = new Size(120, 30);
            const int top_offset_endbutton = 20;
            const int horz_spacing_endbutton = 10;

            Font lbl_font = new Font(fontname, 10F, FontStyle.Bold, GraphicsUnit.Point);
            Font box_font = new Font(fontname, 10F, FontStyle.Regular, GraphicsUnit.Point);
            Font rdb_font = new Font(fontname, 8F, FontStyle.Regular, GraphicsUnit.Point);
            Font tab_font = new Font(fontname, 11F, FontStyle.Regular, GraphicsUnit.Point);
            Font ttl_font = new Font(fontname, 20F, FontStyle.Bold, GraphicsUnit.Point);

            Dictionary<string, Dictionary<string, object>> radio_choice_dictionaries = new Dictionary<string, Dictionary<string, object>>();
            int number_tabs = AppInpPrm.OptionSections.Count;

            MainPrompt = new Form()
            {
                Size = prompt_size,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };
            Label PromptTitle = new Label()
            {
                Location = title_coord,
                Size = title_size,
                Text = title,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = ttl_font
            };
            MainPrompt.Controls.Add(PromptTitle);
            TabControl TabCntl = new TabControl()
            {
                Location = tabcntl_coord,
                Size = tabcntl_size,
                Font = tab_font,
                // Appearance = TabAppearance.FlatButtons               
            };
            TabPage[] TabPages = new TabPage[number_tabs];
            Dictionary<string, Control> Cntl_List = new Dictionary<string, Control>();
            List<RadioButton> Radio_List = new List<RadioButton>();

            for (int i = 0; i < number_tabs; i++)
            {                
                string section_name = AppInpPrm.OptionSections.Keys.ElementAt(i);
                Dictionary<string, ApplicationInputParameters.PropertypAndFormType> section_options = AppInpPrm.OptionSections[section_name];
                
                TabPages[i] = new TabPage()
                {
                    Location = tabpage_coord, 
                    Size = tabpage_size, 
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
                    var prop_val = AppInpPrm.GetPropValue(prop_name);

                    Cntl_List.Add("label_of_" + prop_name, new Label()
                    {
                        Location = new Point(left_label, top_label), 
                        Text = prop_alias,
                        Width = width_label,
                        TextAlign = ContentAlignment.MiddleRight,
                        Font = lbl_font
                    });
                    TabPages[i].Controls.Add(Cntl_List["label_of_" + prop_name]);
                    top_label += general_spacing;

                    if (section_options[prop_name].IsTextBox())
                    {
                        Cntl_List.Add(prop_name, new TextBox()
                        {
                            Location = new Point(left_box, top_box),
                            Width = width_box,
                            Text = prop_val.ToString(),
                            Font = box_font,
                            BorderStyle = BorderStyle.FixedSingle
                        });
                    }
                    else if (section_options[prop_name].IsRadiobuttonGroup())
                    {
                        string dict_name = section_options[prop_name].dict_name;
                        /* Source 
                         * https://stackoverflow.com/questions/10206557/c-sharp-cast-dictionarystring-anytype-to-dictionarystring-object-involvin
                         */
        IDictionary _dict_ = (IDictionary)AppInpPrm.GetPropValue(dict_name);
                        Dictionary<string, object> dict = _dict_.Cast<dynamic>().ToDictionary(entry => (string)entry.Key, entry => entry.Value);
                        radio_choice_dictionaries.Add(prop_name, dict);

                        int radio_group_height = (int)Math.Ceiling(((double)dict.Count) / ((double)max_radio_col)) * radio_spacing + top_offset_radiogroup;
                        Cntl_List.Add(prop_name, new Panel()
                        {
                            Location = new Point(left_box, top_box - top_offset_radiogroup),
                            Size = new Size(width_box, radio_group_height),
                            BorderStyle = BorderStyle.None
                            
                        });
                        top_box += radio_group_height - top_offset_radiogroup - radio_spacing;
                        top_label += radio_group_height - top_offset_radiogroup - radio_spacing; 

                        int default_idx = -1;
                        int cur_radio_row = 0, cur_radio_col = 0; 
                        for (int i_rdbtr = 0; i_rdbtr < dict.Count; i_rdbtr++)
                        {
                            cur_radio_row = (int)Math.Floor(((double)i_rdbtr) / ((double) max_radio_col));
                            cur_radio_col = i_rdbtr % max_radio_col; 
                            var i_key = dict.Keys.ElementAt(i_rdbtr).ToString();
                            Radio_List.Add(new RadioButton()
                            {
                                Location = new Point(left_offset_radiobutton + width_radio * cur_radio_col, top_offset_radiobutton + radio_spacing * cur_radio_row),
                                Size = new Size(width_radio, radio_spacing),
                                Font = rdb_font,
                                Text = i_key
                            });
                            Cntl_List[prop_name].Controls.Add(Radio_List[Radio_List.Count - 1]);

                            if (string.Compare(prop_val.ToString(), dict[i_key].ToString()) == 0)
                            {
                                default_idx = Radio_List.Count - 1;
                            }
                        }
                        Radio_List[default_idx].Checked = true;
                    }
                    else if (section_options[prop_name].IsColorButton())
                    {
                        Cntl_List.Add(prop_name, new Button()
                        {
                            Location = new Point(left_box, top_box),       
                            Width = width_clrbt,
                            Font = box_font,
                            BackColor = (Color)prop_val,
                            FlatStyle = FlatStyle.Flat,
                        });
                        (new ToolTip()).SetToolTip(Cntl_List[prop_name], "Click to select " + prop_alias); 
                        ((Button)Cntl_List[prop_name]).FlatAppearance.BorderSize = 0;
                        Cntl_List[prop_name].Click += (sender, e) =>
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
                    TabPages[i].Controls.Add(Cntl_List[prop_name]);
                    top_box += general_spacing;

                }
                TabCntl.Controls.Add(TabPages[i]);
            }

            MainPrompt.Controls.Add(TabCntl);

            Button FolderDialogButton = new Button()
            {
                Text = "Choose folder and file name",
                Location = new Point(left_box + horz_offset_folderdialog, Cntl_List["output_file_name"].Top + general_spacing), 
                Size = new Size(width_box - horz_offset_folderdialog, Cntl_List["output_file_name"].Height + vert_offset_folderdialog),
                Font = box_font,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };            
            FolderDialogButton.FlatAppearance.MouseOverBackColor = Color.Silver;
            FolderDialogButton.Click += (sender, e) => {
                // https://stackoverflow.com/questions/11624298/how-to-use-openfiledialog-to-select-a-folder by Daniel Ballinger 
                OpenFileDialog folderBrowser = new OpenFileDialog()
                {
                    ValidateNames = false,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    InitialDirectory = AppInpPrm.output_folder
                };
                folderBrowser.FileName = "Choose folder then enter desired file name here";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(folderBrowser.FileName);
                    string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                    Cntl_List["output_folder"].Text = folderPath;
                    Cntl_List["output_file_name"].Text = fileName;
                }
            };
            TabPages[0].Controls.Add(FolderDialogButton);

            Button ConfirmationButton = new Button()
            {
                Text = "Continue",
                Location = new Point(TabCntl.Left + TabCntl.Width - end_button_size.Width * 2 - horz_spacing_endbutton, 
                        TabCntl.Top + TabCntl.Height + top_offset_endbutton),
                Size = end_button_size,
                DialogResult = DialogResult.OK,
                Font = lbl_font,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            ConfirmationButton.FlatAppearance.MouseOverBackColor = Color.Silver;

            ConfirmationButton.Click += (sender, e) => {
                MainPrompt.Close();
            };
            MainPrompt.Controls.Add(ConfirmationButton);
            MainPrompt.AcceptButton = ConfirmationButton;

            Button ExitButton = new Button()
            {
                Text = "Exit",
                Location = new Point(ConfirmationButton.Left + ConfirmationButton.Width + horz_spacing_endbutton,
                        TabCntl.Top + TabCntl.Height + top_offset_endbutton),
                Size = end_button_size,
                Font = lbl_font,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            ExitButton.FlatAppearance.MouseOverBackColor = Color.Silver;
            ExitButton.Click += (sender, e) =>
            {
                MainPrompt.Close();
                Environment.Exit(1);
            };
            MainPrompt.Controls.Add(ExitButton);
            MainPrompt.CancelButton = ExitButton;
            
            if (MainPrompt.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, string> description_dict = new Dictionary<string, string>(); 
                foreach (var item in Cntl_List)
                {
                    string prop_name = item.Key;
                    Control cntl_obj = item.Value;
                    if (cntl_obj is TextBox)
                    {
                        AppInpPrm.SetPropValue(prop_name, cntl_obj.Text);
                        description_dict[prop_name] = "<NAN>"; 
                    }
                    else if (cntl_obj is Panel)
                    {
                        Dictionary<string, object> prop_dict = radio_choice_dictionaries[prop_name];
                        // an efficient way to find checked values
                        // https://stackoverflow.com/questions/1797907/which-radio-button-in-the-group-is-checked by SLaks
                        var checkedButton = cntl_obj.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                        AppInpPrm.SetPropValue(prop_name, prop_dict[checkedButton.Text]);
                        description_dict[prop_name] = checkedButton.Text;
                    }
                    else if (cntl_obj is Button)
                    {
                        AppInpPrm.SetPropValue(prop_name, cntl_obj.BackColor);
                        description_dict[prop_name] = AppInpPrm.GetPropValue(prop_name).ToString();
                    }
                    else
                    {
                        continue;
                    }
                }
                AppInpPrm.CompleteInitialize(); // for all the necessary checking and parsing of parameters

                string config_file = Cntl_List["config_path"].Text;
                AppInpPrm.WriteConfigurationFile(config_file, description_dict);
    
            }

            return AppInpPrm;
        }
        #endregion Main prompt dialog to edit the parameters 

        /* Dispose method for this object after being used */
        public void Dispose()
        {
            if (MainPrompt != null)
            {
                MainPrompt.Dispose();
            }
            if (ConfigPrompt != null)
            {
                ConfigPrompt.Dispose();
            }
        }
    }
}
