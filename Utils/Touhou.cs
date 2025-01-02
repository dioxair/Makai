using Memory;
using MsBox.Avalonia.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tmds.DBus.Protocol;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Makai.Utils
{
    public class Touhou
    {
        private readonly Mem memory = new();
        private readonly Signatures signatures = new();
        private readonly Offsets offsets = new();

        private readonly Dictionary<string, long> cachedMemAddresses = new();

        private bool autobomb;
        private bool autocollectItems;
        private bool invulnerability;

        public Touhou()
        {
            memory.OpenProcess("th12.exe");
        }

        public enum UFOColor
        {
            None,
            Red,
            Blue,
            Green
        }

        public int Score
        {
            get => GetIntFromSignature(signatures.Score, offsets.Score);
            set => SetIntFromSignature(signatures.Score, offsets.Score, value.ToString());
        }
        public int HighScore
        {
            get => GetIntFromSignature(signatures.HighScore, offsets.HighScore);
            set => SetIntFromSignature(signatures.HighScore, offsets.HighScore, value.ToString());
        }
        public int Graze
        {
            get => GetIntFromSignature(signatures.Graze, offsets.Graze);
            set => SetIntFromSignature(signatures.Graze, offsets.Graze, value.ToString());
        }

        public int UFOSlot1
        {
            get => GetIntFromSignature(signatures.UFOSlot1, offsets.UFOSlot1);
            set => SetIntFromSignature(signatures.UFOSlot1, offsets.UFOSlot1, value.ToString());
        }
        public int UFOSlot2
        {
            get => GetIntFromSignature(signatures.UFOSlot2, offsets.UFOSlot2);
            set => SetIntFromSignature(signatures.UFOSlot2, offsets.UFOSlot2, value.ToString());
        }
        public int UFOSlot3
        {
            get => GetIntFromSignature(signatures.UFOSlot3, offsets.UFOSlot3);
            set => SetIntFromSignature(signatures.UFOSlot3, offsets.UFOSlot3, value.ToString());
        }

        public float Lives
        {
            get => GetFloatFromSignature(signatures.Lives, signatures.LivesPart, offsets.Lives, offsets.LivesPart);
            set => SetFloatFromSignature(signatures.Lives, signatures.LivesPart, offsets.Lives, offsets.LivesPart,
                value);
        }
        public float Spellcards
        {
            get => GetFloatFromSignature(signatures.Spellcards, signatures.SpellcardsPart, offsets.Spellcards,
                offsets.SpellcardsPart);
            set => SetFloatFromSignature(signatures.Spellcards, signatures.SpellcardsPart, offsets.Spellcards,
                offsets.SpellcardsPart, value);
        }
        public float Power
        {
            get => GetIntFromSignature(signatures.Power, offsets.Power) / 100f;
            set => SetIntFromSignature(signatures.Power, offsets.Power, Math.Round(value * 100f).ToString());
        }

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

        public void DisableGraze() => memory.FreezeValue(GetMemoryAddress(signatures.Graze, offsets.Graze), "int", "0");
        public void EnableGraze() => memory.UnfreezeValue(GetMemoryAddress(signatures.Graze, offsets.Graze));

        private long GetCachedAddress(string signature)
        {
            if (!cachedMemAddresses.TryGetValue(signature, out long address))
            {
                address = memory.AoBScan(signature).Result.FirstOrDefault();
                cachedMemAddresses[signature] = address;
            }
            return address;
        }

        private bool SetIntFromSignature(string signature, uint offset, string value) =>
            memory.WriteMemory(GetMemoryAddress(signature, offset), "int", value);
        private bool SetFloatFromSignature(string signature1, string signature2, uint offset1, uint offset2,
            float value)
        {
            if (!float.TryParse(value.ToString(CultureInfo.InvariantCulture), out float floatValue))
                return false;

            int integerPart = (int)floatValue;
            int fractionalPart = (int)((floatValue - integerPart) *
                                       Math.Pow(10, CountDecimalPlaces(value.ToString(CultureInfo.InvariantCulture))));

            string memAddr1 = GetMemoryAddress(signature1, offset1);
            string memAddr2 = GetMemoryAddress(signature2, offset2);

            bool writeSuccess1 = memory.WriteMemory(memAddr1, "int", integerPart.ToString());
            bool writeSuccess2 = memory.WriteMemory(memAddr2, "int", fractionalPart.ToString());

            return writeSuccess1 && writeSuccess2;
        }

        private int GetIntFromSignature(string signature, uint offset) =>
            memory.ReadInt(GetMemoryAddress(signature, offset));
        private float GetFloatFromSignature(string signature1, string signature2, uint offset1, uint offset2) =>
            float.Parse($"{GetIntFromSignature(signature1, offset1)}.{GetIntFromSignature(signature2, offset2)}");
        private void ModifyFirstByte(string signature, byte mod)
        {
            long codeAddr = GetCachedAddress(signature);
            string memAddr = codeAddr.ToString("X");
            int numOfBytes = signature.Split(" ").Length;
            byte[] bytes = memory.ReadBytes(memAddr, numOfBytes);
            bytes[0] = mod;

            memory.WriteBytes(memAddr, bytes);
        }

        private string GetMemoryAddress(string signature, uint offset)
        {
            long codeAddr = GetCachedAddress(signature);
            string pointer = (codeAddr + offset).ToString("X");
            return memory.ReadInt(pointer).ToString("X");
        }
        private int CountDecimalPlaces(string number)
        {
            int decimalIndex = number.IndexOf('.');
            if (decimalIndex == -1) return 0;
            return number.Length - decimalIndex - 1;
        }
    }
}
