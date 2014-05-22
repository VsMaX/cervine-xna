using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class MenuSprite
    {
        private string name;
        private Texture2D texture2D;
        private Texture2D texture2DHover;
        private Vector2 position;

        public MenuSprite(string name, Texture2D texture2D, Texture2D texture2DHover, Vector2 position)
        {
            this.name = name;
            this.texture2D = texture2D;
            this.texture2DHover = texture2DHover;
            this.position = position;
        }

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

        public bool IsActiveMenuItem { get; set; }
    }
}
