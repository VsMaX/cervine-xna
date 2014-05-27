using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cervine.AStar;
using Microsoft.Xna.Framework;
using Priority_Queue;

namespace Cervine
{
    public class AGwiazdka
    {
        private GameBoard gameBoard;

        public AGwiazdka(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        public List<Field> FindPath(Field start, Field end)
        {
            HeapPriorityQueue<Field> openList = new HeapPriorityQueue<Field>((gameBoard.SizeX + 1) * (gameBoard.SizeY + 1));
            ResetPositions();
            start.G = 0;
            start.H = 0;
            start.Priority = 0;
            openList.Enqueue(start, 0);
            start.Opened = true;
            while (openList.Count != 0)
            {
                var unit = openList.Dequeue();
                unit.Closed = true;

                if (unit == end)
                {
                    return BacktracePath(unit);
                }

                var neighbors = gameBoard.GetAdjacentPositionsForAStar(unit).ToList();
                foreach (var neighbor in neighbors)
                {
                    if (neighbor.Closed) continue;

                    var distance = unit.G + 1;
                    if (!neighbor.Opened || distance < neighbor.G)
                    {
                        neighbor.G = distance;
                        neighbor.H = ManhattanHeuristic(neighbor, end);
                        neighbor.Priority = neighbor.G + neighbor.H;
                        neighbor.Parent = unit;
                        if (!neighbor.Opened)
                        {
                            openList.Enqueue(neighbor, neighbor.Priority);
                            neighbor.Opened = true;
                        }
                        else
                            openList.UpdatePriority(neighbor, neighbor.Priority);
                    }
                }
            }
            return null;
        }

        private void ResetPositions()
        {
            for (int i = 0; i < gameBoard.SizeX; i++)
            {
                for (int j = 0; j < gameBoard.SizeY; j++)
                {
                    var field = gameBoard.Fields[i, j];
                    field.G = 0;
                    field.H = 0;
                    field.Closed = false;
                    field.Priority = 0;
                    field.InsertionIndex = 0;
                    field.QueueIndex = 0;
                    field.Opened = false;
                    field.Parent = null;
                }
            }
        }

        public static double ManhattanHeuristic(Field currentUnit, Field goalUnit)
        {
            return Math.Abs(currentUnit.X - goalUnit.X) + Math.Abs(currentUnit.Y - goalUnit.Y);
        }

        private List<Field> BacktracePath(Field unit)
        {
            var list = new List<Field>();
            var node = unit;
            list.Add(node);
            while (node.Parent != null)
            {
                node = node.Parent;
                list.Add(node);
            }
            list.RemoveAt(list.Count - 1);
            return list;
        }
    }
}
