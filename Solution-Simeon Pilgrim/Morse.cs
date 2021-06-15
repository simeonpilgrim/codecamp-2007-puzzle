using System;
using System.Collections.Generic;
using System.Text;

namespace BootCamp
{
    static class Morse
    {
        public static Dictionary<char, bool[]> GetLetterCodes()
        {
            Dictionary<char, bool[]> codes = new Dictionary<char, bool[]>();
            codes.Add('A', MorseToCode(".-"));
            codes.Add('N', MorseToCode("-."));
            codes.Add('B', MorseToCode("-..."));
            codes.Add('O', MorseToCode("---"));
            codes.Add('C', MorseToCode("-.-."));
            codes.Add('P', MorseToCode(".--."));
            codes.Add('D', MorseToCode("-.."));
            codes.Add('Q', MorseToCode("--.-"));
            codes.Add('E', MorseToCode("."));
            codes.Add('R', MorseToCode(".-."));
            codes.Add('F', MorseToCode("..-."));
            codes.Add('S', MorseToCode("..."));
            codes.Add('G', MorseToCode("--."));
            codes.Add('T', MorseToCode("-"));
            codes.Add('H', MorseToCode("...."));
            codes.Add('U', MorseToCode("..-"));
            codes.Add('I', MorseToCode(".."));
            codes.Add('V', MorseToCode("...-"));
            codes.Add('J', MorseToCode(".---"));
            codes.Add('W', MorseToCode(".--"));
            codes.Add('K', MorseToCode("-.-"));
            codes.Add('X', MorseToCode("-..-"));
            codes.Add('L', MorseToCode(".-.."));
            codes.Add('Y', MorseToCode("-.--"));
            codes.Add('M', MorseToCode("--"));
            codes.Add('Z', MorseToCode("--.."));
            return codes;
        }

        public static bool[] MorseToCode(string p)
        {
            List<bool> code = new List<bool>(p.Length);
            foreach (char ch in p)
            {
                code.Add((ch == '.'));
            }
            return code.ToArray();
        }

        public static bool[] WordToCode(Dictionary<char, bool[]> codes, string word)
        {
            List<bool> code = new List<bool>(word.Length * 4);
            foreach (char ch in word)
            {
                bool[] val;
                if (codes.TryGetValue(ch, out val))
                {
                    code.AddRange(val);
                }
            }

            return code.ToArray();
        }
    }
}
