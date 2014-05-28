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
        public ScoresMenu gameScoresMenu;
        public TimeSpan gameTimeElapsed;
        public SpriteFont gameTimeFont;
        public CervineGame game;
        public GameBoard gameBoard;

        public SpriteManager(CervineGame game, Point boardSize, SpriteBatch spriteBatch)
            : base(game)
        {
            Game.Content.RootDirectory = "Content";
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

        protected override void LoadContent()
        {
            gameTimeFont = Game.Content.Load<SpriteFont>(@"arial");
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
            gameBoard = new GameBoard(boardSize, 40, 40, 80, this, gameTimeFont, bombfireTexture,
                normalEnemyTexture, hunterEnemyTexture, shooterEnemyTexture, tankEnemyTexture, foodTexture,
                medpackTexture, tntDetonatorTexture, chargingTexture, tntTexture, bombTexture);
            gameBoard.GameOverEvent += OnGameOver;
            //Load the player sprite
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"player_transparent"),
                new Point(0, 0), gameBoard, Game.Content.Load<Texture2D>(@"heart"),
                hungerTexture,
                Game.Content.Load<Texture2D>(@"player_transparent_yellow"),
                bombTexture);

            gameBoard.AddObject(player);

            backgroundImage = Game.Content.Load<Texture2D>(@"background");
            
            var r = new Random();
            var wallSprite = Game.Content.Load<Texture2D>(@"wall");
            //generate random walls
            for (int i = 0; i < 30; i++)
            {
                int X = r.Next(boardSize.X);
                int Y = r.Next(boardSize.Y);
                var position = new Point(X, Y);
                if (gameBoard.IsPositionValid(position) && position.X >= 3 && position.Y >= 3)
                {
                    gameBoard.AddObject(new WallSprite(wallSprite, new Point(X, Y),
                        gameBoard));
                }
            }

            var wallDestroyableSprite = Game.Content.Load<Texture2D>(@"wall_destroyable");
            //generate random destroyable walls
            for (int i = 0; i < 50; i++)
            {
                int X = r.Next(boardSize.X);
                int Y = r.Next(boardSize.Y);
                var position = new Point(X, Y);
                if (gameBoard.IsPositionValid(position) && position.X >= 3 && position.Y >= 3)
                {
                    var destrWallSprite = new DestroyableWallSprite(wallDestroyableSprite,
                    new Point(X, Y), gameBoard);
                    gameBoard.AddObject(destrWallSprite);
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
            
            gameScoresMenu = new ScoresMenu(this.game, new List<Score>(), gameTimeFont);
            base.LoadContent();
        }

        public void OnGameOver()
        {
            
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
            else if(game.GameState == GameState.GameOver)
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
                game.GameState = GameState.PauseMenu;
            }

            gameBoard.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, 800, 680), Color.White);

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
            else if (game.GameState == GameState.GameOver)
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
            spriteBatch.DrawString(gameTimeFont, gameTimeElapsed.TotalSeconds.ToString("0000"), new Vector2(20, 10), Color.White);
            
            gameBoard.Draw(gameTime, spriteBatch);
        }
    }

    public class ScoresMenu
    {
        private SpriteFont _font;
        private List<Score> _scoresList;

        public ScoresMenu(CervineGame cervineGame, List<Score> scoresList, SpriteFont font)
        {
            this._font = font;
            this._scoresList = scoresList;
        }

        public void GetHighScoresUserName(int score)
        {
            var scores = new Score();
            scores.Time = score;
            this.GetUserName = true;
            this.Score = scores;
            this._scoresList.Add(Score);
        }

        public Score Score { get; set; }
        public bool GetUserName { get; set; }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (GetUserName)
            {
                var keyboard = Keyboard.GetState().GetPressedKeys();
                var key = keyboard.FirstOrDefault();
                if (key != Keys.Enter)
                {
                    Score.Name += key.ToString();
                }
                else
                {
                    GetUserName = false;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var scores = _scoresList.OrderByDescending(x => x.Time).ToList();
            for (int i = 0; i < scores.Count; i++)
            {
                var score = scores[i];
                spriteBatch.DrawString(_font, score.Name, new Vector2(20, 10 + 50 * i), Color.White);
                spriteBatch.DrawString(_font, score.Time.ToString("0000"), new Vector2(550, 10 + 50* i), Color.White);
            }
        }
    }

    public class Score
    {
        public string Name { get; set; }
        public int Time { get; set; }
    }
}