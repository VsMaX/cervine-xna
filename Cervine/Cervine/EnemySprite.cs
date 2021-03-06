﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine.Content
{
    /// <summary>
    /// EnemySprite class
    /// </summary>
    public class EnemySprite : Sprite
    {
        protected double timeElapsed;
        protected double timeBeforeDirectionChange;
        /// <summary>
        /// Ctor for EnemySprite
        /// </summary>
        /// <param name="texture2D">Enemy sprite texture to render</param>
        /// <param name="position">Initial enemy position</param>
        /// <param name="board">Gameboard on which enemy is placed</param>
        public EnemySprite(Texture2D texture2D, Point position, GameBoard board) : base(texture2D, position, board)
        {
        }
        /// <summary>
        /// Update function that takes action upon enemysprite
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Delay2 == 0)
            {
                if (this.Position == board.Player.Position)
                {
                    board.Player.DecreaseLife();
                }
            }
            Delay2 = (Delay2 + 1)%20;
        }
        /// <summary>
        /// Delay to execute Update method
        /// </summary>
        public int Delay2 { get; set; }

        protected Point _direction;
        /// <summary>
        /// Randomly changes direction in which enemy moves
        /// </summary>
        protected void ChangeDirection()
        {
            Random r = new Random();
            _direction.X = r.Next(1000)%3 - 1;
            if (_direction.X == 0)
            {
                _direction.Y = r.Next(1000)%3 - 1;
            }
            else
            {
                _direction.Y = 0;
            }
        }
    }
}