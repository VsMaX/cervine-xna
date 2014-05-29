using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Medpack PowerUp
    /// </summary>
    public class MedpackPowerUp : PowerUp
    {
        public MedpackPowerUp(Texture2D texture2D, Point position, GameBoard board)
            : base(texture2D, position, board)
        {
        }
    }
}
