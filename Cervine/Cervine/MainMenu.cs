﻿using System;
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

        protected static readonly Keys MenuDownKey = Keys.Down;
        protected static readonly Keys MenuUpKey = Keys.Up;
        protected static readonly Keys MenuEnterKey = Keys.Enter;

        protected int activeMenuIndex;
        protected int maxMenuIndex;
        protected List<MenuSprite> menuSprites;
        protected SpriteManager _spriteManager;

        public MainMenu(SpriteManager spriteManager, List<MenuSprite> menuSpriteList)
        {
            activeMenuIndex = 0;
            menuSprites = menuSpriteList;
            menuSprites[activeMenuIndex].IsActiveMenuItem = true;
            maxMenuIndex = menuSprites.Count;
            _spriteManager = spriteManager;
        }

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

        public int Delay { get; set; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var menuSprite in menuSprites)
            {
                menuSprite.Draw(gameTime, spriteBatch);
            }
        }
    }
}