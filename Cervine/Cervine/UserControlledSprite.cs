using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    /// <summary>
    /// Player controlled by user
    /// </summary>
    public class UserControlledSprite : Sprite
    {
        private readonly Texture2D _destroyableWallTexture;
        private Vector2 lastDirection;
        private Vector2 lastPosition;
        private MouseState prevMouseState;
        /// <summary>
        /// Ctor for UserControlledSprite
        /// </summary>
        /// <param name="texture2D">Texture of user to be displayed on screen</param>
        /// <param name="position">Initial position of UserControlledSprite</param>
        /// <param name="board">GameBoard of current game on which UserControlledSprite is</param>
        /// <param name="lifeTextureImage">Texture used for displaying life</param>
        /// <param name="hungerTextureImage">Texture used for displaying hunger</param>
        /// <param name="yellowUserTexture2D">Texture used for diplaying charging effect</param>
        /// <param name="bombTexture">Texture used for displaying bomb on map</param>
        /// <param name="destroyableWallTexture">Texture used for displaying destroyable wall on map</param>
        public UserControlledSprite(Texture2D texture2D, Point position, GameBoard board, Texture2D lifeTextureImage,
            Texture2D hungerTextureImage,
            Texture2D yellowUserTexture2D, Texture2D bombTexture, Texture2D destroyableWallTexture)
            : base(texture2D, position, board)
        {
            MaxHungerDelay = 30;
            Life = 3;
            MaxLife = 3;
            Hunger = 150;
            LifeTextureImage = lifeTextureImage;
            HungerTextureImage = hungerTextureImage;
            YellowTextureImage = yellowUserTexture2D;
            BombTexture = bombTexture;
            _destroyableWallTexture = destroyableWallTexture;
        }

        public Texture2D HungerTextureImage { get; set; }
        public Texture2D YellowTextureImage { get; set; }
        public Texture2D BombTexture { get; set; }
        /// <summary>
        /// User life
        /// </summary>
        public int Life { get; set; }
        /// <summary>
        /// User hunger delay before hunger decreases
        /// </summary>
        public int HungerDelay { get; set; }
        /// <summary>
        /// Maximum hunger that user can have
        /// </summary>
        public int MaxHungerDelay { get; set; }
        /// <summary>
        /// Current hunger value
        /// </summary>
        public int Hunger { get; set; }
        /// <summary>
        /// Gets user input for direction
        /// </summary>
        public override Point Direction
        {
            get
            {
                Point inputDirection = Point.Zero;
                KeyboardState keyboardState = Keyboard.GetState();
                // If player pressed arrow keys, move the sprite
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                }
                else if (keyboardState.IsKeyDown(Keys.Up))
                {
                    inputDirection.Y -= 1;
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    inputDirection.Y += 1;
                }

                return inputDirection;
            }
        }

        public Texture2D LifeTextureImage { get; set; }
        /// <summary>
        /// Maximum life that user can have
        /// </summary>
        public decimal MaxLife { get; set; }
        /// <summary>
        /// Current powerup holding by user
        /// </summary>
        public PowerUp PowerUp { get; set; }
        /// <summary>
        /// Delay before next life can be subtracted from user
        /// </summary>
        public int LifeDelay { get; set; }
        /// <summary>
        /// Returns true if user has any powerup in slot, false otherwise
        /// </summary>
        public bool HasPowerUp
        {
            get { return PowerUp != null; }
        }
        /// <summary>
        /// Resets player stats
        /// </summary>
        public override void Reset()
        {
            Position = new Point(0, 0);
            base.Reset();
        }
        /// <summary>
        /// Takes user input and moves player accordingly.
        /// Picks up all PowerUps and handles bomb planting.
        /// Decreases hunger for player.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Delay == 0)
            {
                var newPosition = new Point(Position.X + Direction.X, Position.Y + Direction.Y);
                newPosition = board.AdjustToBoardSize(newPosition);

                if (Life < 0 || Hunger < 0)
                {
                    board.GameOver();
                }
                if (board.Fields[newPosition.X, newPosition.Y].Sprite is PowerUp)
                {
                    var powerup = board.Fields[newPosition.X, newPosition.Y].Sprite as PowerUp;
                    if (powerup is MedpackPowerUp)
                    {
                        if (Life < MaxLife)
                            Life++;
                    }
                    else if (powerup is FoodPowerUp)
                    {
                        Hunger = Math.Min(Hunger + 20, 150);
                    }
                    else if (powerup is DebrisPowerUp)
                    {
                        if (PowerUp is DebrisPowerUp)
                        {
                            var playerDebris = PowerUp as DebrisPowerUp;
                            playerDebris.IncreaseDebris();
                        }
                        else
                        {
                            var debrisPowerUp = powerup as DebrisPowerUp;
                            debrisPowerUp.DebrisCount = 1;
                            PowerUp = debrisPowerUp;
                        }
                    }
                    else
                    {
                        PowerUp = powerup;
                    }

                    board.RemoveObject(powerup);
                }

                Keys[] key = Keyboard.GetState().GetPressedKeys();
                if (key.Contains(Keys.X) && PowerUp is DebrisPowerUp) // plant wall
                {
                    var debris = PowerUp as DebrisPowerUp;
                    if (debris.DebrisCount > 1)
                    {
                        var wallPos = new Point();
                        if (key.Contains(Keys.Left))
                        {
                            wallPos = new Point(Position.X - 1, Position.Y);
                            if (board.IsPositionValid(wallPos))
                            {
                                var destroyableWall = new DestroyableWallSprite(_destroyableWallTexture, wallPos, board);
                                board.AddObject(destroyableWall);
                                debris.DebrisCount -= 2;
                                if (debris.DebrisCount == 0)
                                    PowerUp = null;
                            }
                        }
                        if (key.Contains(Keys.Right))
                        {
                            wallPos = new Point(Position.X + 1, Position.Y);
                            if (board.IsPositionValid(wallPos))
                            {
                                var destroyableWall = new DestroyableWallSprite(_destroyableWallTexture, wallPos, board);
                                board.AddObject(destroyableWall);
                                debris.DebrisCount -= 2;
                                if (debris.DebrisCount == 0)
                                    PowerUp = null;
                            }
                        }
                        if (key.Contains(Keys.Up))
                        {
                            wallPos = new Point(Position.X, Position.Y - 1);
                            if (board.IsPositionValid(wallPos))
                            {
                                var destroyableWall = new DestroyableWallSprite(_destroyableWallTexture, wallPos, board);
                                board.AddObject(destroyableWall);
                                debris.DebrisCount -= 2;
                                if (debris.DebrisCount == 0)
                                    PowerUp = null;
                            }
                        }
                        if (key.Contains(Keys.Down))
                        {
                            wallPos = new Point(Position.X, Position.Y + 1);
                            if (board.IsPositionValid(wallPos))
                            {
                                var destroyableWall = new DestroyableWallSprite(_destroyableWallTexture, wallPos, board);
                                board.AddObject(destroyableWall);
                                debris.DebrisCount -= 2;
                                if (debris.DebrisCount == 0)
                                    PowerUp = null;
                            }
                        }
                    }
                }
                if (board.IsPositionValid(newPosition))
                {
                    Point oldPosition = Position;
                    Position = newPosition;
                    board.ChangePosition(oldPosition, this);
                }

                if (key.Contains(Keys.Space))
                {
                    if (PowerUp is TntDetonatorPowerUp)
                    {
                        var tnt = PowerUp as TntDetonatorPowerUp;
                        if (tnt.IsPlanted)
                        {
                            tnt.Detonate();
                            PowerUp = null;
                        }
                        else
                        {
                            tnt.Plant(Position);
                        }
                    }
                    else
                    {
                        if (board.Bombs.Count(x => x.GetType() == typeof (Bomb)) < 2)
                        {
                            var bomb = new Bomb(BombTexture, Position, board);
                            board.Plant(bomb);
                        }
                    }
                }

                if (HungerDelay >= MaxHungerDelay)
                {
                    HungerDelay = 0;
                    Hunger--;
                }
                else
                {
                    HungerDelay++;
                }
            }
            if (PowerUp != null)
            {
                if (PowerUp is ChargingPowerUp)
                {
                    PowerUp.Life -= (int) gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (PowerUp.Life <= 0)
                    {
                        PowerUp = null;
                    }
                }
            }
            Delay = (Delay + 1)%5;
        }
        /// <summary>
        /// Decreases player's life by one
        /// </summary>
        public override void DecreaseLife()
        {
            Life--;
        }
    }
}