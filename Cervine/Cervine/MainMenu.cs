using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    public class MainMenu
    {
        private int selectedEntry;

        private static readonly Keys MenuDownKey = Keys.Down;
        private static readonly Keys MenuUpKey = Keys.Up;
        private static readonly Keys MenuEnterKey = Keys.Enter;

        protected int activeMenuIndex;
        protected int maxMenuIndex;
        protected List<MenuSprite> menuSprites;
        protected CervineGame game;

        public MainMenu(CervineGame game, List<MenuSprite> menuSpriteList)
        {
            this.game = game;
            activeMenuIndex = 0;
            menuSprites = menuSpriteList;
            menuSprites[activeMenuIndex].IsActiveMenuItem = true;
            maxMenuIndex = menuSprites.Count;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Contains(MenuDownKey))
            {
                menuSprites[activeMenuIndex].IsActiveMenuItem = false;
                activeMenuIndex = activeMenuIndex >= maxMenuIndex - 1 ? maxMenuIndex - 1 : activeMenuIndex + 1;
                menuSprites[activeMenuIndex].IsActiveMenuItem = true;
            }
            else if (keys.Contains(MenuUpKey))
            {
                menuSprites[activeMenuIndex].IsActiveMenuItem = false;
                activeMenuIndex = activeMenuIndex <= 0 ? 0 : activeMenuIndex - 1;
                menuSprites[activeMenuIndex].IsActiveMenuItem = true;
            }
            else if (keys.Contains(MenuEnterKey))
            {
                switch (activeMenuIndex)
                {
                    case 0:
                        game.GameState = GameState.Playing;
                        break;
                    default:
                        break;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var menuSprite in menuSprites)
            {
                menuSprite.Draw(gameTime, spriteBatch);
            }
        }
    }
}