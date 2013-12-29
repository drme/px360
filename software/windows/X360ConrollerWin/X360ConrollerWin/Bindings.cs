using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace ThW.X360.Controller.Windows
{
    /// <summary>
    /// Mouse controls left or right stick.
    /// </summary>
    public enum MouseControl
    {
        LS,
        RS
    }

    /// <summary>
    /// Key bindings.
    /// </summary>
    public class Bindings
    {
        /// <summary>
        /// Constructs key bindings object.
        /// </summary>
        /// <param name="gameName">game name those bindings belongs to.</param>
        public Bindings(String gameName)
        {
            this.GameName = gameName;

            this.keyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            this.keyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            this.keyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            this.keyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            this.keyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            this.keyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            this.keyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            this.keyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            this.keyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            this.keyBindings.Add(new Bind(X360Keys.B, Keys.F));
            this.keyBindings.Add(new Bind(X360Keys.X, Keys.E));
            this.keyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            this.keyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            this.keyBindings.Add(new Bind(X360Keys.A, Keys.A));
            this.keyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            this.keyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            this.keyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            this.keyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            this.keyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            this.keyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            this.keyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));
        }

        public Bindings(Bindings src)
        {
            this.GameName = src.GameName;
            this.InvertMouse = src.InvertMouse;
            this.MouseControl = src.MouseControl;
            this.MouseSensitivity = src.MouseSensitivity;

            foreach (Bind bind in src.KeyBindings)
            {
                this.keyBindings.Add(new Bind(bind));
            }
        }

        public Bindings()
        {
        }

        public List<Bind> KeyBindings
        {
            get
            {
                return this.keyBindings;
            }
        }

        public MouseControl MouseControl
        {
            get
            {
                return this.mouseControl;
            }
            set
            {
                this.mouseControl = value;
            }
        }

        public bool InvertMouse
        {
            get
            {
                return this.invertMouse;
            }
            set
            {
                this.invertMouse = value;
            }
        }

        public float MouseSensitivity
        {
            get
            {
                return this.mouseSensitivity;
            }
            set
            {
                this.mouseSensitivity = value;
            }
        }

        public String GameName
        {
            get
            {
                return this.gameName;
            }
            set
            {
                this.gameName = value;
            }
        }

        private bool invertMouse = false;
        private List<Bind> keyBindings = new List<Bind>();
        private MouseControl mouseControl = MouseControl.RS;
        private float mouseSensitivity = 1.0f;
        private String gameName = "";
    }
}
