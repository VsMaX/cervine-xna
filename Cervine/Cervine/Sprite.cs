using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Sprite class representing any object that can be drawn and acted upon on game
    /// </summary>
    public abstract class Sprite
    {
        /// <summary>
        /// Sprite's texture image used to draw on screen
        /// </summary>
        public virtual Texture2D TextureImage { get; set; }
        /// <summary>
        /// Delay of input
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// Life of sprite
        /// </summary>
        public int Life { get; set; }

        /// <summary>
        /// Sprite position on board
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// GameBoard of current game
        /// </summary>
        protected GameBoard board;
        /// <summary>
        /// Returns current position of sprite on board
        /// </summary>
        /// <returns></returns>
        public Point GetPosition()
        {
            return Position;
        }
        /// <summary>
        /// Ctor of abstract class Sprite
        /// </summary>
        /// <param name="texture2D">Texture image used to render sprite on screen</param>
        /// <param name="position">Initial position of sprite</param>
        /// <param name="board">GameBoard of current game</param>
        public Sprite(Texture2D texture2D, Point position, GameBoard board)
        {
            this.board = board;
            this.TextureImage = texture2D;
            this.Position = position;
            this.Life = 1;
        }
        /// <summary>
        /// Position of sprite from last update
        /// </summary>
        protected Point lastPosition;

        /// <summary>
        /// Abstract definition of direction property
        /// </summary>
        public virtual Point Direction
        {
            get
            {
                return new Point();
            }
        }
        /// <summary>
        /// Calculates all moves and actions made for sprite
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            
        }
        /// <summary>
        /// Resets sprite to initial value
        /// </summary>
        public virtual void Reset()
        {

        }
        /// <summary>
        /// Decreases sprite's life by one
        /// </summary>
        public virtual void DecreaseLife()
        {
            Life--;
        }
        /// <summary>
        /// Returns extended area of neighbouring positions
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Returns closest neighbouring area of positions
        /// </summary>
        /// <returns></returns>
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