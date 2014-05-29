using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Bomb class
    /// </summary>
    public class Bomb : Sprite
    {
        /// <summary>
        /// Ctor for bomb
        /// </summary>
        /// <param name="texture2D">Bomb display texture</param>
        /// <param name="position">Bomb's position on board</param>
        /// <param name="board">GameBoard on which bomb is planted</param>
        public Bomb(Texture2D texture2D, Point position, GameBoard board)
            : base(texture2D, position, board)
        {
            TimeTick = 240;
        }
        /// <summary>
        /// Direction
        /// </summary>
        public override Point Direction
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
             var positions = GetAffectedPositions();

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
                for (int i = 0; i < board.Bombs.Count; i++)
                {
                    var bomb = board.Bombs[i];
                    if (bomb.Position.X == position.X && bomb.Position.Y == position.Y)
                    {
                        bomb.DecreaseLife();
                    }
                }
            }
            board.Fields[Position.X, Position.Y].Bomb = null;
            board.Bombs.Remove(this);
        }

        public virtual List<Point> GetAffectedPositions()
        {
            if (board.Player.PowerUp is ChargingPowerUp)
            {
                return GetAffectedPositionsLarge();
            }
            else
            {
                return GetAffectedPositionsSmall();
            }
        }
    }
}