using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cervine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using GamePadState = Microsoft.Xna.Framework.Input.GamePadState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Cervine
{
    public class UserControlledSprite: Sprite
    {
        // Movement stuff
        MouseState prevMouseState;

        // Get direction of sprite based on player input and speed
        private Vector2 lastDirection;
        private Vector2 lastPosition;
        public Texture2D HungerTextureImage { get; set; }
        public Texture2D YellowTextureImage { get; set; }
        public Texture2D BombTexture { get; set; }

        public int Life { get; set; }
        public int HungerDelay { get; set; }
        public int MaxHungerDelay { get; set; }
        public int Hunger { get; set; }

        public UserControlledSprite(Texture2D texture2D, Point position, GameBoard board, Texture2D lifeTextureImage, Texture2D hungerTextureImage,
            Texture2D yellowUserTexture2D, Texture2D bombTexture) : base(texture2D, position, board)
        {
            MaxHungerDelay = 30;
            Life = 3;
            MaxLife = 3;
            Hunger = 200;
            this.LifeTextureImage = lifeTextureImage;
            this.HungerTextureImage = hungerTextureImage;
            this.YellowTextureImage = yellowUserTexture2D;
            this.BombTexture = bombTexture;
        }

        public override Point direction
        {
            get
            {
                Point inputDirection = Point.Zero;
                var keyboardState = Keyboard.GetState();
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

        public override void Reset()
        {
            this.Position = new Point(0,0);
            base.Reset();
        }

        public Texture2D LifeTextureImage { get; set; }

        public override void Update(GameTime gameTime)
        {
            if (Delay == 0)
            {
                var newPosition = new Point(Position.X + direction.X, Position.Y + direction.Y);
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
                        if(Life < MaxLife)
                            this.Life++;
                    }
                    else if (powerup is FoodPowerUp)
                    {
                        this.Hunger = Math.Min(Hunger + 20, 200);
                    }
                    else
                    {
                        this.PowerUp = powerup;
                    }
                    
                    board.RemoveObject(powerup);   
                }
                if (board.IsPositionValid(newPosition))
                {
                    var oldPosition = Position;
                    Position = newPosition;
                    board.ChangePosition(oldPosition, this);
                }
                
                
                var key = Keyboard.GetState().GetPressedKeys();
                if (key.Contains(Keys.Space))
                {
                    if (PowerUp is TntDetonatorPowerUp)
                    {
                        var tnt = PowerUp as TntDetonatorPowerUp;
                        if (tnt.IsPlanted)
                        {
                            tnt.Detonate();
                            this.PowerUp = null;
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
                    PowerUp.Life -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (PowerUp.Life <= 0)
                    {
                        PowerUp = null;
                    }
                }
                else // Dynamite
                {
                    
                }
                
            }
            Delay = (Delay + 1)%5;
        }

        public decimal MaxLife { get; set; }

        public PowerUp PowerUp { get; set; }

        public override void DecreaseLife()
        {
            Life--;
            if (Life <= 0)
            {
                board.GameOver();
            }
        }

        public int LifeDelay { get; set; }

        public bool HasPowerUp
        {
            get { return PowerUp != null; }
        }
    }
}