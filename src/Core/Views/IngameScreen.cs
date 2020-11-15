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

            UniformGrid controlButtonsGrid = this.GetControlButtonsGrid();

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
        /// Gets the uniform grid for the action control buttons
        /// </summary>
        /// <returns>uniform grid</returns>
        private UniformGrid GetControlButtonsGrid()
        {
            var backgroundColor = new Color(0, 128, 255);
            Color textColor = Color.White;

            var lookButton = new Button
            {
                Content = "Look",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = 32,
            };
            lookButton.Clicked += (s, o) => this.viewModel.Action(IngameViewModel.ActionType.Look);

            var getButton = new Button
            {
                Content = "Get",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = 32,
            };
            getButton.Clicked += (s, o) => this.viewModel.Action(IngameViewModel.ActionType.Get);

            var useButton = new Button
            {
                Content = "Use",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = 32,
            };
            useButton.Clicked += (s, o) => this.viewModel.Action(IngameViewModel.ActionType.Use);

            var attackButton = new Button
            {
                Content = "Attack",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = 32,
            };
            attackButton.Clicked += (s, o) => this.viewModel.Action(IngameViewModel.ActionType.Attack);

            var talkButton = new Button
            {
                Content = "Talk",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = 32,
            };
            talkButton.Clicked += (s, o) => this.viewModel.Action(IngameViewModel.ActionType.Talk);

            var optionsButton = new Button
            {
                Content = "Options",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = 50,
            };
            optionsButton.Clicked += (s, o) => this.viewModel.Action(IngameViewModel.ActionType.Options);

            return new UniformGrid
            {
                Height = 80,
                Width = 320,
                BorderThickness = 0,
                Columns = 3,
                Rows = 2,
                Items =
                {
                    lookButton,
                    getButton,
                    useButton,
                    attackButton,
                    talkButton,
                    optionsButton
                }
            };
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
