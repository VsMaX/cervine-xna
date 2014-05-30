using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            ResetAGwiazdka();
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
                var positions = gameBoard.GetAdjacentPositionsForAStar(unit).ToList();
                foreach (var position in positions)
                {
                    if (!position.Closed)
                    {
                        var distance = unit.G + 1;
                        if (!position.Opened || distance < position.G)
                        {
                            position.G = distance;
                            position.H = DistanceFields(position, end);
                            position.Priority = position.G + position.H;
                            position.Parent = unit;
                            if (!position.Opened)
                            {
                                openList.Enqueue(position, position.Priority);
                                position.Opened = true;
                            }
                            else
                                openList.UpdatePriority(position, position.Priority);
                        }
                    }
                }
            }
            return null;
        }
        
        private void ResetAGwiazdka()
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

        public static double DistanceFields(Field currentUnit, Field goalUnit)
        {
            return Math.Abs(currentUnit.X - goalUnit.X) + Math.Abs(currentUnit.Y - goalUnit.Y);
        }
    }
}
