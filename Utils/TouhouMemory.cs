using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makai.Utils
{
    public class Signatures
    {
        public string Autobomb => "f6 05 ? ? ? ? ? 0f 84 ? ? ? ? 6a ? 8d 87";
        public string Invulnerability => "83 ec ? 8b 0d ? ? ? ? c7 86";
        public string AutocollectItems => "0e 8b 89";
        public string Score => "e9 ? ? ? ? 01 00 00 01";
        public string HighScore => "08 f3";
    }

    // cheat engine can't disassemble these instructions for some reason so I'll just put the offsets.
    public class Offsets
    {
        public string Lives => "004B0C98";
        public string LivesPart => "004B0C9C";
        public string Spellcards => "004B0CA0";
        public string SpellcardsPart => "004B0CA4";
        public string Power => "004B0C48";
        public string Graze => "004B0CDC";
    }
}
