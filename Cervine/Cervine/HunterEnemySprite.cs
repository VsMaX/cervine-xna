using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cervine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cervine
{
    public class HunterEnemySprite : EnemySprite
    {
        public HunterEnemySprite(Texture2D textureImage, Point position, GameBoard board) : base(textureImage, position, board)
        {
            timeBeforeDirectionChange = 5000;
            this.Life = 1;
        }

        public override Point direction
        {
            get { throw new NotImplementedException(); }
        }

        public override void Update(GameTime gameTime)
        {
            if (Delay == 0)
            {
                var oldPosition = Position;
                if (IsPlayerInRange)
                {
                    var astar = new AGwiazdka(board);
                    var path = astar.FindPath(board.Fields[this.Position.X, this.Position.Y],
                        board.Fields[board.Player.Position.X, board.Player.Position.Y]);
                    Field lastPoint = null;
                    if(path != null)
                        lastPoint = path.LastOrDefault();
                    if (lastPoint != null)
                    {
                        Position = new Point(lastPoint.X, lastPoint.Y);
                        board.ChangePosition(oldPosition, this);
                    }
                }
                else
                {
                    timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
                    var newPosition = Position;
                    if (timeElapsed >= timeBeforeDirectionChange)
                    {
                        timeElapsed = 0;
                        ChangeDirection();
                    }
                    newPosition = new Point(newPosition.X + _direction.X, newPosition.Y + _direction.Y);
                    newPosition = board.AdjustToBoardSize(newPosition);
                    if (board.IsPositionValidExceptPlayer(newPosition))
                    {
                        Position = newPosition;
                        board.ChangePosition(oldPosition, this);
                    }
                    else
                    {
                        ChangeDirection();
                    }
                }
            }
            Delay = (Delay + 1) % 20;
            
            base.Update(gameTime);
        }

        public bool IsPlayerInRange
        {
            get { return AGwiazdka.ManhattanHeuristic(board.Fields[this.Position.X, this.Position.Y],
                board.Fields[board.Player.Position.X,board.Player.Position.Y]) < 10; }
        }
    }
}