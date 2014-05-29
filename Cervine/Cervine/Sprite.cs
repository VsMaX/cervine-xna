using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public abstract class Sprite
    {
        // Stuff needed to draw the sprite
        public virtual Texture2D TextureImage { get; set; }
        public int Delay { get; set; }
        protected Point frameSize;
        protected Point currentFrame;
        protected Point sheetSize;
        public Guid Guid { get; set; }
        // Collision data
        protected int collisionOffset;
        public int Life { get; set; }
        // Framerate stuff
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected const int defaultMillisecondsPerFrame = 16;

        // Movement data
        public Point Position { get; set; }
        protected Rectangle modelSize;
        protected GameBoard board;

        public Point GetPosition()
        {
            return Position;
        }

        public Sprite(Texture2D texture2D, Point position, GameBoard board)
        {
            this.board = board;
            this.TextureImage = texture2D;
            this.Position = position;
            Guid = new Guid();
            this.Life = 1;
        }

        protected Point lastPosition;

        protected float halfSpriteHeight
        {
            get
            {
                return frameSize.Y / 2;
            }
        }

        protected void RollbackMove()
        {
            this.Position = lastPosition;
        }

        // Abstract definition of Direction property
        public virtual Point Direction
        {
            get
            {
                return new Point();
            }
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Reset()
        {

        }

        public virtual void DecreaseLife()
        {
            Life--;
        }

        public List<Point> GetAffectedPositionsLarge()
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

        public List<Point> GetAffectedPositionsSmall()
        {
            var affectedPositions = new List<Point>();
            int x = Position.X;
            int y = Position.Y;

            var pos1 = board.AdjustToBoardSize(new Point(x + 1, y));
            var pos2 = board.AdjustToBoardSize(new Point(x - 1, y));
            var pos3 = board.AdjustToBoardSize(new Point(x, y + 1));
            var pos4 = board.AdjustToBoardSize(new Point(x, y - 1));

            affectedPositions.Add(pos1);
            affectedPositions.Add(pos2);
            affectedPositions.Add(pos3);
            affectedPositions.Add(pos4);

            if ((board.GetSprite(pos1) is WallSprite))
            {
                affectedPositions.Remove(pos1);
            }
            if ((board.GetSprite(pos2) is WallSprite))
            {
                affectedPositions.Remove(pos2);
            }
            if ((board.GetSprite(pos3) is WallSprite))
            {
                affectedPositions.Remove(pos3);
            }
            if ((board.GetSprite(pos4) is WallSprite))
            {
                affectedPositions.Remove(pos4);
            }

            return affectedPositions;
        }
    }
}