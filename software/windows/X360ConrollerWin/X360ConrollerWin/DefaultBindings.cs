using Microsoft.Xna.Framework.Input;

namespace ThW.X360.Controller.Windows
{
    public class DefaultBindings
    {
        public static void AddDefaultBindings(Config config)
        {
            AddDashboard(config);
            AddHalo1(config);
            AddHalo3(config);
            AddHaloODST(config);
            AddHaloReach(config);
            AddTDU2(config);
        }

        private static void AddHalo1(Config config)
        {
            Bindings bindings = new Bindings("Halo Combat Evolved");

            bindings.KeyBindings.Clear();
            bindings.KeyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            bindings.KeyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            bindings.KeyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            bindings.KeyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            bindings.KeyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            bindings.KeyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            bindings.KeyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            bindings.KeyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            bindings.KeyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            bindings.KeyBindings.Add(new Bind(X360Keys.B, Keys.F));
            bindings.KeyBindings.Add(new Bind(X360Keys.X, Keys.E));
            bindings.KeyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            bindings.KeyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            bindings.KeyBindings.Add(new Bind(X360Keys.A, Keys.A));
            bindings.KeyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            bindings.KeyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            bindings.KeyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));

            config.Games.Add(bindings);
        }

        private static void AddHaloODST(Config config)
        {
            Bindings bindings = new Bindings("Halo O. D. S. T");

            bindings.KeyBindings.Clear();
            bindings.KeyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            bindings.KeyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            bindings.KeyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            bindings.KeyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            bindings.KeyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            bindings.KeyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            bindings.KeyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            bindings.KeyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            bindings.KeyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            bindings.KeyBindings.Add(new Bind(X360Keys.B, Keys.F));
            bindings.KeyBindings.Add(new Bind(X360Keys.X, Keys.E));
            bindings.KeyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            bindings.KeyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            bindings.KeyBindings.Add(new Bind(X360Keys.A, Keys.A));
            bindings.KeyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            bindings.KeyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            bindings.KeyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));

            config.Games.Add(bindings);
        }

        private static void AddHaloReach(Config config)
        {
            Bindings bindings = new Bindings("Halo Reach");

            bindings.KeyBindings.Clear();
            bindings.KeyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            bindings.KeyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            bindings.KeyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            bindings.KeyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            bindings.KeyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            bindings.KeyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            bindings.KeyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            bindings.KeyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            bindings.KeyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            bindings.KeyBindings.Add(new Bind(X360Keys.B, Keys.F));
            bindings.KeyBindings.Add(new Bind(X360Keys.X, Keys.E));
            bindings.KeyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            bindings.KeyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            bindings.KeyBindings.Add(new Bind(X360Keys.A, Keys.A));
            bindings.KeyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            bindings.KeyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            bindings.KeyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));

            config.Games.Add(bindings);
        }

        private static void AddHalo3(Config config)
        {
            Bindings bindings = new Bindings("Halo 3");

            bindings.KeyBindings.Clear();
            bindings.KeyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            bindings.KeyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            bindings.KeyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            bindings.KeyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            bindings.KeyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            bindings.KeyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            bindings.KeyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            bindings.KeyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            bindings.KeyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            bindings.KeyBindings.Add(new Bind(X360Keys.B, Keys.F));
            bindings.KeyBindings.Add(new Bind(X360Keys.X, Keys.E));
            bindings.KeyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            bindings.KeyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            bindings.KeyBindings.Add(new Bind(X360Keys.A, Keys.A));
            bindings.KeyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            bindings.KeyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            bindings.KeyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));

            config.Games.Add(bindings);
        }

        private static void AddDashboard(Config config)
        {
            Bindings bindings = new Bindings("Dashboard");

            bindings.KeyBindings.Clear();
            bindings.KeyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            bindings.KeyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            bindings.KeyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            bindings.KeyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            bindings.KeyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            bindings.KeyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            bindings.KeyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            bindings.KeyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            bindings.KeyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            bindings.KeyBindings.Add(new Bind(X360Keys.B, Keys.F));
            bindings.KeyBindings.Add(new Bind(X360Keys.X, Keys.E));
            bindings.KeyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            bindings.KeyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            bindings.KeyBindings.Add(new Bind(X360Keys.A, Keys.A));
            bindings.KeyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            bindings.KeyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            bindings.KeyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));

            config.Games.Add(bindings);
        }

        private static void AddTDU2(Config config)
        {
            Bindings bindings = new Bindings("Test Drive Unlimited 2");

            bindings.KeyBindings.Clear();
            bindings.KeyBindings.Add(new Bind(X360Keys.Guide, Keys.D1));
            bindings.KeyBindings.Add(new Bind(X360Keys.LS, Keys.Space));
            bindings.KeyBindings.Add(new Bind(X360Keys.Back, Keys.Tab));
            bindings.KeyBindings.Add(new Bind(X360Keys.Start, Keys.Escape));
            bindings.KeyBindings.Add(new Bind(X360Keys.Left, Keys.Left));
            bindings.KeyBindings.Add(new Bind(X360Keys.Right, Keys.Right));
            bindings.KeyBindings.Add(new Bind(X360Keys.Up, Keys.Up));
            bindings.KeyBindings.Add(new Bind(X360Keys.Down, Keys.Down));
            bindings.KeyBindings.Add(new Bind(X360Keys.RS, Keys.W));
            bindings.KeyBindings.Add(new Bind(X360Keys.B, Keys.F));
            bindings.KeyBindings.Add(new Bind(X360Keys.X, Keys.E));
            bindings.KeyBindings.Add(new Bind(X360Keys.Y, MouseKey.MouseWheelUp));
            bindings.KeyBindings.Add(new Bind(X360Keys.RB, Keys.LeftAlt));
            bindings.KeyBindings.Add(new Bind(X360Keys.A, Keys.A));
            bindings.KeyBindings.Add(new Bind(X360Keys.LB, Keys.LeftControl));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSLeft, Keys.S));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSRight, Keys.D));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSDown, Keys.X));
            bindings.KeyBindings.Add(new Bind(X360Keys.LT, Keys.G));
            bindings.KeyBindings.Add(new Bind(X360Keys.LSUp, MouseKey.MouseRight));
            bindings.KeyBindings.Add(new Bind(X360Keys.RT, MouseKey.MouseLeft));

            config.Games.Add(bindings);
        }
    }
}
