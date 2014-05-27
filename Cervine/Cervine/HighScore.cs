using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cervine
{
    public class HighScore
    {
        public HighScore(double totalMilliseconds)
        {
            this.TotalMilliseconds = totalMilliseconds;
        }

        public double TotalMilliseconds { get; set; }
    }
}
