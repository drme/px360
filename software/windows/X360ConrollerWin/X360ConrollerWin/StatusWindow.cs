using System;
using Microsoft.Xna.Framework.Graphics;
using ThW.UI;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.X360.Controller.Windows
{
    public class StatusWindow : Window
    {
        public StatusWindow(Desktop desktop, GraphicsDevice graphics) : base(desktop, CreationFlag.FlagsNone, "g_ui/main_menu/status.window.xml")
        {
            this.gameName = FindControl<Label>("gameTextBox");
            this.statusPanel = FindControl<Panel>("statusPanel");
            this.infoPanel = FindControl<Panel>("infoPanel");
            this.statusButton = FindControl<Button>("statusButton");
            this.graphics = graphics;

            int x = 700;
            int y = 0;
            int dy = 32;

            AddLabel("rt", x, y += dy, X360Keys.RT);
            AddLabel("a", x, y += dy, X360Keys.A, new Color(105,163,25,255));
            AddLabel("b", x, y += dy, X360Keys.B, new Color(186,47,24,255));
            AddLabel("x", x, y += dy, X360Keys.X, new Color(50,77,156,255));
            AddLabel("y", x, y += dy, X360Keys.Y, new Color(181,145,7,255));

            x = 600;
            y = 0;

            AddLabel("r_stick_up", x, y += dy, X360Keys.RSUp);
            AddLabel("r_stick_down", x, y += dy, X360Keys.RSDown);
            AddLabel("r_stick_left", x, y += dy, X360Keys.RSLeft);
            AddLabel("r_stick_right", x, y += dy, X360Keys.RSRight);
            AddLabel("r_stick", x, y += dy, X360Keys.RS);

            x = 500;
            y = 0;

            AddLabel("lb", x, y += dy, X360Keys.LB);
            AddLabel("back", x, y += dy, X360Keys.Back);
            AddLabel("guide", x, y += dy, X360Keys.Guide);
            AddLabel("start", x, y += dy, X360Keys.Start);
            AddLabel("rb", x, y += dy, X360Keys.RB);

            x = 400;
            y = 0;

            AddLabel("l_stick_up", x, y += dy, X360Keys.LSUp);
            AddLabel("l_stick_down", x, y += dy, X360Keys.LSDown);
            AddLabel("l_stick_left", x, y += dy, X360Keys.LSLeft);
            AddLabel("l_stick_right", x, y += dy, X360Keys.LSRight);
            AddLabel("l_stick", x, y += dy, X360Keys.LS);

            x = 300;
            y = 0;

            AddLabel("lt", x, y += dy, X360Keys.LT);
            AddLabel("dpad_up", x, y += dy, X360Keys.Up);
            AddLabel("dpad_down", x, y += dy, X360Keys.Down);
            AddLabel("dpad_left", x, y += dy, X360Keys.Left);
            AddLabel("dpad_right", x, y += dy, X360Keys.Right);

            this.Desktop.Resized += (sender, a) => { this.Bounds = new Rectangle(0, 0, sender.Width, sender.Height); };
        }

        private Label AddLabel(String icon, int x, int y, X360Keys key)
        {
            return AddLabel(icon, x, y, key, Colors.White);
        }

        private Label AddLabel(String icon, int x, int y, X360Keys key, Color color)
        {
            Label label = CreateControl<Label>();

            label.X = x - this.infoPanel.X;
            label.Y = y - this.infoPanel.Y;
            label.Width = 98;
            label.Height = 30;
            label.Icon = "g_ui/icons/" + icon;
            label.IconColor = color;
            label.TextColor = new Color(198, 198, 198);
            label.BackColor = new Color(21, 21, 21);
            label.IconAlignment = ContentAlignment.MiddleLeft;
            label.TextAlignment = ContentAlignment.MiddleCenter;
            label.TextOffset.X = 10;
            label.Tag = key;

            this.Animations.Add(new LinearPropertyAnimation(label.X + 1800, label.X, 0.3 + 0.025 * (x / 100 + 1), (a) => { label.X = a; }));

            this.infoPanel.AddControl(label);

            return label;
        }

        public void UpdateState(Bindings bindings, Controller controller)
        {
            foreach (Bind key in bindings.KeyBindings)
            {
                foreach (Control control in this.infoPanel.Controls)
                {
                    if (key.XboxKey.Equals(control.Tag))
                    {
                        switch (key.Type)
                        {
                            case KeyType.Keyboard:
                                control.Text = key.Key.ToString();
                                break;
                            case KeyType.Mouse:
                                control.Text = key.Mouse.ToString();
                                break;
                            default:
                                throw new NotSupportedException();
                        }

                        break;
                    }
                }
            }

            if (controller.Connected)
            {
                this.statusButton.IconColor = Colors.White;
            }
            else
            {
                this.statusButton.IconColor = Colors.DarkGray;
            }
        }

        public String GameName
        {
            set
            {
                this.gameName.Text = value;
            }
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
            this.statusPanel.Y = this.graphics.Viewport.Height - 73;
            this.statusPanel.Width = this.graphics.Viewport.Width;
            this.infoPanel.X = this.graphics.Viewport.Width - this.infoPanel.Width;

            base.Render(graphics, x, y);
        }

        private GraphicsDevice graphics = null;
        private Control infoPanel = null;
        private Control statusPanel = null;
        private Control gameName = null;
        private Button statusButton = null;
    }
}
