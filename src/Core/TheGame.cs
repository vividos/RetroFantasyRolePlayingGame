using Game.Core.Models;
using Game.Core.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using System.Diagnostics;
using System.IO;

namespace Game.Core
{
    /// <summary>
    /// The game
    /// </summary>
    public class TheGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Width of virtual drawing area
        /// </summary>
        private const int VirtualWidth = 800;

        /// <summary>
        /// Height of virtual drawing area
        /// </summary>
        private const int VirtualHeight = 480;

        /// <summary>
        /// Graphics device manager
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// Screen manager
        /// </summary>
        private readonly ScreenManager screenManager;

        /// <summary>
        /// Graphical User Interface system
        /// </summary>
        private GuiSystem guiSystem;

        /// <summary>
        /// Render target for rendering to fixd width 2D canvas
        /// </summary>
        private RenderTarget2D renderTarget;

        /// <summary>
        /// Sprite batch for drawing to render target
        /// </summary>
        private SpriteBatch targetBatch;

        /// <summary>
        /// Indicates if the game is running on a touch enabled device
        /// </summary>
        public bool IsTouchDevice { get; private set; }

        /// <summary>
        /// Screen width of the virtual screen to be scaled to the display
        /// </summary>
        public int ScreenWidth { get; private set; }

        /// <summary>
        /// Screen height
        /// </summary>
        public int ScreenHeight { get; private set; }

        /// <summary>
        /// The folder name where savegames are stored; may not exist yet
        /// </summary>
        public string SavegameFolder { get; set; }

        /// <summary>
        /// Contains the currently loaded game data
        /// </summary>
        public GameData CurrentGameData { get; set; }

        /// <summary>
        /// Creates a new game object
        /// </summary>
        /// <param name="isTouchEnabledDevice">
        /// indicates if the game is running on a touch enabled device
        /// </param>
        /// <param name="isMobileDevice">
        /// indicates if the game is running on a mobile device
        /// </param>
        public TheGame(bool isTouchEnabledDevice, bool isMobileDevice)
        {
            this.IsTouchDevice = isTouchEnabledDevice;

            this.ScreenWidth = VirtualWidth;
            this.ScreenHeight = VirtualHeight;

            this.graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = isMobileDevice,
                PreferredBackBufferWidth = this.ScreenWidth,
                PreferredBackBufferHeight = this.ScreenHeight,
                SupportedOrientations =
                DisplayOrientation.LandscapeLeft |
                DisplayOrientation.LandscapeRight
            };

            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = !isMobileDevice;

            TouchPanel.EnableMouseTouchPoint = true;

            this.screenManager = this.Components.Add<ScreenManager>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content. Calling base.Initialize() will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = GameData.GameName;
            if (!string.IsNullOrEmpty(GameData.GameSubtitle))
            {
                Window.Title += ": " + GameData.GameSubtitle;
            }

            this.renderTarget = new RenderTarget2D(
                this.graphics.GraphicsDevice,
                this.ScreenWidth,
                this.ScreenHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                this.graphics.GraphicsDevice.PresentationParameters.MultiSampleCount,
                RenderTargetUsage.DiscardContents);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.targetBatch = new SpriteBatch(this.GraphicsDevice);

            this.SetupUserInterface();

            this.NavigateToScreen(GameScreenType.StartGameScreen);
        }

        /// <summary>
        /// Sets up user interface using MonoGame.Extended.Gui.
        /// </summary>
        private void SetupUserInterface()
        {
            var viewportAdapter = new BoxingViewportAdapter(
                this.Window,
                this.GraphicsDevice,
                this.ScreenWidth,
                this.ScreenHeight);
            var guiRenderer = new GuiSpriteBatchRenderer(this.GraphicsDevice, () => Matrix.Identity);

            var guiFont = Content.Load<BitmapFont>("fonts/PixelAzureBondsBitmap24");
            BitmapFont.UseKernings = false;
            Skin.CreateDefault(guiFont);

            this.guiSystem = new GuiSystem(viewportAdapter, guiRenderer)
            {
                ActiveScreen = new MonoGame.Extended.Gui.Screen
                {
                    Content = new StackPanel
                    {
                        Width = this.ScreenWidth,
                        Height = this.ScreenHeight,
                    }
                }
            };

            this.guiSystem.ActiveScreen.Hide();
        }

        /// <summary>
        /// Sets new content for the GUI screen. This can be used for game screen classes to set
        /// a user interface using MonoGame.Extended.Gui.
        /// </summary>
        /// <param name="content">content to set, or null if nothing should be displayed</param>
        public void SetGuiScreenContent(Control content)
        {
            if (content != null)
            {
                this.guiSystem.ActiveScreen.Content = content;
                this.guiSystem.ActiveScreen.Show();
                this.guiSystem.ClientSizeChanged();
            }
            else
            {
                this.guiSystem.ActiveScreen.Hide();
            }
        }

        /// <summary>
        /// Navigates to a new screen
        /// </summary>
        /// <param name="type">game screen type</param>
        public void NavigateToScreen(GameScreenType type)
        {
            var transition = new MonoGame.Extended.Screens.Transitions.FadeTransition(GraphicsDevice, Color.Black, 0.5f);

            switch (type)
            {
                case GameScreenType.StartGameScreen:
                    this.screenManager.LoadScreen(new StartGameScreen(this), transition);
                    break;

                case GameScreenType.IngameScreen:
                    this.screenManager.LoadScreen(new IngameScreen(this), transition);
                    break;

                default:
                    Debug.Assert(false, "game screen type not handled yet!");
                    break;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            this.guiSystem.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself. This uses a RenderTarget2D object;
        /// see this link for more:
        /// http://www.infinitespace-studios.co.uk/general/monogame-scaling-your-game-using-rendertargets-and-touchpanel/
        /// Note that the suggestion setting TouchPanel properties doesn't work.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.renderTarget);

            this.GraphicsDevice.Clear(Color.Black);

            // draw currently active screen
            base.Draw(gameTime);

            if (this.guiSystem.ActiveScreen.IsVisible)
            {
                this.guiSystem.Draw(gameTime);
            }

            this.GraphicsDevice.SetRenderTarget(null);

            // draw render target
            Rectangle destinationRectangle = this.CalcDrawRectangle();

            this.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            this.targetBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            this.targetBatch.Draw(this.renderTarget, destinationRectangle, Color.White);
            this.targetBatch.End();
        }

        /// <summary>
        /// Calculates rectangle to draw in
        /// </summary>
        /// <returns>draw rectangle</returns>
        private Rectangle CalcDrawRectangle()
        {
            int windowWidth = this.Window.ClientBounds.Width;
            int windowHeight = this.Window.ClientBounds.Height;

            float outputAspect = windowWidth / (float)windowHeight;
            float preferredAspect = this.ScreenWidth / (float)this.ScreenHeight;

            Rectangle destinationRectangle;

            if (outputAspect <= preferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)(windowWidth / preferredAspect + 0.5f);
                int barHeight = (windowHeight - presentHeight) / 2;

                destinationRectangle = new Rectangle(0, barHeight, windowWidth, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)(windowHeight * preferredAspect + 0.5f);
                int barWidth = (windowWidth - presentWidth) / 2;

                destinationRectangle = new Rectangle(barWidth, 0, presentWidth, windowHeight);
            }

            return destinationRectangle;
        }

        /// <summary>
        /// Called when the game is unloaded
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            // store current game data when user closes the game
            if (this.CurrentGameData != null)
            {
                if (!Directory.Exists(this.SavegameFolder))
                {
                    Directory.CreateDirectory(this.SavegameFolder);
                }

                var savegameFilename = Path.Combine(this.SavegameFolder, GameData.SavegameFilename);

                using (var stream = new FileStream(savegameFilename, FileMode.Create))
                {
                    this.CurrentGameData.Save(stream);
                }
            }
        }

        /// <summary>
        /// Disposes of managed resources
        /// </summary>
        /// <param name="disposing">true when in Dispose(), false when in finalizer</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.renderTarget.Dispose();
                this.renderTarget = null;
            }

            base.Dispose(disposing);
        }
    }
}
