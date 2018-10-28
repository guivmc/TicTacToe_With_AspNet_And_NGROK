using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApsRedes.Models
{
    public class Match
    {
        public Player p1 { get; set; }
        public Player p2 { get; set; }
        public int[,] board { get; set; }
        public int roundCounter { get; set; } = 0;
        public bool turn { get; set; } = true;
    }
}