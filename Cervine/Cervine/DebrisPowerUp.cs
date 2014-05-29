using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class DebrisPowerUp : PowerUp
    {
        private Texture2D _texture1;
        private Texture2D _texture2;
        private Texture2D _texture3;
        private Texture2D _texture4;

        public DebrisPowerUp(Texture2D debris1Texture, Texture2D debris2Texture, Texture2D debris3Texture, Texture2D debris4Texture,
            Point position, GameBoard board)
            : base(debris4Texture, position, board)
        {
            _texture1 = debris1Texture;
            _texture2 = debris2Texture;
            _texture3 = debris3Texture;
            _texture4 = debris4Texture;
            DebrisCount = 4;
        }

        public override Texture2D TextureImage
        {
            get
            {
                switch (DebrisCount)
                {
                    case 0:
                        return _texture4;
                    case 1:
                        return _texture1;
                    case 2:
                        return _texture2;
                    case 3:
                        return _texture3;
                    case 4:
                        return _texture4;
                    default:
                        return _texture4;

                }
            }
        }

        public override Point Direction
        {
            get { throw new NotImplementedException(); }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void IncreaseDebris()
        {
            DebrisCount = Math.Min(DebrisCount + 1, 4);
        }

        public int DebrisCount { get; set; }
    }
}