using Microsoft.Xna.Framework.Input;

namespace ThW.X360.Controller.Windows
{
    public enum MouseKey
    {
        MouseLeft,
        MouseRight,
        MouseMiddle,
        MouseWheelUp,
        MouseWheelDown
    }

    public enum KeyType
    {
        Mouse,
        Keyboard
    }

    public class Bind
    {
        public Bind()
        {
        }

        public Bind(Bind src)
        {
            this.Key = src.Key;
            this.Mouse = src.Mouse;
            this.XboxKey = src.XboxKey;
            this.Type = src.Type;
        }

        public Bind(X360Keys xboxKey, Keys key)
        {
            this.xboxKey = xboxKey;
            this.key = key;
            this.keyType = KeyType.Keyboard;
        }

        public Bind(X360Keys xboxKey, MouseKey key)
        {
            this.xboxKey = xboxKey;
            this.mouseKey = key;
            this.keyType = KeyType.Mouse;
        }

        public X360Keys XboxKey
        {
            get
            {
                return this.xboxKey;
            }
            set
            {
                this.xboxKey = value;
            }
        }

        public Keys Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        public KeyType Type
        {
            get
            {
                return this.keyType;
            }
            set
            {
                this.keyType = value;
            }
        }

        public MouseKey Mouse
        {
            get
            {
                return this.mouseKey;
            }
            set
            {
                this.mouseKey = value;
            }
        }

        private X360Keys xboxKey;
        private Keys key;
        private MouseKey mouseKey;
        private KeyType keyType;
    }
}
