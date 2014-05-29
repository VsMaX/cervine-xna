using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Wall sprite class. Wall cannot be destroyed or moved.
    /// </summary>
    public class WallSprite : Sprite
    {
        /// <summary>
        /// Ctor for WallSprite
        /// </summary>
        /// <param name="textureImage">Texture image used to render WallSprite on map</param>
        /// <param name="position">Position on WallSprite on map</param>
        /// <param name="gameBoard">GameBoard of currnent game on which WallSprite is placed</param>
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
