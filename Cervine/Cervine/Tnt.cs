using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Sophisticated bomb that can be detonated remotely
    /// </summary>
    public class Tnt : Bomb
    {
        /// <summary>
        /// True if bomb is set to be detonated, false otherwise
        /// </summary>
        public bool IsTriggered { get; set; }
        /// <summary>
        /// Ctor for Tnt
        /// </summary>
        /// <param name="tntTexture">Texture of tnt to be planted</param>
        /// <param name="position">Initial position on board</param>
        /// <param name="board">GameBoard that the Tnt is planted on</param>
        public Tnt(Texture2D tntTexture, Point position, GameBoard board)
            : base(tntTexture, position, board)
        {
            TimeTick = 41;
        }

        public override void Update(GameTime gameTime)
        {

        }
        /// <summary>
        /// Returns true if bomb has been triggered to detonation and detonation animation has completed
        /// </summary>
        public override bool IsReadyToDetonate
        {
            get { return IsTriggered && TimeTick <= 0; }
        }
        /// <summary>
        /// Decsreases time of detonation animation
        /// </summary>
        public override void DecreaseTime()
        {
            if (IsTriggered)
            {
                TimeTick--;
            }
        }
        /// <summary>
        /// Detonated bomb and decreases life of everyone in the range of detonation
        /// </summary>
        public override void Detonate()
        {
            var positions = GetAffectedPositions();
            foreach (var position in positions)
            {
                foreach (var drawable in board.Drawables.Where(drawable => drawable.Position == position))
                {
                    drawable.DecreaseLife();
                    drawable.DecreaseLife(); // 2 pkt obrazen
                }
            }
            board.Bombs.Remove(this);
            board.Fields[Position.X, Position.Y].Bomb = null;
        }
        /// <summary>
        /// Returns positions that will be affected by detonation
        /// </summary>
        /// <returns></returns>
        public override List<Point> GetAffectedPositions()
        {
            return GetAffectedPositionsLarge();
        }
    }
}
