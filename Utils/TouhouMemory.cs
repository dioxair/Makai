namespace Makai.Utils
{
    public class Signatures
    {
        public string Autobomb => "? 05 ? ? ? ? ? 0f 84 ? ? ? ? 6a ? 8d 87";
        public string AutocollectItems => "? 8b 89 2c 0a 00 00 d9 41 08 e9";
        public string Invulnerability => "? ec ? 8b 0d ? ? ? ? c7 86";

        public string Score => "8b 1d ? ? ? ? 56 8b 35 ? ? ? ? 57";
        public string HighScore => "a1 ? ? ? ? 8b 15 ? ? ? ? eb";
        public string Power => "a1 ? ? ? ? 89 54 24 ? 99";
        public string Graze => "8b 0d ? ? ? ? d9 5c 24 ? d9 ee 8b 54 24 ? d9 5c 24 ? 89 54 24";

        public string Lives => "89 1d ? ? ? ? eb ? a1 ? ? ? ? 3b c5";
        public string LivesPart => "89 2d ? ? ? ? 75";
        public string Spellcards => "a1 ? ? ? ? 8b 0d ? ? ? ? 50 51";
        public string SpellcardsPart => "a1 ? ? ? ? 8b 0d ? ? ? ? 50 51";
    }

    public class Offsets
    {
        public uint Score => 0x2;
        public uint HighScore => 0x1;
        public uint Power => 0x01;
        public uint Graze => 0x02;

        public uint Lives => 0x02;
        public uint LivesPart => 0x02;
        public uint Spellcards => 0x07;
        public uint SpellcardsPart => 0x01;
    }
}
