using System;

namespace GuiTests
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameRoot())
                game.Run();
        }
    }
}
