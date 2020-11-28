using Game.Core.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        /// Keyboard state of last Update call
        /// </summary>
        private KeyboardState lastKeyboardState;

        /// <summary>
        /// Creates a new in-game screen object
        /// </summary>
        /// <param name="game">game object</param>
        public IngameScreen(TheGame game)
            : base(game)
        {
            this.viewModel = new IngameViewModel(game);

            this.InitUserInterface(game);
        }

        /// <summary>
        /// Initializes user interface
        /// </summary>
        /// <param name="game">game instance</param>
        private void InitUserInterface(TheGame game)
        {
            var mapView = new MapView(this.viewModel.MapViewViewModel)
            {
                Width = 480,
                Height = 480,
                BorderThickness = 0,
                BackgroundColor = Color.Black,
            };

            var partyListView = new Box
            {
                Height = 200,
                Width = 320,
                BorderThickness = 0,
                FillColor = Color.DarkOrange,
            };

            var messageScrollView = new MessageScrollView
            {
                Height = 200,
                Width = 320,
            };

            messageScrollView.Bindings.Add(
                new Binding(
                    this.viewModel.MessageScrollViewModel,
                    nameof(MessageScrollViewModel.VisibleLines),
                    nameof(MessageScrollView.MessageScrollLines)));

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
            // nothing to do
        }

        /// <summary>
        /// Updates screen state, e.g. from keyboard or gamepad state
        /// </summary>
        /// <param name="gameTime">game time; unused</param>
        public override void Update(GameTime gameTime)
        {
            this.CheckKeyboard();
        }

        /// <summary>
        /// Checks keyboard keys and updates view model accordingly.
        /// </summary>
        private void CheckKeyboard()
        {
            var keyboardState = Keyboard.GetState();

            int moveX = 0;
            int moveY = 0;

            if (this.lastKeyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Right))
            {
                moveX = 1;
            }

            if (this.lastKeyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Left))
            {
                moveX = -1;
            }

            if (this.lastKeyboardState.IsKeyDown(Keys.Down) && !keyboardState.IsKeyDown(Keys.Down))
            {
                moveY = 1;
            }

            if (this.lastKeyboardState.IsKeyDown(Keys.Up) && !keyboardState.IsKeyDown(Keys.Up))
            {
                moveY = -1;
            }

            this.lastKeyboardState = keyboardState;

            if (moveX != 0 || moveY != 0)
            {
                this.viewModel.MovePlayer(moveX, moveY);
            }
        }

        /// <summary>
        /// Draws the in-game screen
        /// </summary>
        /// <param name="gameTime">game time; unused</param>
        public override void Draw(GameTime gameTime)
        {
            // nothing to do
        }
    }
}
