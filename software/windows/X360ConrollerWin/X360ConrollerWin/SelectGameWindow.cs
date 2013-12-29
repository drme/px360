using System;
using ThW.UI;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.X360.Controller.Windows
{
    /// <summary>
    /// Active game selection window.
    /// </summary>
    public class SelectGameWindow : Window
    {
        /// <summary>
        /// Created game selection window.
        /// </summary>
        /// <param name="desktop">desktop it belongs to</param>
        /// <param name="config">game configuration</param>
        public SelectGameWindow(Desktop desktop, Config config) : base(desktop, CreationFlag.FlagsNone, "g_ui/main_menu/select_game.window.xml")
        {
            this.config = config;

            ScrollPanel gamesList = FindControl<ScrollPanel>("gamesList");

            int y = 4;

            foreach (Bindings binding in this.config.Games)
            {
                if (null != binding)
                {
                    InfoPanel panel = CreateControl<InfoPanel>();

                    panel.BackColor = Colors.None;
                    panel.X = 4;
                    panel.Y = y;
                    panel.Tag = binding;
                    panel.Title = binding.GameName;
                    panel.SubTitle = "XBOX 360";
                    panel.Comment = "";
                    panel.InfoImage = SelectGameImage(binding.GameName);
                    panel.Cursor = MousePointers.PointerHand;
                    panel.Clicked += this.GameSelected;
                    panel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop | AnchorStyle.AnchorRight;

                    gamesList.AddControl(panel);

                    y += 128;
                }
            }
        }

        /// <summary>
        /// Active was selected.
        /// </summary>
        /// <param name="sender">clicked control</param>
        /// <param name="args">arguments</param>
        private void GameSelected(Control sender, EventArgs args)
        {
            this.config.ActiveGame = ((Bindings)sender.Tag).GameName;

            Close();

            Config.Save(this.Desktop, this.config);
        }

        private String SelectGameImage(String gameName)
        {
            gameName = gameName.ToLower();

            String path = "g_ui/games/";

            if (gameName.Contains("halo"))
            {
                if (gameName.Contains("reach"))
                {
                    return path + "halo_reach";
                }
                else if (gameName.Contains("3"))
                {
                    return path + "halo3";
                }
                else if (gameName.Replace(".", "").Contains("odst"))
                {
                    return path + "halo_odst";
                }

                return path + "halo1";
            }
            else if (gameName.Contains("test") && gameName.Contains("drive") && gameName.Contains("unlimited") && gameName.Contains("2"))
            {
                return path + "tdu2";
            }

            return "g_ui/main_menu/x360";
        }

        internal static String TypeName
        {
            get
            {
                return "selectGameWindow";
            }
        }

        private Config config = null;
    }

    internal class SelectGameWindowCreator : IWindowCreatorTemplate<SelectGameWindow>
    {
        public SelectGameWindowCreator(Config config)
        {
            this.config = config;
        }

        public override Window NewWindow(Desktop desktop)
        {
            return new SelectGameWindow(desktop, this.config);
        }

        private Config config = null;
    }
}
