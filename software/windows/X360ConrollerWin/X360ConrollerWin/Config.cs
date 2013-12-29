using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Xml.Serialization;
using ThW.UI;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.X360.Controller.Windows
{
    public class Config
    {
        public Config()
        {
        }

        private static void Save(Config config, String fileName)
        {
#if WINDOWS
            using (IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User| IsolatedStorageScope.Assembly, null, null))
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName, FileMode.Create, store))
                {
                    XmlSerializer formatter = new XmlSerializer(config.GetType());
                    formatter.Serialize(stream, config);
                }
            }
#endif
        }

        /// <summary>
        /// Saves settings o nthe thead while displaying spinner window.
        /// </summary>
        /// <param name="desktop">desktop to which attch saving window</param>
        /// <param name="config">config to save</param>
        internal static void Save(Desktop desktop, Config config)
        {
            Window window = desktop.NewWindow("g_ui/main_menu/saving.window.xml");

            window.Visible = true;

            Thread thread = new Thread(new ThreadStart(() => 
            {
                Config.Save(config, "x360.xonfig.xml");

                Thread.Sleep(100);

                window.Close(); 
            }));

            thread.Start();
        }

        internal static Config Load(String fileName)
        {
#if WINDOWS
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName, FileMode.Open, store))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Config));
                        return (Config)serializer.Deserialize(stream);
                    }
                }
            }
            catch (Exception)
            {
                Config config = new Config();

                DefaultBindings.AddDefaultBindings(config);

                return config;
            }
#else
            Config config = new Config();

            config.bindings.Add(new Bindings("Default game"));

            return config;
#endif
        }

        public String ArduinoPort
        {
            get
            {
                return this.arduinoPort;
            }
            set
            {
                bool raiseEvent = (value != this.arduinoPort);

                this.arduinoPort = value;

                if ((null != this.ArduinoPortChanged) && (true == raiseEvent))
                {
                    this.ArduinoPortChanged(this, EventArgs.Empty);
                }
            }
        }

        public int ArduinoPortBaud
        {
            get
            {
                return this.arduinoPortBaud;
            }
            set
            {
                bool raiseEvent = (value != this.arduinoPortBaud);

                this.arduinoPortBaud = value;

                if ((null != this.ArduinoPortChanged) && (true == raiseEvent))
                {
                    this.ArduinoPortChanged(this, EventArgs.Empty);
                }
            }
        }

        public int RemoteServerPort
        {
            get
            {
                return this.remoteServerPort;
            }
            set
            {
                bool raiseEvent = (value != this.remoteServerPort);

                this.remoteServerPort = value;

                if ((true == raiseEvent) && (null != this.RemoteServerChanged))
                {
                    this.RemoteServerChanged(this, EventArgs.Empty);
                }
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public bool FullScreen
        {
            get
            {
                return this.fullScreen;
            }
            set
            {
                this.fullScreen = value;
            }
        }

        public String ActiveGame
        {
            get
            {
                return this.activeGame;
            }
            set
            {
                this.activeGame = value;
                this.selectedGame = null;
            }
        }

        public Bindings SelectedGame
        {
            get
            {
                if (null == this.selectedGame)
                {
                    foreach (Bindings bindings in this.bindings)
                    {
                        if (bindings.GameName == this.activeGame)
                        {
                            this.selectedGame = bindings;
                            break;
                        }
                    }
                }

                if (null == this.selectedGame)
                {
                    if (this.bindings.Count == 0)
                    {
                        this.bindings.Add(new Bindings("Default game"));
                    }

                    this.selectedGame = this.bindings[0];
                }

                return this.selectedGame;
            }
        }

        public List<Bindings> Games
        {
            get
            {
                return this.bindings;
            }
        }

        public event UIEventHandler<Config> ArduinoPortChanged = null;
        public event UIEventHandler<Config> RemoteServerChanged = null;
        
        private List<Bindings> bindings = new List<Bindings>();
        private String arduinoPort = "COM20";
        private int arduinoPortBaud = 9600;
        private int remoteServerPort = -1;
        private int width = -1;
        private int height = -1;
        private bool fullScreen = false;
        private String activeGame = null;
        private Bindings selectedGame = null;
    }
}
