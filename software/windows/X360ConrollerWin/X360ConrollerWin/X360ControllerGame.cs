using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ThW.X360.Controller.Windows
{
    public enum InputMode
    {
        Game,
        Binding,
        Menu
    }

    public class X360ControllerGame : Microsoft.Xna.Framework.Game
    {
        public X360ControllerGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.config = Config.Load("x360.xonfig.xml");
            this.controller = new Controller(this.config);
            this.service = new RemoteService(this.controller, this.config);
            this.ui = new SetUpInterface(this, this.graphics, this.Content);
        }

        protected override void Initialize()
        {
            this.ui.Init(this.config);

            base.Initialize();

            this.service.Start();

            graphics.IsFullScreen = config.FullScreen;

            if ((config.Width > 0) && (config.Height > 0))
            {
                graphics.PreferredBackBufferWidth = config.Width;
                graphics.PreferredBackBufferHeight = config.Height;
                graphics.ApplyChanges();
            }
        }

        protected override void LoadContent()
        {
            this.ui.LoadContent();

            base.LoadContent();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (this.ui.InMenu == false)
            {
                this.inputMode = InputMode.Game;
            }
            else
            {
                this.escDown = false;
            }

            switch (this.inputMode)
            {
                case InputMode.Binding:
                    this.escDown = false;
                    GetKey();
                    this.IsMouseVisible = false;
                    break;
                case InputMode.Game:
                    this.IsMouseVisible = false;
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.escDown = true;
                        this.state = this.controller.GetState(config.SelectedGame, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
                    }
                    else if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                    {
                        if (true == this.escDown)
                        {
                            this.inputMode = InputMode.Menu;
                            this.ui.InMenu = true;
                        }
                        else
                        {
                            this.escDown = false;
                            this.state = this.controller.GetState(config.SelectedGame, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
                        }
                    }
                    PrintGamepadState();
                    break;
                case InputMode.Menu:
                    this.IsMouseVisible = true;
                    this.ui.Update(gameTime);
                    break;
                default:
                    throw new NotSupportedException();
            }

            base.Update(gameTime);
        }

        protected override void OnExiting(object sender, System.EventArgs args)
        {
            this.service.Stop();

            this.ui.OnExiting(sender, args);

            base.OnExiting(sender, args);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (this.inputMode)
            {
                case InputMode.Game:
                    this.controller.SendState(this.state);
                    this.ui.DrawStatus(gameTime, config.SelectedGame.GameName, config.SelectedGame, this.controller);
                    break;
                case InputMode.Binding:
                case InputMode.Menu:
                    this.ui.Draw(gameTime);
                    break;
                default:
                    throw new NotSupportedException();
            }

            base.Draw(gameTime);
        }

        public void GetKey(BindKeyInput inputKey)
        {
            this.inputKey = inputKey;
            this.inputMode = InputMode.Binding;
        }

        private void GetKey()
        {
            SetPressed(Mouse.GetState().LeftButton, this.lastMouseState.LeftButton, MouseKey.MouseLeft);
            SetPressed(Mouse.GetState().RightButton, this.lastMouseState.RightButton, MouseKey.MouseRight);
            SetPressed(Mouse.GetState().MiddleButton, this.lastMouseState.MiddleButton, MouseKey.MouseMiddle);

            for (int key = 1; key < 255; key++)
            {
                Keys keyCode = (Keys)key;

                if (keyCode != Keys.Escape)
                {
                    if ((true == this.lastKeyboardState.IsKeyDown(keyCode)) && (false == Keyboard.GetState().IsKeyDown(keyCode)))
                    {
                        this.inputMode = InputMode.Menu;
                        this.inputKey.SetKey((Keys)key);
                        this.inputKey = null;
                    }
                }
            }

            this.lastMouseState = Mouse.GetState();
            this.lastKeyboardState = Keyboard.GetState();
        }

        private void SetPressed(ButtonState state, ButtonState lastState, MouseKey key)
        {
            if ((ButtonState.Pressed == lastState) && (ButtonState.Released == state) && (null != this.inputKey))
            {
                this.inputMode = InputMode.Menu;
                this.inputKey.SetKey(key);
                this.inputKey = null;
            }
        }

        private void PrintGamepadState()
        {
            if (true == GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                Debug.WriteLine(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X);
            }
        }

        private Config config = null;
        private GraphicsDeviceManager graphics = null;
        private Controller controller = null;
        private X360State state = null;
        private RemoteService service = null;
        private SetUpInterface ui = null;
        private InputMode inputMode = InputMode.Menu;
        private BindKeyInput inputKey = null;
        private bool escDown = false;
        private MouseState lastMouseState;
        private KeyboardState lastKeyboardState;
    }
}
