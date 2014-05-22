using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cervine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class NormalEnemySprite : EnemySprite
    {
        private Random r;
        public NormalEnemySprite(Texture2D textureImage, Point position, GameBoard gameBoard)
            : base(textureImage, position, gameBoard)
        {
            r = new Random();
            timeBeforeDirectionChange = 5000;
            this.Life = 2;
        }

        private double timeElapsed;
        private double timeBeforeDirectionChange;
        private Point _direction;

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            var newPosition = Position;
            if (timeElapsed >= timeBeforeDirectionChange)
            {
                timeElapsed = 0;
                if (_direction.X == 0)
                {
                    if (_direction.Y == 1)
                    {
                        _direction = new Point(1, 0);
                    }
                    else //_direction.Y == -1
                    {
                        _direction = new Point(-1, 0);
                    }
                }
                else
                {
                    if (_direction.X == 1)
                    {
                        _direction = new Point(0, 1);
                    }
                    else //_direction.X == -1
                    {
                        _direction = new Point(0, -1);
                    }
                }
            }
            if (Delay == 0)
            {
                newPosition = new Point(newPosition.X + _direction.X, newPosition.Y + _direction.Y);
                var oldPosition = Position;
                newPosition = board.AdjustToBoardSize(newPosition);
                if (board.IsPositionValid(newPosition))
                {
                    Position = newPosition;
                    board.ChangePosition(oldPosition, this);
                }
            }
            Delay = (Delay + 1)%5;
            
            base.Update(gameTime);
        }
    }
}