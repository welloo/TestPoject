using System;

namespace Match3Test
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainFrame())
                game.Run();
        }
    }

}
