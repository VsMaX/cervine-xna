using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// Destroyable wall sprite
    /// </summary>
    public class DestroyableWallSprite : Sprite
    {
        /// <summary>
        /// Ctor for destroyable wall sprite
        /// </summary>
        /// <param name="textureImage"></param>
        /// <param name="position"></param>
        /// <param name="gameBoard"></param>
        public DestroyableWallSprite(Texture2D textureImage, Point position, GameBoard gameBoard) : base(textureImage, position, gameBoard)
        {
        }
        /// <summary>
        /// Overriden update function that keeps DestroyableWallSprite in place
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
        }
    }
}
