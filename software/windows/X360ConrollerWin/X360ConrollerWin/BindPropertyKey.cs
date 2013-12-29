using System;
using Microsoft.Xna.Framework.Input;
using ThW.UI;
using ThW.UI.Controls;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;
using ThW.UI.Utils.Themes;

namespace ThW.X360.Controller.Windows
{
    public class BindPropertyKey : Property
    {
        public BindPropertyKey() : this("", "", "")
        {
        }

        public BindPropertyKey(String name, String group, String text) : base(name, group, text, BindKeyInput.TypeName)
        {
        }

        public override String ToString()
        {
            return this.keyName;
        }

        public override void FromString(String strValue, Theme theme)
        {
            this.keyName = strValue;
        }

        public static String TypeName
        {
            get
            {
                return "bindKey";
            }
        }

        public override object Value
        {
            get
            {
                return this.key;
            }
            set
            {
                bool raiseEvent = (value != this.key);

                this.key = value;

                if (true == raiseEvent)
                {
                    RaiseChangeEvent();
                }
            }
        }

        public Object key = null;
        protected String keyName = "";
    }

    internal class BindKeyInputControlsCreator : IControlsCreator
    {
        public BindKeyInputControlsCreator(X360ControllerGame game)
        {
            this.game = game;
        }

        public Control CreateControl(Window window, CreationFlag creationFlags)
        {
            return new BindKeyInput(this.game, window, creationFlags);
        }

        public bool ShowInDesigner
        {
            get
            {
                return false;
            }
        }

        private X360ControllerGame game = null;
    }

    public class BindKeyInput : Control
    {
        public BindKeyInput(X360ControllerGame game, Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.game = game;
        }

        public override void OnFocus()
        {
            this.game.GetKey(this);

            this.Border = BorderStyle.Lowered;
            this.BackColor = Colors.Red;

            base.OnFocus();
        }

        internal static String TypeName
        {
            get
            {
                return "bindKeyInput";
            }
        }

        internal void SetKey(MouseKey key)
        {
            this.Border = BorderStyle.None;
            this.BackColor = Colors.None;
            this.Value = key;
        }

        internal void SetKey(Keys key)
        {
            this.Border = BorderStyle.None;
            this.BackColor = Colors.None;
            this.Value = key;
        }

        public override object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                bool raiseEvent = (value != this.value);

                this.value = value;

                 if (true == raiseEvent)
                {
                    RaiseValueChangedEvent();
                }
            }
        }

        public override string Text
        {
            get
            {
                if (null != this.value)
                {
                    return this.value.ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
            }
        }

        private X360ControllerGame game = null;
        private object value = null;
    }
}
