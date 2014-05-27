using System;
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

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public List<Point> GetAffectedPositions()
        {
            var affectedPositions = new List<Point>();
            int x = Position.X;
            int y = Position.Y;
            var position1 = board.AdjustToBoardSize(new Point(x + 1, y));
            var position2 = board.AdjustToBoardSize(new Point(x - 1, y));
            var position3 = board.AdjustToBoardSize(new Point(x, y + 1));
            var position4 = board.AdjustToBoardSize(new Point(x, y - 1));
            var position5 = board.AdjustToBoardSize(new Point(x + 2, y));
            var position6 = board.AdjustToBoardSize(new Point(x - 2, y));
            var position7 = board.AdjustToBoardSize(new Point(x, y + 2));
            var position8 = board.AdjustToBoardSize(new Point(x, y - 2));
            var position9 = board.AdjustToBoardSize(new Point(x + 1, y + 1));
            var position10 = board.AdjustToBoardSize(new Point(x - 1, y - 1));
            var position11 = board.AdjustToBoardSize(new Point(x + 1, y - 1));
            var position12 = board.AdjustToBoardSize(new Point(x - 1, y + 1));

            affectedPositions.Add(position1);
            affectedPositions.Add(position2);
            affectedPositions.Add(position3);
            affectedPositions.Add(position4);
            affectedPositions.Add(position5);
            affectedPositions.Add(position6);
            affectedPositions.Add(position7);
            affectedPositions.Add(position8);
            affectedPositions.Add(position9);
            affectedPositions.Add(position10);
            affectedPositions.Add(position11);
            affectedPositions.Add(position12);

            
            if ((board.GetSprite(position1) is WallSprite))
            {
                affectedPositions.Remove(position1);
                affectedPositions.Remove(position5);
            }
            if ((board.GetSprite(position2) is WallSprite))
            {
                affectedPositions.Remove(position2);
                affectedPositions.Remove(position6);
            }
            if ((board.GetSprite(position3) is WallSprite))
            {
                affectedPositions.Remove(position3);
                affectedPositions.Remove(position7);
            }
            if ((board.GetSprite(position4) is WallSprite))
            {
                affectedPositions.Remove(position4);
                affectedPositions.Remove(position8);
            }
            if ((board.GetSprite(position5) is WallSprite))
            {
                affectedPositions.Remove(position5);
            }
            if ((board.GetSprite(position6) is WallSprite))
            {
                affectedPositions.Remove(position6);
            }
            if ((board.GetSprite(position7) is WallSprite))
            {
                affectedPositions.Remove(position7);
            }
            if ((board.GetSprite(position8) is WallSprite))
            {
                affectedPositions.Remove(position8);
            }
            if ((board.GetSprite(position9) is WallSprite))
            {
                affectedPositions.Remove(position9);
            }
            if ((board.GetSprite(position10) is WallSprite))
            {
                affectedPositions.Remove(position10);
            } 
            if ((board.GetSprite(position11) is WallSprite))
            {
                affectedPositions.Remove(position11);
            } 
            if ((board.GetSprite(position12) is WallSprite))
            {
                affectedPositions.Remove(position12);
            }

            return affectedPositions;
        }

        public override void DecreaseLife()
        {
            this.TimeTick = 5;
        }
    }
}