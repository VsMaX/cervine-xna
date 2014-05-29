using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class PowerUp : Sprite
    {
        public PowerUp(Texture2D texture2D, Point position, GameBoard board)
            : base(texture2D, position, board)
        {
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
