using Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makai.Utils
{
    public class Touhou
    {
        private Mem memory = new();
        private readonly Signatures signatures = new();

        public float Power;
        public int Score;
        public int HighScore;
        public int Lives;
        public int SpellCards;
        public int Graze;
    }
}
