using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ThW.Engine;
using ThW.Input;
using ThW.UI;
using ThW.UI.Utils;
using ThW.XNA.Storage;
using ThW.XNA.Input;
using ThW.XNA.Audio;
using ThW.XNA;

namespace ThW.X360.Controller.Windows
{
    public class SetUpInterface
    {
        public SetUpInterface(X360ControllerGame game, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.game = game;
            this.graphics = graphics;
            this.content = content;

            this.game.TargetElapsedTime = TimeSpan.FromTicks(333333);
            this.game.InactiveSleepTime = TimeSpan.FromSeconds(1);

            this.graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

            Thread.CurrentThread.Name = "Rendering Loop Thread";

            this.game.Window.AllowUserResizing = true;
            this.game.Window.ClientSizeChanged += this.WindowSizeChanged;
            this.graphics.PreparingDeviceSettings += this.WindowSizeChanged;
        }

        private void WindowSizeChanged(object sender, EventArgs e)
        {
            if (null != this.worldEngine)
            {
                this.worldEngine.OnResize(this.game.GraphicsDevice.Viewport.Width, this.game.GraphicsDevice.Viewport.Height);
            }

            if (null != this.statusDesktop)
            {
                this.statusDesktop.SetSize(this.game.GraphicsDevice.Viewport.Width, this.game.GraphicsDevice.Viewport.Height);
            }
        }

        public void Init(Config config)
        {
            this.render = new XNARender2(this.graphics, this.game.GraphicsDevice);
            this.xnaInput = new XNAInput(this.game.Window, this.game.GraphicsDevice, this.game);
            this.xnaAudio = new XNAAudio();
            this.render.Init();

            this.worldEngine = new WorldEngine(new XNAPlatform(this.game, this.graphics), this.render, null);
            this.worldEngine.FilesSystem.AddFilesProvider(new XNAContentStoreProvider(this.game.Content.RootDirectory, null));
            this.worldEngine.Shaders.RegisterTextureLoader(new XNAContentTextureLoader(this.game.Content, this.game.Content.RootDirectory, null));
            this.worldEngine.Init(this.game, this.game.Window.Handle, this.game.Content.RootDirectory, this.xnaInput, this.xnaAudio, "ui/themes/wp7", null);
            this.worldEngine.UIEngine.RegisterWindowType(new OptionsWindowCreator(config, this.graphics), OptionsWindow.TypeName);
            this.worldEngine.UIEngine.RegisterWindowType(new SelectGameWindowCreator(config), SelectGameWindow.TypeName);
            this.worldEngine.MainDesktop.Desktop.DrawCursor = false;
            this.worldEngine.ProgressHandler.ProgressStarted();
            this.worldEngine.ProgressHandler.ProgressMessage("");
            this.worldEngine.HudVisible = false;
            this.worldEngine.ShowFps = true;

            this.game.IsMouseVisible = true;
            this.game.IsFixedTimeStep = false;

            this.worldEngine.UIEngine.RegisterControlsCreator(new BindKeyInputControlsCreator(this.game), BindKeyInput.TypeName, typeof(BindKeyInput));
            this.worldEngine.UIEngine.RegisterPropertyCreator(new IPropertyCreatorTemplate<BindPropertyKey>(), BindPropertyKey.TypeName);

            this.statusDesktop = this.worldEngine.UIEngine.NewDesktop("", "ui/themes/wp7/");
            this.statusWindow = new StatusWindow(this.statusDesktop, this.graphics.GraphicsDevice);
            this.statusDesktop.AddWindow(this.statusWindow);
            this.statusDesktop.DrawCursor = false;
            this.worldEngine.MainDesktop.Desktop.CursorChanged += this.MainDesktopCursorChanged;
            this.worldEngine.ProgressHandler.ProgressFinished();
        }

        private void MainDesktopCursorChanged(Desktop sender, EventArgs data)
        {
#if WINDOWS
            switch (this.worldEngine.MainDesktop.Desktop.CurrentCursor)
            {
                case UI.Controls.MousePointers.PointerHand:
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                    break;
                default:
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
                    break;
            }
#endif
        }

        public void LoadContent()
        {
            this.render.LoadShaders(this.game.Content);

            this.worldEngine.player.Position.X = 90;
            this.worldEngine.player.Position.Y = 7;
            this.worldEngine.player.Position.Z = -17;

            UserActions.temp_playerAngle[0] = 360f;
            UserActions.temp_playerAngle[1] = 8f;
            UserActions.temp_playerAngle[2] = 185.50f;
        }

        public void OnExiting(object sender, EventArgs args)
        {
            this.worldEngine.OnExiting();
        }

        public void Update(GameTime gameTime)
        {
            this.worldEngine.UpdateWorld(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f, gameTime.TotalGameTime.TotalMilliseconds / 1000.0f);

            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Released) && (this.previuosBackState == ButtonState.Pressed))
            {
                this.worldEngine.MainDesktop.OnKeyPress((char)0, Key.Back);
            }

            this.previuosBackState = GamePad.GetState(PlayerIndex.One).Buttons.Back;
        }

        public void Draw(GameTime gameTime)
        {
            if ((this.game.GraphicsDevice.DisplayMode.Width != this.worldEngine.MainDesktop.Desktop.Width) || ((this.game.GraphicsDevice.DisplayMode.Height != this.worldEngine.MainDesktop.Desktop.Height)))
            {
                WindowSizeChanged(null, null);
            }

            this.render.Init3d();
            this.worldEngine.RenderWorld();

            this.render.Init2d();
            this.worldEngine.Render2d();
        }

        public void DrawStatus(GameTime gameTime, String gameName, Bindings bindings, Controller controller)
        {
            this.render.Init3d();
            this.worldEngine.RenderWorld();

            this.render.Init2d();
            this.statusWindow.GameName = gameName;
            this.statusWindow.UpdateState(bindings, controller);
            this.statusDesktop.Render((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }

        public bool InMenu
        {
            get
            {
                return this.worldEngine.MainDesktop.Active;
            }
            set
            {
                this.worldEngine.MainDesktop.Active = value;
            }
        }

        private XNARender2 render = null;
        private GraphicsDeviceManager graphics = null;
        private WorldEngine worldEngine = null;
        private XNAInput xnaInput = null;
        private XNAAudio xnaAudio = null;
        private ButtonState previuosBackState = ButtonState.Released;
        private X360ControllerGame game = null;
        private Desktop statusDesktop = null;
        private StatusWindow statusWindow = null;
        private ContentManager content = null;
    }
}
