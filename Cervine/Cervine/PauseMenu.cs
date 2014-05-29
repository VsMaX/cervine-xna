using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    /// <summary>
    /// Pause menu responsible for displaying and managing pause menu while in game
    /// </summary>
    public class PauseMenu : MainMenu
    {
        /// <summary>
        /// Ctor for PauseMenu
        /// </summary>
        /// <param name="spriteManager">SpriteManager used to draw objects from menu</param>
        /// <param name="menuSpriteList">List of menu options that are drawn by PauseMenu</param>
        /// <param name="spriteFont">Font used to draw text</param>
        public PauseMenu(SpriteManager spriteManager, List<MenuSprite> menuSpriteList, SpriteFont spriteFont)
            : base(spriteManager, menuSpriteList, spriteFont)
        {
        }
        /// <summary>
        /// Handles user input while in menu
        /// </summary>
        /// <param name="gameTime">Instance of GameTime class</param>
        /// <param name="clientBounds">Game window width and height</param>
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (Delay == 0)
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
                            _spriteManager.GameState = GameState.Playing;
                            break;
                        case 1:
                            //_spriteManager.SaveGame();
                            break;
                        case 5:
                            _spriteManager.GameState = GameState.MainMenu;
                            break;
                        default:
                            break;
                    }
                }
            }
            Delay = (Delay + 1)%5;
        }
    }
}