using System;

namespace lunge.MapEditor
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
