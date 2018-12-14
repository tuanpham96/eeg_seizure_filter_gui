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
        /* ConfigurationOptionPrompt: ask for configuration options, refer to description of enum `Config_Options` for details 
         * This is called before `MainPrompt` to decide where to load the configuration file from first and 
         * whether to start `MainPrompt` at all. 
         * + LAYOUT: 
         *      - Initialize the dialog and title 
         *      - Create RadioButton object for each option 
         *      - Create components for choosing the directory of configuration file 
         *      - Create buttons to either continue or exit 
         *      - Return configuration option, file location and warning 
         * + INPUT: None
         * + OUTPUT: 
         *      - config_option:            configuration option, refer to `Config_Options` enum 
         *      - config_file_path:         path to look for or configuration file 
         *      - warning:                  configuration file loading warnings, refer to `LoadConfig_Warning` enum 
         * 
         */
        private void ConfigurationOptionPrompt(out Config_Options config_option, out string config_file_path, out LoadConfig_Warning warning)
        {
            // (1) Initialize the dialog
            Font font_style = new Font(fontname, 12F, FontStyle.Regular, GraphicsUnit.Point);
            ConfigPrompt = new Form()
            {
                Size = new Size(400, 400),
                FormBorderStyle = FormBorderStyle.Sizable,
                Text = "Configuration loading option" ,
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };
            // Dialog title 
            ConfigPrompt.Controls.Add(new Label {
                Location = new Point(20, 20), 
                Size = new Size(400, 50), 
                Text = "Would you wish to?",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = font_style
            });

            // Radiobutton options and tooltip description (for mouse hovering) 
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

            // Position and size parameters 
            int cur_radio = 0; 
            int left_radio = 20, width_radio = 350, top_radio = 65, height_radio = 30, spacing = 10; 

            // (2) Create RadioButton object for each option 
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
                top_radio += height_radio + spacing; // for the next one 

                (new ToolTip()).SetToolTip(Radio_List[cur_radio], description); 
            }
            Radio_List[0].Checked = true;

            // (3) Create components for choosing the directory of configuration file 
            string current_directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            // FolderDialogButton 
            Button FolderDialogButton = new Button()
            {
                Text = "Choose configuration file",
                Location = new Point(left_radio, top_radio + 10),
                Size = new Size(width_radio, 30),
                Font = font_style,
                BackColor = Color.WhiteSmoke,
                FlatStyle = FlatStyle.Flat
            };
            // TextBox to report the name 
            TextBox ConfigFileText = new TextBox()
            {
                Text = current_directory + "\\customized_config_file.txt", 
                Location = new Point(left_radio, FolderDialogButton.Bottom + 10),
                Size = new Size(width_radio, 30),
                Font = font_style,
                BackColor = Color.WhiteSmoke
            };
            // Report which file it is in ConfigFileText after action on FolderDialogButton
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

            // (4) Create buttons to either continue or exit 
            // ConfirmationButton to continue with either MainPrompt (then MainForm) or 
            // MainForm directly 
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

            // ExitButton to exit the program before starting either MainPrompt nor MainForm 
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
            // (5) Return configuration option, file location and warning 
            if (ConfigPrompt.ShowDialog() == DialogResult.OK) 
            {
                // an efficient way to find checked RadioButton of a given group 
                // https://stackoverflow.com/questions/1797907/which-radio-button-in-the-group-is-checked by SLaks
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
        /* ParameterInputPrompt: edit parametrs to start and run the application in `MainForm`  
         * This is called after `ConfigurationOptionPrompt` to modify parameters in `AppInpPrm.OptionSections`
         * + LAYOUT:
         *      - Initialize general variables related to dialog's component's size, position and font 
         *      - Initialize the dialog and its title + tab components: 
         *              * Each tab represents a section in `AppInpPrm.OptionSections`
         *              * Within each tab are the parameters in the corresponding section to edit 
         *      - Initialize TabPage for each section
         *      - For each element in each section, add a label and proccess the element according to the type: 
         *              * Textbox: just add the TextBox
         *              * RadiobuttonGroup:
         *                  (a) Obtain the corresponding option dictionary  
         *                  (b) Add a panel to group the radio buttons
         *                  (c) For each option, add a `RadioButton` button object showing the description
         *                  (d) Check the button representing the value loaded from the configuration file
         *              * ColorButton: create a `Button` that can prompt a `ColorDialog`
         *      - Add a button to prompt a folder dialog for choosing the output file directory 
         *      - Process the changes made after the prompt is closed (to continue) and save configuration set up to a file
         * + INPUT:
         *      - title:                    the tile of the dialog 
         *      - caption:                  the caption of the dialog 
         * + OUTPUT: 
         *      - a modified `AppInpPrm` 
         * 
         */
        private ApplicationInputParameters ParameterInputPrompt(string title, string caption)
        {
            // (1) Initialize general variables related to dialog's component's size, position and font 
            // General dialog variables
            Size prompt_size = new Size(1000, 700);
            Point title_coord = new Point(50, 20);
            Size title_size = new Size(600, 50); 
            // General tab variables 
            Point tabcntl_coord = new Point(50, 80);
            Size tabcntl_size = new Size(900, 500); 
            Point tabpage_coord = new Point(50, 20);
            Size tabpage_size = new Size(800, 500);

            // Variables for subcomponents in the tab
            const int max_radio_col = 4;
            const int top_label_begin = 30, left_label = 10, width_label = 300;
            const int top_box_begin = top_label_begin, left_box = 320, width_box = 500;
            const int width_clrbt = width_box / 3;
            const int general_spacing = 32;
            const int top_offset_radiogroup = 10, top_offset_radiobutton = 5, left_offset_radiobutton = 20, width_radio = 120;
            const int radio_spacing = 25;

            // Folder dialog 
            const int horz_offset_folderdialog = 200, vert_offset_folderdialog = 10;

            // Confirmation and Exit buttons 
            Size end_button_size = new Size(120, 30);
            const int top_offset_endbutton = 20;
            const int horz_spacing_endbutton = 10;

            // Fonts 
            Font lbl_font = new Font(fontname, 10F, FontStyle.Bold, GraphicsUnit.Point);
            Font box_font = new Font(fontname, 10F, FontStyle.Regular, GraphicsUnit.Point);
            Font rdb_font = new Font(fontname, 8F, FontStyle.Regular, GraphicsUnit.Point);
            Font tab_font = new Font(fontname, 11F, FontStyle.Regular, GraphicsUnit.Point);
            Font ttl_font = new Font(fontname, 20F, FontStyle.Bold, GraphicsUnit.Point);

            // radio_choice_dictionaries:   nested dictionary to save the the dictionary objects (representing the option dictionary in `ApplicationInputParameters`
            //                              the first key refers to the name of the parameter of interest (only parameters with `ApplicationInputParameters.PropertypAndFormType.FormType` = `RadiobuttonGroup`) 
            //                              the second key refers to the alias/description (`prop_alias` in `ApplicationInputParameters.PropertypAndFormType`), which would appear on the text of each RadioButton 
            //                              the final value refers to the actual value to be assigned to the parameter. 
            // for example, if the parameter is `window_type` then: 
            //      >> radio_choice_dictionaries["window_type"] = AppInpPrm.wintype_dict
            Dictionary<string, Dictionary<string, object>> radio_choice_dictionaries = new Dictionary<string, Dictionary<string, object>>();

            // Number of sections like "Input and Output", "General plot options", ... 
            int number_tabs = AppInpPrm.OptionSections.Count;

            // (2) Initialize the dialog and its title + tab components 
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

            // Control list (dictionary) to add the components corresponding to each parameter  
            Dictionary<string, Control> Cntl_List = new Dictionary<string, Control>();

            // RadioButton list to contain all the RadioButton objectes 
            List<RadioButton> Radio_List = new List<RadioButton>();

            // For each section 
            for (int i = 0; i < number_tabs; i++)
            {                
                string section_name = AppInpPrm.OptionSections.Keys.ElementAt(i);
                Dictionary<string, ApplicationInputParameters.PropertypAndFormType> section_options = AppInpPrm.OptionSections[section_name];
                
                // (3) Initialize TabPage for each section 
                TabPages[i] = new TabPage()
                {
                    Location = tabpage_coord, 
                    Size = tabpage_size, 
                    Text = section_name
                };
                
                int number_elements = section_options.Count;
                int top_label = top_box_begin;
                int top_box = top_label_begin;
                string prop_alias, prop_name; 

                // (4) For each element in each section 
                for (int j = 0; j < number_elements; j++)
                {
                    prop_name = section_options.Keys.ElementAt(j);          // like "hostname"
                    prop_alias = section_options[prop_name].prop_alias;     // like "Host name"
                    var prop_val = AppInpPrm.GetPropValue(prop_name);       // the value from the loaded config file 

                    // Add a desciptor label for each parameter 
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

                    // Process the element according to the type (`ApplicationInputParameters.PropertypAndFormType.FormType`)
                    if (section_options[prop_name].IsTextBox())
                    {
                        // Simplest case: Textbox, just add a `TextBox` object 
                        // the value to be changed would be whatever the text in `TextBox.Text`
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
                        // RadiobuttonGroup:
                        // (a) Obtain the corresponding option dictionary  
                        // (b) Add a panel to group the radio buttons
                        // (c) For each option, add a `RadioButton` button object showing the description
                        // (d)Check the button representing the value loaded from the configuration file

                        // (a) Casting a dicionary object to add to `radio_choice_dictionaries`; source: 
                        // https://stackoverflow.com/questions/10206557/c-sharp-cast-dictionarystring-anytype-to-dictionarystring-object-involvin
                        string dict_name = section_options[prop_name].dict_name;
                        IDictionary _dict_ = (IDictionary)AppInpPrm.GetPropValue(dict_name);
                        Dictionary<string, object> dict = _dict_.Cast<dynamic>().ToDictionary(entry => (string)entry.Key, entry => entry.Value);
                        radio_choice_dictionaries.Add(prop_name, dict);

                        // (b) Add a `Panel` object to group the `RadioButton` objects 
                        // because each group of `RadioButton` objects can only have 1 checked button 
                        int radio_group_height = (int)Math.Ceiling(((double)dict.Count) / ((double)max_radio_col)) * radio_spacing + top_offset_radiogroup;
                        Cntl_List.Add(prop_name, new Panel()
                        {
                            Location = new Point(left_box, top_box - top_offset_radiogroup),
                            Size = new Size(width_box, radio_group_height),
                            BorderStyle = BorderStyle.None
                            
                        });
                        top_box += radio_group_height - top_offset_radiogroup - radio_spacing;
                        top_label += radio_group_height - top_offset_radiogroup - radio_spacing; 

                        int default_idx = -1; // the index of the one that records the initial value loaded from the configuration file 
                        int cur_radio_row = 0, cur_radio_col = 0; 
                        for (int i_rdbtr = 0; i_rdbtr < dict.Count; i_rdbtr++)
                        {
                            // (c) For each option, add a `RadioButton` button object showing the description
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
                        // (d) Check the button representing the value loaded from the configuration file 
                        Radio_List[default_idx].Checked = true;
                    }
                    else if (section_options[prop_name].IsColorButton())
                    {
                        // ColorButton: create a `Button` that can prompt a `ColorDialog`
                        // the initial color would be the same as the color loaded from the 
                        // configuration file 
                        Cntl_List.Add(prop_name, new Button()
                        {
                            Location = new Point(left_box, top_box),       
                            Width = width_clrbt,
                            Font = box_font,
                            BackColor = (Color)prop_val,
                            FlatStyle = FlatStyle.Flat,
                        });
                        // Add tooltip 
                        (new ToolTip()).SetToolTip(Cntl_List[prop_name], "Click to select " + prop_alias); 
                        ((Button)Cntl_List[prop_name]).FlatAppearance.BorderSize = 0;
                        Cntl_List[prop_name].Click += (sender, e) =>
                        {
                            // Prompt the color dialog to select a color 
                            // sadly this does not include the transparency factor 
                            // it'd be nice to be able to include that though 
                            ColorDialog colorDialog = new ColorDialog()
                            {
                                AllowFullOpen = true,
                                ShowHelp = false,
                                Color = (sender as Button).BackColor
                            };
                            // Then change the color of the button after choice of color is finalized in the color dialog 
                            if (colorDialog.ShowDialog() == DialogResult.OK)
                            {
                                (sender as Button).BackColor = colorDialog.Color;
                            }
                        };
                    }
                    TabPages[i].Controls.Add(Cntl_List[prop_name]);
                    top_box += general_spacing;

                }
                // Add the `TabPage` to `TabControl` 
                TabCntl.Controls.Add(TabPages[i]);
            }

            // Add the `TabControl` to the main dialog `MainPrompt`  
            MainPrompt.Controls.Add(TabCntl);

            // (4) Add a button to prompt a folder dialog for choosing the output file directory 
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
            
            // (5) Add confirmation and exit button 
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
            
            // (6) Process the changes made after the prompt is closed (to continue) and save configuration set up to a file
            if (MainPrompt.ShowDialog() == DialogResult.OK)
            {
                // Description dictionary for addional descripton of values
                // (also refer to the discussion of configuration file template in the "ApplicationInputParameters.cs" 
                // Key: the name of the parameter
                // Value: 
                //      + [default] <NAN>: meaning there's no additional description here
                //      + if the parameter's value is from an option dictionary, this is one of the 
                //          KEYS (descriptor) in the dictionary (not the VALUES)  
                //      + if this is a color, the method is `Color.ToString()` to get either
                //          the name of the color, or an ARGB array of it   
                Dictionary<string, string> description_dict = new Dictionary<string, string>(); 
                foreach (var item in Cntl_List)
                {
                    string prop_name = item.Key;
                    Control cntl_obj = item.Value;
                    if (cntl_obj is TextBox)
                    {
                        // set new value by `TextBox.Text` 
                        // the parsing is done by reflection in `ApplicationInputParameters`
                        AppInpPrm.SetPropValue(prop_name, cntl_obj.Text);
                        description_dict[prop_name] = "<NAN>"; 
                    }
                    else if (cntl_obj is Panel)
                    {
                        // obtain the checked button and the corresponding option/choice dictionary 
                        // in order to set the parameter's value properly 
                        Dictionary<string, object> prop_dict = radio_choice_dictionaries[prop_name]; 
                        var checkedButton = cntl_obj.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

                        AppInpPrm.SetPropValue(prop_name, prop_dict[checkedButton.Text]);
                        description_dict[prop_name] = checkedButton.Text;
                    }
                    else if (cntl_obj is Button)
                    {
                        // set new color value 
                        AppInpPrm.SetPropValue(prop_name, cntl_obj.BackColor);
                        description_dict[prop_name] = AppInpPrm.GetPropValue(prop_name).ToString();
                    }
                    else // some of these are just labels, hence ignore
                    {
                        continue;
                    }
                }
                AppInpPrm.CompleteInitialize(); // for all the necessary checking and parsing of parameters

                // saving the configuration changes to a file 
                string config_file = Cntl_List["config_path"].Text;
                AppInpPrm.WriteConfigurationFile(config_file, description_dict);
    
            }
            // The modified `AppInpPrm` is returned then assigned to `Result` of `Prompt`
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
