using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cervine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class TankMonsterEnemySprite : EnemySprite
    {
        public TankMonsterEnemySprite(Texture2D tankEnemyTexture, Point position, GameBoard gameBoard)
            : base(tankEnemyTexture, position, gameBoard)
        {
            Life = 3;
        }

        public override void Update(GameTime gameTime)
        {
            if (Delay == 0)
            {
                var astar = new AGwiazdka(board);
                var path = astar.FindPath(board.Fields[this.Position.X, this.Position.Y],
                    board.Fields[board.Player.Position.X, board.Player.Position.Y]);
                Field lastPoint = null;
                if (path != null)
                    lastPoint = path.LastOrDefault();
                if (lastPoint != null)
                {
                    var oldPosition = Position;
                    Position = new Point(lastPoint.X, lastPoint.Y);
                    board.ChangePosition(oldPosition, this);
                }
                    
                base.Update(gameTime);
            }
            Delay = (Delay + 1)%25;
            base.Update(gameTime);
        }
    }
}