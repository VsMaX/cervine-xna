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
    /// <summary>
    /// Normal Enemy Sprite class
    /// </summary>
    public class NormalEnemySprite : EnemySprite
    {
        /// <summary>
        /// Ctor for NormalEnemySprite
        /// </summary>
        /// <param name="textureImage">Texture image of enemy sprite</param>
        /// <param name="position">Initial position of enemy</param>
        /// <param name="gameBoard">Instance of GameBoard on which current EnemySprite is be placed</param>
        public NormalEnemySprite(Texture2D textureImage, Point position, GameBoard gameBoard)
            : base(textureImage, position, gameBoard)
        {
            timeBeforeDirectionChange = 2000;
            this.Life = 2;
        }

        private double timeElapsed;
        private double timeBeforeDirectionChange;
        /// <summary>
        /// Updates enemy's moves
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Delay == 0)
            {
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
            base.Update(gameTime);
            Delay = (Delay + 1)%20;
        }
    }
}