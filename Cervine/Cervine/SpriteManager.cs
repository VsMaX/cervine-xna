using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public MainMenu pauseMenu;
        public TimeSpan gameTimeElapsed;
        public SpriteFont gameTimeFont;
        public CervineGame game;
        public GameBoard gameBoard;

        public SpriteManager(CervineGame game, Point boardSize)
            : base(game)
        {
            Game.Content.RootDirectory = "Content";
            this.game = game;
            this.boardSize = boardSize;
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
            this.gameBoard.ResetGame();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            gameTimeFont = Game.Content.Load<SpriteFont>(@"arial");

            this.gameBoard = new GameBoard(boardSize, 40, 40, 80, game, gameTimeFont);
            gameBoard.GameOverEvent += OnGameOver;
            //Load the player sprite
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"player_transparent"),
                new Point(0, 0), gameBoard, Game.Content.Load<Texture2D>(@"heart"));

            gameBoard.AddObject(player);

            backgroundImage = Game.Content.Load<Texture2D>(@"background");

            gameBoard.AddObject(new NormalEnemySprite(Game.Content.Load<Texture2D>(@"Enemies/enemy_normal_transparent"),
                new Point(19, 14), gameBoard));

            gameBoard.AddObject(new HunterEnemySprite(Game.Content.Load<Texture2D>(@"Enemies/enemy_hunter_transparent"),
                new Point(5,0), gameBoard));
            
            var r = new Random();

            //generate random walls
            for (int i = 0; i < 20; i++)
            {
                int X = r.Next(boardSize.X);
                int Y = r.Next(boardSize.Y);
                var position = new Point(X, Y);
                if (gameBoard.IsPositionValid(position))
                {
                    gameBoard.AddObject(new WallSprite(Game.Content.Load<Texture2D>(@"wall"), new Point(X, Y),
                        gameBoard));
                }
            }
            //generate random destroyable walls
            for (int i = 0; i < 20; i++)
            {
                int X = r.Next(boardSize.X);
                int Y = r.Next(boardSize.Y);
                var position = new Point(X, Y);
                if (gameBoard.IsPositionValid(position))
                {
                    var wallSprite = new DestroyableWallSprite(Game.Content.Load<Texture2D>(@"wall_destroyable"),
                    new Point(X, Y), gameBoard);
                    gameBoard.AddObject(wallSprite);
                }
            }

            var menuSpriteList = new List<MenuSprite>
            {
                new MenuSprite("NOWA-GRA", Game.Content.Load<Texture2D>(@"UI/NOWA-GRA"),
                    Game.Content.Load<Texture2D>(@"UI/NOWA-GRA-HOVER"), new Vector2(200, 50)),
                new MenuSprite("ZALADUJ-GRE", Game.Content.Load<Texture2D>(@"UI/ZALADUJ-GRE"),
                    Game.Content.Load<Texture2D>(@"UI/ZALADUJ-GRE-HOVER"), new Vector2(200, 100)),
                new MenuSprite("NAJLEPSZE-WYNIKI", Game.Content.Load<Texture2D>(@"UI/NAJLEPSZE-WYNIKI"),
                    Game.Content.Load<Texture2D>(@"UI/NAJLEPSZE-WYNIKI-HOVER"), new Vector2(200, 150)),
                new MenuSprite("STEROWANIE", Game.Content.Load<Texture2D>(@"UI/STEROWANIE"),
                    Game.Content.Load<Texture2D>(@"UI/STEROWANIE-HOVER"), new Vector2(200, 200)),
                new MenuSprite("DZWIEK", Game.Content.Load<Texture2D>(@"UI/DZWIEK"),
                    Game.Content.Load<Texture2D>(@"UI/DZWIEK-HOVER"), new Vector2(200, 250)),
                new MenuSprite("POMOC", Game.Content.Load<Texture2D>(@"UI/POMOC"),
                    Game.Content.Load<Texture2D>(@"UI/POMOC-HOVER"), new Vector2(200, 300))
            };

            var pauseMenuSpriteList = new List<MenuSprite>()
            {
                new MenuSprite("WROC DO GRY", Game.Content.Load<Texture2D>(@"UI/WROC-DO-GRY"),
                    Game.Content.Load<Texture2D>(@"UI/WROC-DO-GRY-HOVER"), new Vector2(200, 50)),
                new MenuSprite("ZAPISZ-GRE", Game.Content.Load<Texture2D>(@"UI/ZAPISZ-GRE"),
                    Game.Content.Load<Texture2D>(@"UI/ZAPISZ-GRE-HOVER"), new Vector2(200, 100)),
                new MenuSprite("STEROWANIE", Game.Content.Load<Texture2D>(@"UI/STEROWANIE"),
                    Game.Content.Load<Texture2D>(@"UI/STEROWANIE-HOVER"), new Vector2(200, 150)),
                new MenuSprite("DZWIEK", Game.Content.Load<Texture2D>(@"UI/DZWIEK"),
                    Game.Content.Load<Texture2D>(@"UI/DZWIEK-HOVER"), new Vector2(200, 200)),
                new MenuSprite("POMOC", Game.Content.Load<Texture2D>(@"UI/POMOC"),
                    Game.Content.Load<Texture2D>(@"UI/POMOC-HOVER"), new Vector2(200, 250)),
                new MenuSprite("WYJSCIE DO MENU", Game.Content.Load<Texture2D>(@"UI/WYJSCIE-DO-MENU"),
                    Game.Content.Load<Texture2D>(@"UI/WYJSCIE-DO-MENU-HOVER"), new Vector2(200, 300))
            };

            mainMenu = new MainMenu(game, menuSpriteList);
            pauseMenu = new MainMenu(game, pauseMenuSpriteList);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (game.GameState == GameState.MainMenu)
            {
                UpdateGameMenu(gameTime);
            }
            else if (game.GameState == GameState.Playing)
            {
                UpdateGamePlaying(gameTime);
            }
            else if (game.GameState == GameState.PauseMenu)
            {
                UpdatePauseMenu(gameTime);
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
                game.GameState = GameState.PauseMenu;
            }

            gameBoard.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            if (game.GameState == GameState.Playing)
            {
                DrawGamePlaying(gameTime);
            }
            else if (game.GameState == GameState.MainMenu)
            {
                mainMenu.Draw(gameTime, spriteBatch);
            }
            else if (game.GameState == GameState.PauseMenu)
            {
                pauseMenu.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawGamePlaying(GameTime gameTime)
        {
            // Draw the player
            gameBoard.Draw(gameTime, spriteBatch);
            //UI
            //game time
            gameTimeElapsed += gameTime.ElapsedGameTime;
            spriteBatch.DrawString(gameTimeFont, gameTimeElapsed.TotalSeconds.ToString("0000"), new Vector2(20, 10), Color.White);
            //background
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, 800, 680), Color.White);
        }
    }
}