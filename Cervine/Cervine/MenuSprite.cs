using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// One instance of one menu option
    /// </summary>
    public class MenuSprite
    {
        private string name;
        private Texture2D texture2D;
        private Texture2D texture2DHover;
        private Vector2 position;
        /// <summary>
        /// Ctor for MenuSprite
        /// </summary>
        /// <param name="name">Name of option</param>
        /// <param name="texture2D">Texture of sprite when not checked</param>
        /// <param name="texture2DHover">Texture of sprite when chosen</param>
        /// <param name="position">Position of menu sprite on screen</param>
        public MenuSprite(string name, Texture2D texture2D, Texture2D texture2DHover, Vector2 position)
        {
            this.name = name;
            this.texture2D = texture2D;
            this.texture2DHover = texture2DHover;
            this.position = position;
        }
        /// <summary>
        /// Draws sprite on the device screen
        /// </summary>
        /// <param name="gameTime">Instance of GameTime class used for drawing</param>
        /// <param name="spriteBatch">SpriteBatch to draw on device screen</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActiveMenuItem)
            {
                spriteBatch.Draw(texture2DHover, position, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture2D, position, Color.White);
            }
        }
        /// <summary>
        /// True if current menu item is active one, false otherwise
        /// </summary>
        public bool IsActiveMenuItem { get; set; }
    }
}
