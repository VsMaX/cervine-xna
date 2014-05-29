using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using Cervine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Cervine
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : DrawableGameComponent
    {
        //SpriteBatch for drawing
        SpriteBatch spriteBatch;
        
        //A sprite for the player and a list of automated sprites
        UserControlledSprite player;
        Texture2D backgroundImage;
        List<Sprite> spriteList = new List<Sprite>();

        public Point boardSize;
        public MainMenu mainMenu;
        public PauseMenu pauseMenu;
        public ScoresMenu gameScoresMenu;
        public TimeSpan gameTimeElapsed;
        public SpriteFont gameTimeFont;
        public CervineGame game;
        public GameBoard gameBoard;
        private ContentManager _contentManager;

        public SpriteManager(CervineGame game, Point boardSize, SpriteBatch spriteBatch, ContentManager contentManager)
            : base(game)
        {
            Game.Content.RootDirectory = "Content";
            _contentManager = contentManager;
            this.game = game;
            this.boardSize = boardSize;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            gameTimeElapsed = new TimeSpan();
            base.Initialize();
        }

        public void OnGameOver(object sender, EventArgs e)
        {
            game.GameState = GameState.MainMenu;
            gameBoard.ResetGame();
        }

        public void NewGame()
        {
            var normalEnemyTexture = Game.Content.Load<Texture2D>(@"Enemies/normal");
            var hunterEnemyTexture = Game.Content.Load<Texture2D>(@"Enemies/hunter");
            var shooterEnemyTexture = Game.Content.Load<Texture2D>(@"Enemies/shooter");
            var tankEnemyTexture = Game.Content.Load<Texture2D>(@"Enemies/tank");
            var bombfireTexture = Game.Content.Load<Texture2D>(@"bombfire");
            var bombTexture = Game.Content.Load<Texture2D>(@"bomb");
            var hungerTexture = Game.Content.Load<Texture2D>(@"hungerbar");
            var foodTexture = Game.Content.Load<Texture2D>(@"food");
            var medpackTexture = Game.Content.Load<Texture2D>(@"medpack");
            var tntDetonatorTexture = Game.Content.Load<Texture2D>(@"tntdetonator");
            var chargingTexture = Game.Content.Load<Texture2D>(@"charging");
            var tntTexture = Game.Content.Load<Texture2D>(@"tnt");
            var debrisTexture1 = Game.Content.Load<Texture2D>(@"debris1");
            var debrisTexture2 = Game.Content.Load<Texture2D>(@"debris2");
            var debrisTexture3 = Game.Content.Load<Texture2D>(@"debris3");
            var debrisTexture4 = Game.Content.Load<Texture2D>(@"debris4");
            var wallDestroyableTexture = Game.Content.Load<Texture2D>(@"wall_destroyable");

            gameBoard = new GameBoard(boardSize, 40, 40, 80, this, gameTimeFont, bombfireTexture,
                normalEnemyTexture, hunterEnemyTexture, shooterEnemyTexture, tankEnemyTexture, foodTexture,
                medpackTexture, tntDetonatorTexture, chargingTexture, tntTexture, bombTexture, debrisTexture1, debrisTexture2, debrisTexture3, debrisTexture4, wallDestroyableTexture);
            gameBoard.GameOverEvent += OnGameOver;
            //Load the player sprite
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"player_transparent"),
                new Point(0, 0), gameBoard, Game.Content.Load<Texture2D>(@"heart"),
                hungerTexture,
                Game.Content.Load<Texture2D>(@"player_transparent_yellow"),
                bombTexture,
                wallDestroyableTexture);

            gameBoard.AddObject(player);


            var r = new Random();
            var wallSprite = Game.Content.Load<Texture2D>(@"wall");
            //generate random walls
            for (int i = 0; i < 30; i++)
            {
                int X = r.Next(boardSize.X);
                int Y = r.Next(boardSize.Y);
                var position = new Point(X, Y);
                if (gameBoard.IsPositionValid(position) && Math.Sqrt(position.X * position.X + position.Y * position.Y) >= 3)
                {
                    gameBoard.AddObject(new WallSprite(wallSprite, new Point(X, Y),
                        gameBoard));
                }
            }

            //generate random destroyable walls
            for (int i = 0; i < 50; i++)
            {
                int X = r.Next(boardSize.X);
                int Y = r.Next(boardSize.Y);
                var position = new Point(X, Y);
                if (gameBoard.IsPositionValid(position) && Math.Sqrt(position.X * position.X + position.Y * position.Y) >= 3)
                {
                    var destrWallSprite = new DestroyableWallSprite(wallDestroyableTexture,
                    new Point(X, Y), gameBoard);
                    gameBoard.AddObject(destrWallSprite);
                }
            }
        }

        protected override void LoadContent()
        {
            backgroundImage = Game.Content.Load<Texture2D>(@"background");
            gameTimeFont = Game.Content.Load<SpriteFont>(@"arial");

            var menuSpriteList = new List<MenuSprite>
            {
                new MenuSprite("NOWA-GRA", Game.Content.Load<Texture2D>(@"UI/NOWA-GRA"),
                    Game.Content.Load<Texture2D>(@"UI/NOWA-GRA-HOVER"), new Vector2(250, 150)),
                new MenuSprite("ZALADUJ-GRE", Game.Content.Load<Texture2D>(@"UI/ZALADUJ-GRE"),
                    Game.Content.Load<Texture2D>(@"UI/ZALADUJ-GRE-HOVER"), new Vector2(250, 200)),
                new MenuSprite("NAJLEPSZE-WYNIKI", Game.Content.Load<Texture2D>(@"UI/NAJLEPSZE-WYNIKI"),
                    Game.Content.Load<Texture2D>(@"UI/NAJLEPSZE-WYNIKI-HOVER"), new Vector2(250, 250)),
                new MenuSprite("STEROWANIE", Game.Content.Load<Texture2D>(@"UI/STEROWANIE"),
                    Game.Content.Load<Texture2D>(@"UI/STEROWANIE-HOVER"), new Vector2(250, 300)),
                new MenuSprite("DZWIEK", Game.Content.Load<Texture2D>(@"UI/DZWIEK"),
                    Game.Content.Load<Texture2D>(@"UI/DZWIEK-HOVER"), new Vector2(250, 350)),
                new MenuSprite("POMOC", Game.Content.Load<Texture2D>(@"UI/POMOC"),
                    Game.Content.Load<Texture2D>(@"UI/POMOC-HOVER"), new Vector2(250, 400))
            };

            var pauseMenuSpriteList = new List<MenuSprite>()
            {
                new MenuSprite("WROC DO GRY", Game.Content.Load<Texture2D>(@"UI/WROC-DO-GRY"),
                    Game.Content.Load<Texture2D>(@"UI/WROC-DO-GRY-HOVER"), new Vector2(250, 150)),
                new MenuSprite("ZAPISZ-GRE", Game.Content.Load<Texture2D>(@"UI/ZAPISZ-GRE"),
                    Game.Content.Load<Texture2D>(@"UI/ZAPISZ-GRE-HOVER"), new Vector2(250, 200)),
                new MenuSprite("STEROWANIE", Game.Content.Load<Texture2D>(@"UI/STEROWANIE"),
                    Game.Content.Load<Texture2D>(@"UI/STEROWANIE-HOVER"), new Vector2(250, 250)),
                new MenuSprite("DZWIEK", Game.Content.Load<Texture2D>(@"UI/DZWIEK"),
                    Game.Content.Load<Texture2D>(@"UI/DZWIEK-HOVER"), new Vector2(250, 300)),
                new MenuSprite("POMOC", Game.Content.Load<Texture2D>(@"UI/POMOC"),
                    Game.Content.Load<Texture2D>(@"UI/POMOC-HOVER"), new Vector2(250, 350)),
                new MenuSprite("WYJSCIE DO MENU", Game.Content.Load<Texture2D>(@"UI/WYJSCIE-DO-MENU"),
                    Game.Content.Load<Texture2D>(@"UI/WYJSCIE-DO-MENU-HOVER"), new Vector2(250, 400))
            };

            mainMenu = new MainMenu(this, menuSpriteList);
            pauseMenu = new PauseMenu(this, pauseMenuSpriteList);
            
            gameScoresMenu = new ScoresMenu(this, new List<Score>(), gameTimeFont);
            base.LoadContent();
        }
        
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (this.GameState == GameState.MainMenu)
            {
                UpdateGameMenu(gameTime);
            }
            else if (this.GameState == GameState.Playing)
            {
                UpdateGamePlaying(gameTime);
            }
            else if (this.GameState == GameState.PauseMenu)
            {
                UpdatePauseMenu(gameTime);
            }
            else if (this.GameState == GameState.GameOver)
            {
                gameScoresMenu.Update(gameTime, Game.Window.ClientBounds);
            }

            base.Update(gameTime);
        }

        private void UpdatePauseMenu(GameTime gameTime)
        {
            pauseMenu.Update(gameTime, Game.Window.ClientBounds);
        }

        private void UpdateGameMenu(GameTime gameTime)
        {
            mainMenu.Update(gameTime, Game.Window.ClientBounds);
        }

        public void UpdateGamePlaying(GameTime gameTime)
        {
            var key = Keyboard.GetState().GetPressedKeys();
            if (key.Contains(Keys.Escape))
            {
                this.GameState = GameState.PauseMenu;
            }

            gameBoard.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, 800, 680), Color.White);

            if (this.GameState == GameState.Playing)
            {
                DrawGamePlaying(gameTime);
            }
            else if (this.GameState == GameState.MainMenu)
            {
                mainMenu.Draw(gameTime, spriteBatch);
            }
            else if (this.GameState == GameState.PauseMenu)
            {
                pauseMenu.Draw(gameTime, spriteBatch);
            }
            else if (this.GameState == GameState.GameOver)
            {
                gameScoresMenu.Draw(gameTime, spriteBatch);
            }
            base.Draw(gameTime);

            spriteBatch.End();
        }

        public void DrawGamePlaying(GameTime gameTime)
        {
            // Draw the player
            // UI
            // game time
            gameTimeElapsed += gameTime.ElapsedGameTime;
            
            gameBoard.Draw(gameTime, spriteBatch);
        }

        public void GameOver(int timeScore)
        {
            var score = new Score();
            this.GameState = GameState.GameOver;
            score.Time = timeScore;
            gameScoresMenu.GetHighScoresUserName(timeScore);
        }

        public GameState GameState { get; set; }
    }
}