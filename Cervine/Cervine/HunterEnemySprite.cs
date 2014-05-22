using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class HunterEnemySprite : Sprite
    {
        public HunterEnemySprite(Texture2D textureImage, Point position, GameBoard board) : base(textureImage, position, board)
        {

        }

        public override Point direction
        {
            get { throw new NotImplementedException(); }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
