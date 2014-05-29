using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class DestroyableWallSprite : Sprite
    {
        public DestroyableWallSprite(Texture2D textureImage, Point position, GameBoard gameBoard) : base(textureImage, position, gameBoard)
        {
        }

        public override Point Direction
        {
            get { throw new NotImplementedException(); }
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
