using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine.Content
{
    public class EnemySprite : Sprite
    {
        protected double timeElapsed;
        protected double timeBeforeDirectionChange;

        public EnemySprite(Texture2D texture2D, Point position, GameBoard board) : base(texture2D, position, board)
        {
        }

        public override Point direction
        {
            get { throw new NotImplementedException(); }
        }

        public override void Update(GameTime gameTime)
        {
            if (board.Fields[Position.X, Position.Y].Sprite == board.Player)
            {
                board.Player.DecreaseLife();
            }
        }

        protected Point _direction;

        protected void ChangeDirection()
        {
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
    }
}