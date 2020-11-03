using Game.Core.Models;
using System.Diagnostics;

namespace Game.Core.ViewModels
{
    /// <summary>
    /// View model for the "start game" screen
    /// </summary>
    internal class StartGameViewModel
    {
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
        public bool IsJourneyOnwardAvail { get; set; }

        /// <summary>
        /// Starts a new game by creating a new game data object and changing to the ingame
        /// screen.
        /// </summary>
        public void StartNewGame()
        {
            // TODO implement
        }

        /// <summary>
        /// "journeys onward" by loading the currently stored game and changing to the ingame
        /// screen.
        /// </summary>
        public void JourneyOnward()
        {
            Debug.Assert(this.IsJourneyOnwardAvail, "journey onward must be available");

            // TODO implement
        }
    }
}
