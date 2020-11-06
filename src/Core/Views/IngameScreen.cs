using Game.Core.ViewModels;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
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

            this.InitUserInterface(game);
        }

        /// <summary>
        /// Initializes user interface
        /// </summary>
        /// <param name="game">game instance</param>
        private void InitUserInterface(TheGame game)
        {
            var mapView = new Box
            {
                Width = 480,
                Height = 480,
                BorderThickness = 0,
                FillColor = Color.LightBlue,
            };

            var partyListView = new Box
            {
                Height = 200,
                Width = 320,
                BorderThickness = 0,
                FillColor = Color.DarkOrange,
            };

            var messageScrollView = new Box
            {
                Height = 200,
                Width = 320,
                BorderThickness = 0,
                FillColor = Color.LawnGreen,
            };

            var controlButtonsGrid = new Box
            {
                Height = 80,
                Width = 320,
                BorderThickness = 0,
                FillColor = Color.Pink,
            };

            var leftStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Width = 480,
                BorderThickness = 0,
                BackgroundColor = Color.Red,
                Items =
                {
                    mapView,
                }
            };

            var rightStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Width = 320,
                BorderThickness = 0,
                BackgroundColor = Color.Green,
                Items =
                {
                    partyListView,
                    messageScrollView,
                    controlButtonsGrid,
                }
            };

            // this stack panel splits the screen in two halves, left and right side
            var content = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = game.ScreenWidth,
                Height = game.ScreenHeight,
                Spacing = 0,
                Items =
                {
                    leftStackPanel,
                    rightStackPanel
                }
            };

            game.SetGuiScreenContent(content);
        }

        /// <summary>
        /// Loads content for in-game screen
        /// </summary>
        public override void LoadContent()
        {
            // TODO implement
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
