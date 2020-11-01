using Game.Core;
using System;

namespace Game.Desktop
{
    /// <summary>
    /// Program class for desktop game
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method for desktop game
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new TheGame())
            {
                game.Run();
            }
        }
    }
}
