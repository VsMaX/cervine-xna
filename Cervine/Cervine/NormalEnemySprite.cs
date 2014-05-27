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
        public NormalEnemySprite(Texture2D textureImage, Point position, GameBoard gameBoard)
            : base(textureImage, position, gameBoard)
        {
            timeBeforeDirectionChange = 3000;
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
                ChangeDirection();
            }
            if (Delay == 0)
            {
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
            Delay = (Delay + 1)%5;
            
            base.Update(gameTime);
        }
    }
}