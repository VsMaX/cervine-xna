using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Charing powerup for Player
    /// </summary>
    public class ChargingPowerUp : PowerUp
    {
        public ChargingPowerUp(Texture2D texture2D, Point position, GameBoard board)
            : base(texture2D, position, board)
        {
            this.Life = 15000;
        }
    }
}
