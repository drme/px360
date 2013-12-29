using System;
using System.Collections.Generic;
#if WINDOWS
using System.IO.Ports;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ThW.UI;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;
using ThW.UI.Design;

namespace ThW.X360.Controller.Windows
{
    public class OptionsWindow : Window
    {
        public OptionsWindow(Desktop desktop, Config config, GraphicsDeviceManager graphics) : base(desktop, CreationFlag.FlagsNone, "g_ui/main_menu/options.window.xml")
        {
            this.config = config;
            this.graphics = graphics;

            this.resolutionsList = FindControl<ComboBox>("cbRes");
            this.fullScreen = FindControl<ComboBox>("cbWindowMode");
            this.listBox = FindControl<ListBox>("games");
            this.gamesListComboBox = FindControl<ComboBox>("gamesList");
            this.gameName = FindControl<TextBox>("gameName");

            //FindControl<TabPage>("tabBindings").IconPicture = this.Engine.GetIcon(20, true, null);
            //FindControl<TabPage>("tabMouse").IconPicture = this.Engine.GetIcon(14, true, null);
            //FindControl<TabPage>("tabVideo").IconPicture = this.Engine.GetIcon(17, true, null);

            FindControl<Button>("addGame").Clicked += this.AddGameClicked;
            FindControl<Button>("removeGame").Clicked += this.RemoveGameClicked;

            this.gamesListComboBox.SelectedItemChanged += this.GameChanged;
            this.listBox.SelectedItemChanged += this.SelectedGameChanged;
            this.gameName.TextChanged += this.GameNameTextChanged;

            FillVideoModes();
            FillKeys(this.config.SelectedGame);
            FillPorts();
            FillGames();
            FillRemoteData();
            RegisterKeyEvents();
        }

        private void RegisterKeyEvents()
        {
            RegisterValueChanged("a");
            RegisterValueChanged("b");
            RegisterValueChanged("x");
            RegisterValueChanged("y");
            RegisterValueChanged("guide");
            RegisterValueChanged("back");
            RegisterValueChanged("start");
            RegisterValueChanged("rb");
            RegisterValueChanged("lb");
            RegisterValueChanged("rt");
            RegisterValueChanged("lt");
            RegisterValueChanged("d_up");
            RegisterValueChanged("d_down");
            RegisterValueChanged("d_left");
            RegisterValueChanged("d_right");
            RegisterValueChanged("ls_up");
            RegisterValueChanged("ls_down");
            RegisterValueChanged("ls_left");
            RegisterValueChanged("ls_right");
            RegisterValueChanged("ls_press");
            RegisterValueChanged("rs_up");
            RegisterValueChanged("rs_down");
            RegisterValueChanged("rs_left");
            RegisterValueChanged("rs_right");
            RegisterValueChanged("rs_press");
        }

        private void RegisterValueChanged(String name)
        {
            PropertyRow row = FindControl<PropertyRow>(name);

            if (null != row)
            {
                row.Property.ValueChanged += this.BindPropertyValueChanged;
            }
        }

        private void BindPropertyValueChanged(Property sender, EventArgs args)
        {
            ClearValue(sender, "a");
            ClearValue(sender, "b");
            ClearValue(sender, "x");
            ClearValue(sender, "y");
            ClearValue(sender, "guide");
            ClearValue(sender, "back");
            ClearValue(sender, "start");
            ClearValue(sender, "rb");
            ClearValue(sender, "lb");
            ClearValue(sender, "rt");
            ClearValue(sender, "lt");
            ClearValue(sender, "d_up");
            ClearValue(sender, "d_down");
            ClearValue(sender, "d_left");
            ClearValue(sender, "d_right");
            ClearValue(sender, "ls_up");
            ClearValue(sender, "ls_down");
            ClearValue(sender, "ls_left");
            ClearValue(sender, "ls_right");
            ClearValue(sender, "ls_press");
            ClearValue(sender, "rs_up");
            ClearValue(sender, "rs_down");
            ClearValue(sender, "rs_left");
            ClearValue(sender, "rs_right");
            ClearValue(sender, "rs_press");
        }

        private void ClearValue(Property sender, String name)
        {
            PropertyRow row = FindControl<PropertyRow>(name);

            if ((null != row) && (row.Property != sender) && (null != sender.Value) && (null != row.Property.Value) && (sender.Value.ToString() == row.Property.Value.ToString()))
            {
                row.Property.Value = null;
            }
        }

        private void GameNameTextChanged(TextBox sender, EventArgs e)
        {
            if (null != this.listBox.SelectedItem)
            {
                this.listBox.SelectedItem.Text = this.gameName.Text;

                foreach (ListItem item in this.listBox.Items)
                {
                    if ((item.Tag == this.editableGame) && (null != this.editableGame))
                    {
                        ((Bindings)item.Tag).GameName = this.gameName.Text;
                    }
                }

                foreach (ComboBoxItem item in this.gamesListComboBox.Items)
                {
                    if (item.Tag == this.editableGame)
                    {
                        item.Text = this.gameName.Text;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Game was selected in the games edit list.
        /// </summary>
        private void SelectedGameChanged(ListBox sender, EventArgs e)
        {
            if (null != this.listBox.SelectedItem)
            {
                this.gameName.Text = this.listBox.SelectedItem.Text;
                this.editableGame = (Bindings)this.listBox.SelectedItem.Tag;
            }
        }

        private void GameChanged(ComboBox sender, EventArgs e)
        {
            UpdateBindings((Bindings)this.lastGame);

            if (null != this.gamesListComboBox.SelectedItem)
            {
                this.lastGame = (Bindings)this.gamesListComboBox.SelectedItem.Tag;

                FillKeys(this.lastGame);
            }
        }

        /// <summary>
        /// Removes game from the list
        /// </summary>
        private void RemoveGameClicked(Button sender, EventArgs e)
        {
            if (null != this.listBox.SelectedItem)
            {
                this.listBox.RemoveControl(this.listBox.SelectedItem);

                foreach (ComboBoxItem item in this.gamesListComboBox.Items)
                {
                    if (item.Tag == this.listBox.SelectedItem.Tag)
                    {
                        this.gamesListComboBox.RemoveItem(item);

                        break;
                    }
                }

                this.editableGame = null;
                this.gameName.Text = "";
            }
        }

        /// <summary>
        /// Adding new game
        /// </summary>
        private void AddGameClicked(Button sender, EventArgs e)
        {
            Bindings game = new Bindings("new game");

            ListItem gameItem = CreateControl<ListItem>();

            gameItem.Text = game.GameName;
            gameItem.TextAlignment = ContentAlignment.MiddleLeft;
            gameItem.Tag = game;

            this.listBox.AddControl(gameItem);
            this.listBox.SelectedItem = gameItem;

            this.gamesListComboBox.AddItem(game.GameName, game.GameName, null, game);

            this.editableGame = game;
        }

        /// <summary>
        /// Closes window and saves settings.
        /// </summary>
        public override void Close()
        {
            if (DialogResult.DialogResultOK == this.DialogResult)
            {
                UpdateVideo();

                if (null != this.gamesListComboBox.SelectedItem)
                {
                    UpdateBindings((Bindings)this.gamesListComboBox.SelectedItem.Tag);
                }

                this.config.Games.Clear();

                foreach (ListItem item in this.listBox.Controls)
                {
                    this.config.Games.Add((Bindings)item.Tag);
                }

                this.config.ActiveGame = ((Bindings)this.gamesListComboBox.SelectedItem.Tag).GameName;

                ComboBox arduinoPortComboBox = FindControl<ComboBox>("arduinoPort");

                if (null != arduinoPortComboBox.SelectedItem)
                {
                    this.config.ArduinoPort = arduinoPortComboBox.SelectedItem.Text;
                }

                CheckBox useRemoteServer = FindControl<CheckBox>("remoteServer");
                TextBox remoteServerport = FindControl<TextBox>("remotePort");

                if (true == useRemoteServer.Checked)
                {
                    try
                    {
                        this.config.RemoteServerPort = int.Parse(remoteServerport.Text);
                    }
                    catch (Exception)
                    {
                        this.config.RemoteServerPort = 9999;
                    }
                }
                else
                {
                    this.config.RemoteServerPort = -1;
                }

                Config.Save(this.Desktop, this.config);
            }

            base.Close();
        }

        /// <summary>
        /// Transfers binding data from interface to the config object.
        /// </summary>
        /// <param name="bindings">bindings object to save config</param>
        private void UpdateBindings(Bindings bindings)
        {
            if (null != bindings)
            {
                bindings.KeyBindings.Clear();

                UpdateValue("a", X360Keys.A, bindings);
                UpdateValue("b", X360Keys.B, bindings);
                UpdateValue("x", X360Keys.X, bindings);
                UpdateValue("y", X360Keys.Y, bindings);

                UpdateValue("guide", X360Keys.Guide, bindings);
                UpdateValue("back", X360Keys.Back, bindings);
                UpdateValue("start", X360Keys.Start, bindings);
                UpdateValue("rb", X360Keys.RB, bindings);
                UpdateValue("lb", X360Keys.LB, bindings);
                UpdateValue("rt", X360Keys.RT, bindings);
                UpdateValue("lt", X360Keys.LT, bindings);
                UpdateValue("d_up", X360Keys.Up, bindings);
                UpdateValue("d_down", X360Keys.Down, bindings);
                UpdateValue("d_left", X360Keys.Left, bindings);
                UpdateValue("d_right", X360Keys.Right, bindings);
                UpdateValue("ls_up", X360Keys.LSUp, bindings);
                UpdateValue("ls_down", X360Keys.LSDown, bindings);
                UpdateValue("ls_left", X360Keys.LSLeft, bindings);
                UpdateValue("ls_right", X360Keys.LSRight, bindings);
                UpdateValue("ls_press", X360Keys.LS, bindings);
                UpdateValue("rs_up", X360Keys.RSUp, bindings);
                UpdateValue("rs_down", X360Keys.RSDown, bindings);
                UpdateValue("rs_left", X360Keys.RSLeft, bindings);
                UpdateValue("rs_right", X360Keys.RSRight, bindings);
                UpdateValue("rs_press", X360Keys.RS, bindings);

                if (true == FindControl<RadioButton>("ms_ls").Selected)
                {
                    bindings.MouseControl = MouseControl.LS;
                }
                else
                {
                    bindings.MouseControl = MouseControl.RS;
                }

                bindings.MouseSensitivity = FindControl<TrackBar>("mouseSensitivity").Position;
                bindings.InvertMouse = FindControl<CheckBox>("invertMouse").Checked;
            }
        }

        /// <summary>
        /// Transfers one key binding from ui element to config objeect.
        /// </summary>
        /// <param name="name">ui component name to get value of</param>
        /// <param name="key">xbox 360 contoller key</param>
        /// <param name="values">confing object to store values to</param>
        private void UpdateValue(String name, X360Keys key, Bindings values)
        {
            PropertyRow row = FindControl<PropertyRow>(name);

            row.UpdateValue(false);

            if (null != row)
            {
                if (row.Property.Value is MouseKey)
                {
                    values.KeyBindings.Add(new Bind((X360Keys)row.Tag, (MouseKey)row.Property.Value));
                }
                else if (row.Property.Value is Keys)
                {
                    values.KeyBindings.Add(new Bind((X360Keys)row.Tag, (Keys)row.Property.Value));
                }
            }
        }

        /// <summary>
        /// Changes display settings and updates config object.
        /// </summary>
        private void UpdateVideo()
        {
            if ((null != this.resolutionsList.SelectedItem) && (null != this.fullScreen.SelectedItem))
            {
                DisplayMode mode = (DisplayMode)this.resolutionsList.SelectedItem.Tag;

                if (this.fullScreen.SelectedItem.Name == "cbiFull")
                {
                    this.graphics.IsFullScreen = true;

                    this.graphics.PreferredBackBufferWidth = mode.Width;
                    this.graphics.PreferredBackBufferHeight = mode.Height;

                    this.config.Width = mode.Width;
                    this.config.Height = mode.Height;
                }
                else
                {
                    this.graphics.IsFullScreen = false;
                }

                this.graphics.ApplyChanges();

                this.config.FullScreen = this.graphics.IsFullScreen;
            }
        }

        /// <summary>
        /// Fills available video modes.
        /// </summary>
        private void FillVideoModes()
        {
            if (null != this.resolutionsList)
            {
                this.resolutionsList.ClearItems();

                ComboBoxItem activeMode = null;

                List<String> added = new List<String>();

                foreach (var mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    String modeName = mode.Width + "x" + mode.Height;

                    if (false == added.Contains(modeName))
                    {
                        ComboBoxItem item = this.resolutionsList.AddItem(modeName, modeName, null, new DisplayMode(mode.Width, mode.Height));

                        if ((mode.Width == this.graphics.PreferredBackBufferWidth) && (mode.Height == this.graphics.PreferredBackBufferHeight))
                        {
                            activeMode = item;
                        }

                        added.Add(modeName);
                    }
                }

                this.resolutionsList.SelectedItem = activeMode;

                if (null == activeMode)
                {
                    DisplayMode mode = new DisplayMode(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight);

                    String modeName = mode.Width + "x" + mode.Height;

                    this.resolutionsList.SelectedItem = this.resolutionsList.AddItem(modeName, modeName, null, mode);
                }

                if (null != this.fullScreen)
                {
                    if (true == this.graphics.IsFullScreen)
                    {
                        this.fullScreen.SelectedItemIndex = 0;
                    }
                    else
                    {
                        this.fullScreen.SelectedItemIndex = 1;
                    }
                }
            }
        }

        private void FillKeys(Bindings bindings)
        {
            SetValue("a", X360Keys.A, bindings);
            SetValue("b", X360Keys.B, bindings);
            SetValue("x", X360Keys.X, bindings);
            SetValue("y", X360Keys.Y, bindings);

            SetValue("guide", X360Keys.Guide, bindings);
            SetValue("back", X360Keys.Back, bindings);
            SetValue("start", X360Keys.Start, bindings);
            SetValue("rb", X360Keys.RB, bindings);
            SetValue("lb", X360Keys.LB, bindings);
            SetValue("rt", X360Keys.RT, bindings);
            SetValue("lt", X360Keys.LT, bindings);
            SetValue("d_up", X360Keys.Up, bindings);
            SetValue("d_down", X360Keys.Down, bindings);
            SetValue("d_left", X360Keys.Left, bindings);
            SetValue("d_right", X360Keys.Right, bindings);
            SetValue("ls_up", X360Keys.LSUp, bindings);
            SetValue("ls_down", X360Keys.LSDown, bindings);
            SetValue("ls_left", X360Keys.LSLeft, bindings);
            SetValue("ls_right", X360Keys.LSRight, bindings);
            SetValue("ls_press", X360Keys.LS, bindings);
            SetValue("rs_up", X360Keys.RSUp, bindings);
            SetValue("rs_down", X360Keys.RSDown, bindings);
            SetValue("rs_left", X360Keys.RSLeft, bindings);
            SetValue("rs_right", X360Keys.RSRight, bindings);
            SetValue("rs_press", X360Keys.RS, bindings);

            if (MouseControl.LS == bindings.MouseControl)
            {
                FindControl<RadioButton>("ms_ls").Selected = true;
                FindControl<RadioButton>("ms_rs").Selected = false;
            }
            else
            {
                FindControl<RadioButton>("ms_ls").Selected = false;
                FindControl<RadioButton>("ms_rs").Selected = true;
            }

            FindControl<TrackBar>("mouseSensitivity").Position = bindings.MouseSensitivity;
            FindControl<CheckBox>("invertMouse").Checked = bindings.InvertMouse;
        }

        /// <summary>
        /// Fills serial ports and selects the active one, that is used for communcating with arduino
        /// </summary>
        private void FillPorts()
        {
#if WINDOWS
            ComboBox comboBox = FindControl<ComboBox>("arduinoPort");

            foreach (String portName in SerialPort.GetPortNames())
            {
                ComboBoxItem item = comboBox.AddItem(portName, portName, null, null);

                if (portName == this.config.ArduinoPort)
                {
                    comboBox.SelectedItem = item;
                }
            }
#endif
        }

        /// <summary>
        /// Sets remote connection settings.
        /// </summary>
        private void FillRemoteData()
        {
            CheckBox useRemoteServer = FindControl<CheckBox>("remoteServer");
            TextBox remoteServerport = FindControl<TextBox>("remotePort");

            if (this.config.RemoteServerPort > 0)
            {
                useRemoteServer.Checked = true;
                remoteServerport.Text = this.config.RemoteServerPort.ToString();
            }
            else
            {
                useRemoteServer.Checked = false;
                remoteServerport.Text = "0";
            }
        }

        /// <summary>
        /// Fils games lists
        /// </summary>
        private void FillGames()
        {
            foreach (Bindings it in this.config.Games)
            {
                Bindings game = new Bindings(it);

                ListItem gameItem = CreateControl<ListItem>();

                gameItem.Text = game.GameName;
                gameItem.TextAlignment = ContentAlignment.MiddleLeft;
                gameItem.Tag = game;

                this.listBox.AddControl(gameItem);

                ComboBoxItem item = this.gamesListComboBox.AddItem(game.GameName, game.GameName, null, game);

                if (this.config.ActiveGame == game.GameName)
                {
                    this.gamesListComboBox.SelectedItem = item;
                }
            }

            this.listBox.ListStyle = ListStyle.Details;
        }

        private void SetValue(String name, X360Keys key, Bindings values)
        {
            PropertyRow row = FindControl<PropertyRow>(name);

            if (null != row)
            {
                row.Tag = key;

                foreach (Bind binding in values.KeyBindings)
                {
                    if (binding.XboxKey == key)
                    {
                        switch (binding.Type)
                        {
                            case KeyType.Keyboard:
                                row.Property.FromString(binding.Key.ToString(), this.Window.Desktop.Theme);
                                row.Property.Value = binding.Key;
                                break;
                            case KeyType.Mouse:
                                row.Property.FromString(binding.Mouse.ToString(), this.Window.Desktop.Theme);
                                row.Property.Value = binding.Mouse;
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Control name as serialized in xml
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "xOptionsWindow";
            }
        }

        private ListBox listBox = null;
        private ComboBox gamesListComboBox = null;
        private ComboBox resolutionsList = null;
        private ComboBox fullScreen = null;
        private TextBox gameName = null;
        private Bindings lastGame = null;
        private Config config = null;
        private GraphicsDeviceManager graphics = null;
        private Bindings editableGame = null;
    }

    internal class OptionsWindowCreator : IWindowCreatorTemplate<OptionsWindow>
    {
        public OptionsWindowCreator(Config config, GraphicsDeviceManager graphics)
        {
            this.config = config;
            this.graphics = graphics;
        }

        public override Window NewWindow(Desktop desktop)
        {
            return new OptionsWindow(desktop, this.config, this.graphics);
        }

        private Config config = null;
        private GraphicsDeviceManager graphics = null;
    }

    internal class DisplayMode
    {
        public DisplayMode(int w, int h)
        {
            this.width = w;
            this.height = h;
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        private int width;
        private int height;
    }
}
