using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cervine
{
    /// <summary>
    /// Main menu class responsible to drawing menus
    /// </summary>
    public class MainMenu
    {
        private int selectedEntry;

        protected static readonly Keys MenuDownKey = Keys.Down;
        protected static readonly Keys MenuUpKey = Keys.Up;
        protected static readonly Keys MenuEnterKey = Keys.Enter;

        protected int activeMenuIndex;
        protected int maxMenuIndex;
        protected List<MenuSprite> menuSprites;
        protected SpriteManager _spriteManager;
        private SpriteFont _spriteFont;
        /// <summary>
        /// Ctor for MainMenu
        /// </summary>
        /// <param name="spriteManager">Instance of SpriteManager class used to draw objects on device</param>
        /// <param name="menuSpriteList">List of menu positions in form of sprites</param>
        /// <param name="spriteFont">Font used to draw text</param>
        public MainMenu(SpriteManager spriteManager, List<MenuSprite> menuSpriteList, SpriteFont spriteFont)
        {
            activeMenuIndex = 0;
            menuSprites = menuSpriteList;
            menuSprites[activeMenuIndex].IsActiveMenuItem = true;
            maxMenuIndex = menuSprites.Count;
            _spriteManager = spriteManager;
            _spriteFont = spriteFont;
        }
        /// <summary>
        /// Updates menu position and handles all user input within menu
        /// </summary>
        /// <param name="gameTime">Instance of GameTime class</param>
        /// <param name="clientBounds">Drawing window game borders in pixels</param>
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
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
                            _spriteManager.NewGame();
                            _spriteManager.GameState = GameState.Playing;
                            break;
                        case 1:
                            _spriteManager.gameBoard = LoadGame();
                            _spriteManager.GameState = GameState.Playing;
                            break;
                        case 2:
                            _spriteManager.GameState = GameState.GameOver;
                            break;
                        default:
                            break;
                    }
                }
            }
            Delay = (Delay + 1)%5;
        }
        /// <summary>
        /// Loads game from previously saved state
        /// </summary>
        /// <returns></returns>
        private GameBoard LoadGame()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("savegame.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            GameBoard board = (GameBoard)formatter.Deserialize(stream);
            stream.Close();
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delay that is used to slow down motion of user input
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// Draws all sprites and menu options on device screen
        /// </summary>
        /// <param name="gameTime">Instance of GameTime class</param>
        /// <param name="spriteBatch">SpriteBatch used to draw objects</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, "CERVINE", new Vector2(250, 100), Color.White);

            foreach (var menuSprite in menuSprites)
            {
                menuSprite.Draw(gameTime, spriteBatch);
            }
        }
    }
}