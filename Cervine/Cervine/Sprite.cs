using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public abstract class Sprite
    {
        // Stuff needed to draw the sprite
        public Texture2D TextureImage { get; set; }
        public int Delay { get; set; }
        protected Point frameSize;
        protected Point currentFrame;
        protected Point sheetSize;
        public Guid Guid { get; set; }
        // Collision data
        protected int collisionOffset;
        public int Life { get; set; }
        // Framerate stuff
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected const int defaultMillisecondsPerFrame = 16;

        // Movement data
        public Point Position { get; set; }
        protected Rectangle modelSize;
        protected GameBoard board;

        public Point GetPosition()
        {
            return Position;
        }

        public Sprite(Texture2D texture2D, Point position, GameBoard board)
        {
            this.board = board;
            this.TextureImage = texture2D;
            this.Position = position;
            Guid = new Guid();
            this.Life = 1;
        }

        protected Point lastPosition;

        protected float halfSpriteHeight
        {
            get
            {
                return frameSize.Y / 2;
            }
        }

        protected void RollbackMove()
        {
            this.Position = lastPosition;
        }

        // Abstract definition of direction property
        public abstract Point direction
        {
            get;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Reset()
        {

        }

        public virtual void DecreaseLife()
        {
            Life--;
        }

    }
}