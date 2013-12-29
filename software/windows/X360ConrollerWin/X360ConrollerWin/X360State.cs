using System;
using Microsoft.Xna.Framework.Input;

namespace ThW.X360.Controller.Windows
{
    public class X360State
    {
        public X360State(Config config)
        {
            this.config = config;
        }

        public X360State(String args)
        {
            this.keyState = X360Keys.None;
            this.lsVertical = 127;
            this.rsVertical = 127;
            this.lsHorizontal = 127;
            this.rsHorizontal = 127;
            this.rt = 0;
            this.lt = 0;

            String[] d = args.Split('&');

            foreach (string st in d)
            {
                if (st.StartsWith("rt="))
                {
                    this.rt = (byte)int.Parse(st.Split('=')[1].Trim());
                }
                else if (st.StartsWith("rsx="))
                {
                    this.rsHorizontal = (byte)int.Parse(st.Split('=')[1].Trim());
                }
                else if (st.StartsWith("rsy="))
                {
                    this.rsVertical = (byte)int.Parse(st.Split('=')[1].Trim());
                }
                else if (st.StartsWith("lt="))
                {
                    this.lt = (byte)int.Parse(st.Split('=')[1].Trim());
                }
                else if (st.StartsWith("lsx="))
                {
                    this.lsHorizontal = (byte)int.Parse(st.Split('=')[1].Trim());
                }
                else if (st.StartsWith("lsy="))
                {
                    this.lsVertical = (byte)int.Parse(st.Split('=')[1].Trim());
                }
            }
        }

        public void UpdateState(Bindings bindings, int w, int h)
        {
            this.keyState = X360Keys.None;
            this.lsVertical = 127;
            this.rsVertical = 127;
            this.lsHorizontal = 127;
            this.rsHorizontal = 127;
            this.rt = 0;
            this.lt = 0;

            if (this.prevX > 0)
            {
                int dx = Mouse.GetState().X - this.prevX;
                int dy = Mouse.GetState().Y - this.prevY;

                if (true == bindings.InvertMouse)
                {
                    dy *= -1;
                }

                switch (bindings.MouseControl)
                {
                    case MouseControl.LS:
                        this.lsVertical = ConvertMouse(dy);
                        this.lsHorizontal = ConvertMouse(dx);
                        break;
                    case MouseControl.RS:
                        this.rsVertical = ConvertMouse(dy);
                        this.rsHorizontal = ConvertMouse(dx);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            this.prevX = w / 2;
            this.prevY = h / 2;
            this.prevW = Mouse.GetState().ScrollWheelValue;
            Mouse.SetPosition(prevX, prevY);

            foreach (Bind binding in bindings.KeyBindings)
            {
                switch (binding.XboxKey)
                {
                    case X360Keys.LSLeft:
                        Map(binding, ref this.lsHorizontal, 255);
                        break;
                    case X360Keys.LSRight:
                        Map(binding, ref this.lsHorizontal, 0);
                        break;
                    case X360Keys.LSDown:
                        Map(binding, ref this.lsVertical, 0);
                        break;
                    case X360Keys.LSUp:
                        Map(binding, ref this.lsVertical, 255);
                        break;
                    case X360Keys.RSLeft:
                        Map(binding, ref this.rsHorizontal, 255);
                        break;
                    case X360Keys.RSRight:
                        Map(binding, ref this.rsHorizontal, 0);
                        break;
                    case X360Keys.RSDown:
                        Map(binding, ref this.rsVertical, 0);
                        break;
                    case X360Keys.RSUp:
                        Map(binding, ref this.rsVertical, 255);
                        break;
                    case X360Keys.LT:
                        Map(binding, ref this.lt, 255);
                        break;
                    case X360Keys.RT:
                        Map(binding, ref this.rt, 255);
                        break;
                    default:
                        Map(binding, binding.XboxKey);
                        break;
                }
            }
        }

        public byte[] GetState()
        {
            byte[] data = new byte[11];

            data[0] = this.lsVertical;
            data[1] = this.lsHorizontal;
            data[2] = this.rsVertical;
            data[3] = this.rsHorizontal;
            data[4] = this.rt;
            data[5] = this.lt;

            byte[] keys = BitConverter.GetBytes((int)this.keyState);

            data[6] = keys[0];
            data[7] = keys[1];
            data[8] = keys[2];
            data[9] = keys[3];

            data[10] = 0xFF;

         //   System.Diagnostics.Debug.WriteLine("FF: " + data[0] + " " + data[1] + " " + data[2] + " " + data[3]);

            return data;
        }

        private void Map(Bind binding, X360Keys xkey)
        {
            switch (binding.Type)
            {
                case KeyType.Mouse:
                    Map(binding.Mouse, xkey);
                    break;
                case KeyType.Keyboard:
                    Map(binding.Key, xkey);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void Map(Bind binding, ref byte stick, byte value)
        {
            switch (binding.Type)
            {
                case KeyType.Mouse:
                    Map(binding.Mouse, ref stick, value);
                    break;
                case KeyType.Keyboard:
                    Map(binding.Key, ref stick, value);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void Map(MouseKey key, X360Keys xkey)
        {
            switch (key)
            {
                case MouseKey.MouseLeft:
                    Map(Mouse.GetState().LeftButton, xkey);
                    break;
                case MouseKey.MouseRight:
                    Map(Mouse.GetState().RightButton, xkey);
                    break;
                case MouseKey.MouseMiddle:
                    Map(Mouse.GetState().MiddleButton, xkey);
                    break;
                case MouseKey.MouseWheelUp:
                    Map((Mouse.GetState().ScrollWheelValue - prevW> 0) ? ButtonState.Pressed : ButtonState.Released, xkey);
                    break;
                case MouseKey.MouseWheelDown:
                    Map((Mouse.GetState().ScrollWheelValue - prevW < 0) ? ButtonState.Pressed : ButtonState.Released, xkey);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void Map(MouseKey key, ref byte stick, byte value)
        {
            switch (key)
            {
                case MouseKey.MouseLeft:
                    Map(Mouse.GetState().LeftButton, ref stick, value);
                    break;
                case MouseKey.MouseRight:
                    Map(Mouse.GetState().RightButton, ref stick, value);
                    break;
                case MouseKey.MouseMiddle:
                    Map(Mouse.GetState().MiddleButton, ref stick, value);
                    break;
                case MouseKey.MouseWheelUp:
                    Map((Mouse.GetState().ScrollWheelValue - prevW > 0) ? ButtonState.Pressed : ButtonState.Released, ref stick, value);
                    break;
                case MouseKey.MouseWheelDown:
                    Map((Mouse.GetState().ScrollWheelValue - prevW < 0) ? ButtonState.Pressed : ButtonState.Released, ref stick, value);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void Map(Keys key, X360Keys xkey)
        {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                this.keyState |= xkey;
            }
        }

        private void Map(ButtonState state, X360Keys xkey)
        {
            if (ButtonState.Pressed == state)
            {
                this.keyState |= xkey;
            }
        }

        private void Map(Keys key, ref byte stick, byte value)
        {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                stick = value;
            }
        }

        private void Map(ButtonState state, ref byte stick, byte value)
        {
            if (ButtonState.Pressed == state)
            {
                stick = value;
            }
        }

        private byte Convert(int da)
        {
            if (da < 0)
            {
                /* int result = 127 + da;

                if (result < 0)
                {
                    return 0;
                }
                else
                {
                    return (byte)result;
                } */

                return 255;
            }
            else if (da > 0)
            {
                /* int result = 127 + da;

                if (result > 255)
                {
                    return 255;
                }
                else
                {
                    return (byte)result;
                } */

                return 0;
            }

            return 127;
        }

        private byte ConvertMouse(int da)
        {
            int dz = 30;
            int sens = (int)(10.0f * this.config.SelectedGame.MouseSensitivity);
            int center = 127;

            if (da < 0)
            {
                int result = center - da * sens;

                if (result > 255)
                {
                    return 255;
                }
                else
                {
                    if (result < center + dz)
                    {
                        return (byte)(center + dz);
                    }
                    else
                    {
                        return (byte)result;
                    }
                }

                return 255;
            }
            else if (da > 0)
            {
                int result = center - da * sens;

                if (result < 0)
                {
                    return 0;
                }
                else
                {
                    if (result > center - dz)
                    {
                        return (byte)(center - dz);
                    }
                    else
                    {
                        return (byte)result;
                    }
                }

                return 0;
            }

            return 127;
        }

        public override string ToString()
        {
            return keyState + " " + lsVertical + " " + lsHorizontal;
        }

        private X360Keys keyState = X360Keys.None;
        private byte lsVertical = 127;
        private byte lsHorizontal = 127;
        private byte rsVertical = 127;
        private byte rsHorizontal = 127;
        private byte rt = 0;
        private byte lt = 0;
        private int prevX = -1;
        private int prevY = -1;
        private int prevW = 0;
        private Config config = null;
    }
}
