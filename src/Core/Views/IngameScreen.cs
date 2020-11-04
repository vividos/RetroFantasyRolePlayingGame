using Game.Core.ViewModels;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Game.Core.Views
{
    /// <summary>
    /// Game screen that shows the in-game views.
    /// </summary>
    internal class IngameScreen : GameScreen
    {
        /// <summary>
        /// View model for the in-game screen
        /// </summary>
        private readonly IngameViewModel viewModel;

        /// <summary>
        /// Creates a new in-game screen object
        /// </summary>
        /// <param name="game">game object</param>
        public IngameScreen(TheGame game)
            : base(game)
        {
            this.viewModel = new IngameViewModel();
        }

        /// <summary>
        /// Loads content for in-game screen
        /// </summary>
        public override void LoadContent()
        {
        }

        /// <summary>
        /// Updates screen state, e.g. from keyboard or gamepad state
        /// </summary>
        /// <param name="gameTime">game time; unused</param>
        public override void Update(GameTime gameTime)
        {
            // TODO implement
        }

        /// <summary>
        /// Draws the in-game screen
        /// </summary>
        /// <param name="gameTime">game time; unused</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO implement
        }
    }
}
