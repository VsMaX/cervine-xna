using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class WallSprite : Sprite
    {
        public WallSprite(Texture2D textureImage, Point position, GameBoard gameBoard) : base(textureImage, position, gameBoard)
        {
        }

        public override Point Direction
        {
            get { return this.Position; }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void DecreaseLife()
        {

        }
    }
}
