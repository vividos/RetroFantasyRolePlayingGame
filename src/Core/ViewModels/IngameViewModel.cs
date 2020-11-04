namespace Game.Core.ViewModels
{
    /// <summary>
    /// View model for the ingame screen.
    /// </summary>
    internal class IngameViewModel
    {
        /// <summary>
        /// Interaction mode for the ingame view model
        /// </summary>
        internal enum InteractionMode
        {
            /// <summary>
            /// Showing the game's screen
            /// </summary>
            Normal,

            /// <summary>
            /// Showing the inventory screen above the game's screen
            /// </summary>
            Inventory,

            /// <summary>
            /// Showing the stats screen above the game's screen
            /// </summary>
            Stats,
        }
    }
}
