using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cervine
{
    /// <summary>
    /// Score class responsible for holding score data for after one game
    /// </summary>
    public class Score
    {
        public Score()
        {
            Name = "";
            Time = 0;
        }
        /// <summary>
        /// Players name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Players game time
        /// </summary>
        public int Time { get; set; }
    }
}
