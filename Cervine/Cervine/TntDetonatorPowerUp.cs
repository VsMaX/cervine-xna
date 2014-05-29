using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    /// <summary>
    /// PowerUp for tnt detonator
    /// </summary>
    public class TntDetonatorPowerUp : PowerUp
    {
        private Texture2D _tntTexture;
        /// <summary>
        /// Ctor for TntDetonatorPowerUp
        /// </summary>
        /// <param name="texture2D">Texture to display of TntDetonatorPowerUp</param>
        /// <param name="position">Initial position of TntDetonatorPowerUp</param>
        /// <param name="board">GameBoard on which TntDetonatorPowerUp is placed</param>
        /// <param name="tntTexture">Texture of tnt bomb that is detonated</param>
        public TntDetonatorPowerUp(Texture2D texture2D, Point position, GameBoard board,
            Texture2D tntTexture)
            : base(texture2D, position, board)
        {
            TextureImage = texture2D;
            _tntTexture = tntTexture;
        }
        /// <summary>
        /// True if tnt has been planted on map
        /// </summary>
        public bool IsPlanted { get; set; }
        /// <summary>
        /// Triggers remote detonation
        /// </summary>
        public void Detonate()
        {
            Tnt.IsTriggered = true;
        }

        public override void Update(GameTime gameTime)
        {
        }
        /// <summary>
        /// Plants bomb on map
        /// </summary>
        /// <param name="position"></param>
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
        /// <summary>
        /// Tnt bomb reference that will be planted on map
        /// </summary>
        public Tnt Tnt { get; set; }
    }
}
