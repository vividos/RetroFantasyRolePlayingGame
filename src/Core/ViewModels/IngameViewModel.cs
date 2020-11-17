namespace Game.Core.ViewModels
{
    /// <summary>
    /// View model for the ingame screen.
    /// </summary>
    internal class IngameViewModel
    {
        /// <summary>
        /// View model for the map view
        /// </summary>
        public MapViewViewModel MapViewViewModel { get; }

        /// <summary>
        /// View model for the message scroll
        /// </summary>
        public MessageScrollViewModel MessageScrollViewModel { get; }

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

        /// <summary>
        /// Action type
        /// </summary>
        internal enum ActionType
        {
            /// <summary>
            /// Starts looking at objects; the user has to select an item on the map or in the
            /// inventory.
            /// </summary>
            Look,

            /// <summary>
            /// Starts getting an object at one of the adjacent squares; the user has to select an
            /// object that is gettable.
            /// </summary>
            Get,

            /// <summary>
            /// Starts using an object in the inventory or on the map
            /// </summary>
            Use,

            /// <summary>
            /// Starts attacking an object (usually a mobile) on the map
            /// </summary>
            Attack,

            /// <summary>
            /// Starts talking to an NPC (or object) on the map
            /// </summary>
            Talk,

            /// <summary>
            /// Shows an options menu
            /// </summary>
            Options,
        }

        /// <summary>
        /// Creates a new view model object for the in-game screen
        /// </summary>
        /// <param name="game">game instance</param>
        public IngameViewModel(TheGame game)
        {
            this.MapViewViewModel = new MapViewViewModel(game);
            this.MessageScrollViewModel = new MessageScrollViewModel(11, 20);
        }

        /// <summary>
        /// Performs a user action, e.g. a button action
        /// </summary>
        /// <param name="actionType">action type</param>
        public void Action(ActionType actionType)
        {
            // TODO implement
        }
    }
}
