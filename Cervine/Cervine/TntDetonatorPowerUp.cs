using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class TntDetonatorPowerUp : PowerUp
    {
        private Texture2D _tntTexture;

        public TntDetonatorPowerUp(Texture2D texture2D, Point position, GameBoard board,
            Texture2D tntTexture)
            : base(texture2D, position, board)
        {
            TextureImage = texture2D;
            _tntTexture = tntTexture;
        }

        public bool IsPlanted { get; set; }

        public void Detonate()
        {
            Tnt.IsTriggered = true;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void Plant(Point position)
        {
            if (board.IsPositionValidExceptPlayer(position))
            {
                this.Position = position;
                var tnt = new Tnt(_tntTexture, position, board);
                IsPlanted = true;
                this.Tnt = tnt;
                board.Plant(tnt);
            }
        }

        public Tnt Tnt { get; set; }
    }
}
