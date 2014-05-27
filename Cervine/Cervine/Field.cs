using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Priority_Queue;

namespace Cervine
{
    public class Field : PriorityQueueNode
    {
        public Field(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Sprite Sprite { get; set; }
        public Sprite Bomb { get; set; }
        public int AfterBomb { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Field Parent { get; set; }
    }
}
