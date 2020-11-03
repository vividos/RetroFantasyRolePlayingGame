using Game.Core.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

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

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        /// <summary>
        /// Loads content for start game screen
        /// </summary>
        public override void LoadContent()
        {
            this.gameTitleFont = Content.Load<SpriteFont>("fonts/BlackcastleMF48");
            this.gameSubtitleFont = Content.Load<SpriteFont>("fonts/PixelAzureBonds24");
        }

        /// <summary>
        /// Updates screen state, e.g. from keyboard or gamepad state
        /// </summary>
        /// <param name="gameTime"></param>
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
