using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class Bomb : Sprite
    {
        public Bomb(Texture2D texture2D, Point position, GameBoard board)
            : base(texture2D, position, board)
        {
            TimeTick = 240;
        }

        public override Point direction
        {
            get { throw new NotImplementedException(); }
        }

        public int TimeTick { get; set; }

        public virtual bool IsReadyToDetonate
        {
            get { return TimeTick <= 0; }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void DecreaseLife()
        {
            this.TimeTick = 4;
        }

        public virtual void DecreaseTime()
        {
            TimeTick--;
        }

        public virtual void Detonate()
        {
            var positions = GetAffectedPositionsSmall();
            foreach (var position in positions)
            {
                for (int i = board.Drawables.Count - 1; i >= 0; i--)
                {
                    var drawable = board.Drawables[i];
                    if (drawable.Position.X == position.X && drawable.Position.Y == position.Y)
                    {
                        drawable.DecreaseLife();
                    }
                }
            }
            board.Fields[Position.X, Position.Y].Bomb = null;
            board.Bombs.Remove(this);
        }

        public virtual List<Point> GetAffectedPositions()
        {
            return GetAffectedPositionsSmall();
        }
    }
}