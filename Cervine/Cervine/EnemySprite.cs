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
            if (Delay2 == 0)
            {
                if (this.Position == board.Player.Position)
                {
                    board.Player.DecreaseLife();
                }
            }
            Delay2 = (Delay2 + 1)%20;
        }

        public int Delay2 { get; set; }

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