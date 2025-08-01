using System;

namespace MyGame
{
    public static class Program
    {
        [STAThread] // Important for some platforms
        static void Main()
        {
            using var game = new Game1();
            game.Run();
        }
    }
}
