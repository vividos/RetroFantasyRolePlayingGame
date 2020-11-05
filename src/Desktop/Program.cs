using Game.Core;
using Game.Core.Models;
using System;
using System.IO;

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
            using (var game = new TheGame(isTouchEnabledDevice: false, isMobileDevice: false))
            {
                game.SavegameFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    GameData.GameName);

                game.Run();
            }
        }
    }
}
