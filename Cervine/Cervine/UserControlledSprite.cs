using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    public class UserControlledSprite : Sprite
    {
        // Movement stuff

        // Get Direction of sprite based on player input and speed
        private readonly Texture2D _destroyableWallTexture;
        private Vector2 lastDirection;
        private Vector2 lastPosition;
        private MouseState prevMouseState;

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

        public int Life { get; set; }
        public int HungerDelay { get; set; }
        public int MaxHungerDelay { get; set; }
        public int Hunger { get; set; }

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

        public decimal MaxLife { get; set; }

        public PowerUp PowerUp { get; set; }

        public int LifeDelay { get; set; }

        public bool HasPowerUp
        {
            get { return PowerUp != null; }
        }

        public override void Reset()
        {
            Position = new Point(0, 0);
            base.Reset();
        }

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

        public override void DecreaseLife()
        {
            Life--;
        }
    }
}