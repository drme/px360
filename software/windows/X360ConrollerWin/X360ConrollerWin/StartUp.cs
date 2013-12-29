using System;

namespace ThW.X360.Controller.Windows
{
#if WINDOWS || XBOX
    public static class StartUp
    {
        public static void Main(string[] args)
        {
            using (X360ControllerGame game = new X360ControllerGame())
            {
                game.Run();
            }
        }
    }
#endif
}
