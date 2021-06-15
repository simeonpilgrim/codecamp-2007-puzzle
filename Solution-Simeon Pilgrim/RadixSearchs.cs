using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace BootCamp
{
    class RadixSearchs
    {
        internal static void CountTESpaceSearch(string textmorse)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            int tcount = 5;
            int ecount = 4;
            int wordcount = 10;
            RadixTree tree = BuildRadixTree();

            int sl = textmorse.Length;
            codes = new bool[wordcount+1][];
            codes[wordcount] = Morse.MorseToCode(textmorse);
            codeslen = new int[wordcount + 1];
            codeslen[wordcount] = sl;
            for (int i = 0; i < wordcount; i++)
            {
                codes[i] = new bool[sl];
                codeslen[i] = 0;
            }

            //sw.Stop();
            //System.Console.WriteLine(sw.ElapsedMilliseconds);
            //sw.Reset();
            //sw.Start();

            foreach (string s in CountTESpaceLines(tree, "", wordcount, tcount, ecount))
            {
                Console.WriteLine(s);
            }

            //sw.Stop();
            //System.Console.WriteLine(sw.ElapsedMilliseconds);

        }

        static bool[][] codes;
        static int[] codeslen;

        static IEnumerable<string> CountTESpaceLines(RadixTree tree, string last, int wordsleft, int countTleft, int countEleft)
        {
            foreach (Pair p in tree.Words(codes[wordsleft]))
            {
                if (p.S != last)
                {
                    int tcount = 0;
                    int ecount = 0;
                    foreach (char ch in p.S)
                    {
                        if (ch == 'T')
                            tcount++;
                        if (ch == 'E')
                            ecount++;
                    }

                    if (p.I == codeslen[wordsleft])
                    {
                        if (wordsleft == 1 && (countTleft - tcount == 0) && (countEleft - ecount == 0))
                        {
                            yield return p.S;
                        }
                    }
                    else if (wordsleft > 1 && (countTleft - tcount >= 0) && (countEleft - ecount >= 0))
                    {
                        int newlen = codeslen[wordsleft] - p.I;
                        codeslen[wordsleft - 1] = newlen;

                        Array.Copy(codes[wordsleft], p.I, codes[wordsleft-1], 0, newlen);
                        foreach (string s in CountTESpaceLines(tree, p.S, wordsleft - 1, countTleft - tcount, countEleft - ecount))
                        {
                            yield return p.S + " " + s;
                        }
                    }
                }
            }
        }

        private static RadixTree BuildRadixTree()
        {
            Dictionary<char, bool[]> codes = Morse.GetLetterCodes();

            RadixTree rt = new RadixTree();
            AddFile(rt, codes, @"c:\military_words.txt");

            return rt;
        }

        private static void AddFile(RadixTree rt, Dictionary<char, bool[]> codes, string p)
        {
            using (StreamReader sr = new StreamReader(p))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    rt.AddWord(Morse.WordToCode(codes, line), line);
                }
            }
        }
    }

}
