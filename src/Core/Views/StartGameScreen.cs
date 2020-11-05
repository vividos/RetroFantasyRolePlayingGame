using Game.Core.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.Screens;
using System;
using System.Diagnostics;

namespace Game.Core.Views
{
    /// <summary>
    /// Game screen that shows the game's title and lets the user start a new game or continue a
    /// stored game by "journeying onward".
    /// </summary>
    internal class StartGameScreen : GameScreen
    {
        /// <summary>
        /// View model for the start screen
        /// </summary>
        private readonly StartGameViewModel viewModel;

        /// <summary>
        /// Sprite batch for rendering
        /// </summary>
        private readonly SpriteBatch spriteBatch;

        /// <summary>
        /// Font for the game title
        /// </summary>
        private SpriteFont gameTitleFont;

        /// <summary>
        /// Font for the subtitle
        /// </summary>
        private SpriteFont gameSubtitleFont;

        /// <summary>
        /// Creates a new start game screen object
        /// </summary>
        /// <param name="game">game object</param>
        public StartGameScreen(TheGame game)
            : base(game)
        {
            this.viewModel = new StartGameViewModel();

            this.InitUserInterface(game);

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        /// <summary>
        /// Initializes user interface
        /// </summary>
        /// <param name="game">game instance</param>
        private void InitUserInterface(TheGame game)
        {
            const int ButtonWidth = 260;
            var backgroundColor = new Color(0, 128, 255);
            Color textColor = Color.White;

            var startNewGameButton = new Button
            {
                Content = "Start new game",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = ButtonWidth,
                HorizontalAlignment = HorizontalAlignment.Centre,
            };
            startNewGameButton.Clicked += this.OnClicked_StartNewGameButton;

            var journeyOnwardButton = new Button
            {
                Content = "Journey Onward",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = ButtonWidth,
                HorizontalAlignment = HorizontalAlignment.Centre,
                IsEnabled = this.viewModel.IsJourneyOnwardAvail,
            };
            journeyOnwardButton.Clicked += this.OnClicked_JourneyOnwardButton;

            var exitButton = new Button
            {
                Content = "Exit",
                BackgroundColor = backgroundColor,
                TextColor = textColor,
                Width = ButtonWidth,
                HorizontalAlignment = HorizontalAlignment.Centre,
            };
            exitButton.Clicked += this.OnClicked_ExitButton;

            var content = new StackPanel
            {
                Width = game.ScreenWidth,
                Height = game.ScreenHeight,
                Spacing = 8,
                Items =
                {
                    // used to move the buttons down, below the title text
                    new StackPanel
                    {
                        Height = 200,
                    },
                    startNewGameButton,
                    journeyOnwardButton,
                    exitButton,
                }
            };

            game.SetGuiScreenContent(content);
        }

        /// <summary>
        /// Called when the user clicked on the "start new game" button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="args">event args</param>
        private void OnClicked_StartNewGameButton(object sender, EventArgs args)
        {
            this.viewModel.StartNewGame();
        }

        /// <summary>
        /// Called when the user clicked on the "journey onward" button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="args">event args</param>
        private void OnClicked_JourneyOnwardButton(object sender, EventArgs args)
        {
            Debug.Assert(
                this.viewModel.IsJourneyOnwardAvail,
                "incorrectly called Clicked header since button must be disabled");

            this.viewModel.JourneyOnward();
        }

        /// <summary>
        /// Called when the user clicked on the "exit" button
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="args">event args</param>
        private void OnClicked_ExitButton(object sender, EventArgs args)
        {
            this.Game.Exit();
        }

        /// <summary>
        /// Loads content for start game screen
        /// </summary>
        public override void LoadContent()
        {
            this.gameTitleFont = Content.Load<SpriteFont>("fonts/BlackCastleMF48");
            this.gameSubtitleFont = Content.Load<SpriteFont>("fonts/PixelAzureBonds24");
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
        /// Draws the start game screen
        /// </summary>
        /// <param name="gameTime">game time; unused</param>
        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            this.DrawGameTitle();

            this.spriteBatch.End();
        }

        /// <summary>
        /// Draws game title and subtitle, if set
        /// </summary>
        private void DrawGameTitle()
        {
            string gameTitle = this.viewModel.GameTitle;
            string gameSubtitle = this.viewModel.GameSubtitle;

            if (!string.IsNullOrEmpty(gameSubtitle))
            {
                gameTitle += ":";
            }

            Vector2 textSize = this.gameTitleFont.MeasureString(gameTitle);

            var game = this.Game as TheGame;
            float pos = (game.ScreenWidth - textSize.X) / 2.0f;

            this.spriteBatch.DrawString(this.gameTitleFont, gameTitle, new Vector2(pos, 20), Color.White);

            if (!string.IsNullOrEmpty(gameSubtitle))
            {
                textSize = this.gameSubtitleFont.MeasureString(gameSubtitle);
                pos = (game.ScreenWidth - textSize.X) / 2.0f;

                this.spriteBatch.DrawString(this.gameSubtitleFont, gameSubtitle, new Vector2(pos, 90), Color.Yellow);
            }
        }
    }
}
