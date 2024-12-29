using Memory;
using System.Linq;

namespace Makai.Utils
{
    public class Touhou
    {
        private readonly Mem memory = new();
        private readonly Signatures signatures = new();
        private readonly Offsets offsets = new();

        private bool autobomb;
        private bool autocollectItems;
        private bool invulnerability;

        public Touhou()
        {
            memory.OpenProcess("th12.exe");
        }

        public int Score => GetIntFromSignature(signatures.Score, offsets.Score);
        public int HighScore => GetIntFromSignature(signatures.HighScore, offsets.HighScore);
        public int Power => GetIntFromSignature(signatures.Power, offsets.Power);
        public int Graze => GetIntFromSignature(signatures.Graze, offsets.Graze);

        public int UFOSlot1 => GetIntFromSignature(signatures.UFOSlot1, offsets.UFOSlot1);
        public int UFOSlot2 => GetIntFromSignature(signatures.UFOSlot2, offsets.UFOSlot2);
        public int UFOSlot3 => GetIntFromSignature(signatures.UFOSlot3, offsets.UFOSlot3);

        public float Lives =>
            GetFloatFromSignature(signatures.Lives, signatures.LivesPart, offsets.Lives, offsets.LivesPart);
        public float Spellcards =>
            GetFloatFromSignature(signatures.Spellcards, signatures.SpellcardsPart, offsets.Spellcards,
                offsets.SpellcardsPart);

        public bool Autobomb
        {
            get => autobomb;
            set
            {
                autobomb = value;
                ModifyFirstByte(signatures.Autobomb, value ? (byte)0xC6 : (byte)0xF6);
            }
        }
        public bool AutocollectItems
        {
            get => autocollectItems;
            set
            {
                autocollectItems = value;
                ModifyFirstByte(signatures.AutocollectItems, value ? (byte)0x00 : (byte)0x0E);
            }
        }
        public bool Invulnerability
        {
            get => invulnerability;
            set
            {
                invulnerability = value;
                ModifyFirstByte(signatures.Invulnerability, value ? (byte)0xC3 : (byte)0x83);
            }
        }

        private int GetIntFromSignature(string signature, uint offset)
        {
            long codeAddr = memory.AoBScan(signature).Result.FirstOrDefault();
            string memPointer = (codeAddr + offset).ToString("X");
            string memAddr = memory.ReadInt(memPointer).ToString("X");
            int value = memory.ReadInt(memAddr);
            return value;
        }
        private float GetFloatFromSignature(string signature1, string signature2, uint offset1, uint offset2)
        {
            int p1 = GetIntFromSignature(signature1, offset1);
            int p2 = GetIntFromSignature(signature2, offset2);

            return float.Parse($"{p1}.{p2}");
        }
        private void ModifyFirstByte(string signature, byte mod)
        {
            long codeAddr = memory.AoBScan(signature).Result.FirstOrDefault();
            string memAddr = codeAddr.ToString("X");
            int numOfBytes = signature.Split(" ").Length;
            byte[] bytes = memory.ReadBytes(memAddr, numOfBytes);
            bytes[0] = mod;

            memory.WriteBytes(memAddr, bytes);
        }
    }
}
