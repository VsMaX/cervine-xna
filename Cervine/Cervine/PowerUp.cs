using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// PowerUp base class used by all PowerUps held by user
    /// </summary>
    public class PowerUp : Sprite
    {
        /// <summary>
        /// Base ctor for PowerUp
        /// </summary>
        /// <param name="texture2D"></param>
        /// <param name="position"></param>
        /// <param name="board"></param>
        public PowerUp(Texture2D texture2D, Point position, GameBoard board)
            : base(texture2D, position, board)
        {
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
