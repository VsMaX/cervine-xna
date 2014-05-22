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
        public int Life { get; set; }
        public int HungerDelay { get; set; }
        public int MaxHungerDelay { get; set; }
        public int Hunger { get; set; }

        public UserControlledSprite(Texture2D texture2D, Point position, GameBoard board, Texture2D lifeTextureImage) : base(texture2D, position, board)
        {
            MaxHungerDelay = 600;
            Life = 3;
            this.LifeTextureImage = lifeTextureImage;
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

                if (Life < 0)
                {
                    board.OnGameOver();
                }
                if (board.IsPositionValid(newPosition))
                {
                    var oldPosition = Position;
                    Position = newPosition;
                    board.ChangePosition(oldPosition, this);
                }
                else
                {
                    if (board.Fields[newPosition.X, newPosition.Y].Sprite is EnemySprite)
                    {
                        this.Life--;
                    }
                }
                if (HungerDelay >= MaxHungerDelay)
                {
                    HungerDelay = 0;
                    Hunger++;
                }
            }
            Delay = (Delay + 1)%5;
            var key = Keyboard.GetState().GetPressedKeys();
            if (key.Contains(Keys.Space))
            {
                board.PlantBomb(Position);
            }
        }
    }
}