using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class Tnt : Bomb
    {
        public bool IsTriggered { get; set; }

        public Tnt(Texture2D tntTexture, Point position, GameBoard board)
            : base(tntTexture, position, board)
        {
            TimeTick = 41;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override bool IsReadyToDetonate
        {
            get { return IsTriggered && TimeTick <= 0; }
        }

        public override void DecreaseTime()
        {
            if (IsTriggered)
            {
                TimeTick--;
            }
        }

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

        public override List<Point> GetAffectedPositions()
        {
            return GetAffectedPositionsLarge();
        }
    }
}
