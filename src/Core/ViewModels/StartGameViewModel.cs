using Game.Core.Models;
using System.Diagnostics;
using System.IO;

namespace Game.Core.ViewModels
{
    /// <summary>
    /// View model for the "start game" screen
    /// </summary>
    internal class StartGameViewModel
    {
        /// <summary>
        /// Game instance
        /// </summary>
        private readonly TheGame game;

        /// <summary>
        /// Filename of the save game
        /// </summary>
        private readonly string savegameFilename;

        /// <summary>
        /// The game's title
        /// </summary>
        public string GameTitle => GameData.GameName;

        /// <summary>
        /// The game's subtitle, or empty when no subtitle exists
        /// </summary>
        public string GameSubtitle => GameData.GameSubtitle;

        /// <summary>
        /// Indicates if "journey onward" button is available
        /// </summary>
        public bool IsJourneyOnwardAvail { get; private set; }

        /// <summary>
        /// Creates a new view model object
        /// </summary>
        /// <param name="game">game object</param>
        public StartGameViewModel(TheGame game)
        {
            this.game = game;

            this.savegameFilename = Path.Combine(game.SavegameFolder, GameData.SavegameFilename);

            this.IsJourneyOnwardAvail = File.Exists(this.savegameFilename);
        }

        /// <summary>
        /// Starts a new game by creating a new game data object and changing to the ingame
        /// screen.
        /// </summary>
        public void StartNewGame()
        {
            this.game.CurrentGameData = GameData.Create();

            this.game.NavigateToScreen(GameScreenType.IngameScreen);
        }

        /// <summary>
        /// "journeys onward" by loading the currently stored game and changing to the ingame
        /// screen.
        /// </summary>
        public void JourneyOnward()
        {
            Debug.Assert(this.IsJourneyOnwardAvail, "journey onward must be available");

            using (var stream = new FileStream(this.savegameFilename, FileMode.Open))
            {
                this.game.CurrentGameData = GameData.Load(stream);
            }

            this.game.NavigateToScreen(GameScreenType.IngameScreen);
        }
    }
}
