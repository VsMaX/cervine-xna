using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cervine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class ShooterMonsterEnemySprite : EnemySprite
    {
        public ShooterMonsterEnemySprite(Texture2D shooterEnemyTexture, Point position, GameBoard gameBoard)
            : base(shooterEnemyTexture, position, gameBoard)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Delay == 0)
            {
                if (IsOnAxisWithPlayer())
                {
                    //ShootPlayer();
                }
                timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
                var newPosition = Position;
                if (timeElapsed >= timeBeforeDirectionChange)
                {
                    timeElapsed = 0;
                    ChangeDirection();
                }
                newPosition = new Point(newPosition.X + _direction.X, newPosition.Y + _direction.Y);
                var oldPosition = Position;
                newPosition = board.AdjustToBoardSize(newPosition);
                if (board.IsPositionValidExceptPlayer(newPosition))
                {
                    Position = newPosition;
                    board.ChangePosition(oldPosition, this);
                }
                else
                {
                    ChangeDirection();
                }
            }
            Delay = (Delay + 1)%20;
            base.Update(gameTime);
        }

        private bool IsOnAxisWithPlayer()
        {
            if (this.Position.X == board.Player.Position.X)
            {
                if (this.Position.Y < board.Player.Position.Y)
                {
                    for (int y = this.Position.Y; y < board.Player.Position.Y; y++)
                    {
                        if (board.Fields[this.Position.X, y].Sprite != null)
                            return false;
                    }   
                }
                else
                {
                    for (int y = this.Position.Y; y > board.Player.Position.Y; y--)
                    {
                        if (board.Fields[this.Position.X, y].Sprite != null)
                            return false;
                    }
                }
            }
            if (Position.Y == board.Player.Position.Y)
            {
                if (this.Position.X < board.Player.Position.X)
                {
                    for (int x = this.Position.X; x < board.Player.Position.Y; x++)
                    {
                        if (board.Fields[x, this.Position.Y].Sprite != null)
                            return false;
                    }
                }
                else
                {
                    for (int x = this.Position.X; x > board.Player.Position.X; x--)
                    {
                        if (board.Fields[x, this.Position.Y].Sprite != null)
                            return false;
                    }
                }
            }
            return true;
        }
    }
}