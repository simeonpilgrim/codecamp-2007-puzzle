using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace BootCamp
{
    class Pair
    {
        int i;
        string s;
        internal Pair(int t, string u){ i = t; s = u;}
        internal int I { get { return i; } }
        internal string S { get { return s; } }
    }

    class RadixTree
    {
        class TreeNode
        {
            internal TreeNode(int d) { depth = d; }

            int depth;
            internal int Depth { get { return depth; } }

            TreeNode True = null;
            TreeNode False = null;
            List<string> words = new List<string>();

            internal TreeNode AddNode(bool b)
            {
                if (b)
                {
                    if (True == null)
                    {
                        True = new TreeNode(depth + 1);
                    }
                    return True;
                }
                else
                {
                    if (False == null)
                    {
                        False = new TreeNode(depth + 1);
                    }
                    return False;
                }
            }

            internal void AddWord(string w)
            {
                words.Add(w);
            }

            internal TreeNode Step(bool b)
            {
                if (b)
                {
                    return True;
                }
                else
                {
                    return False;
                }
            }

            internal IEnumerable<string> Words()
            {
                return words;
            }
        }

        TreeNode root = new TreeNode(0);

        internal void AddWord(bool[] path, string word)
        {
            TreeNode tn = root;

            foreach (bool b in path)
            {
                tn = tn.AddNode(b);
            }

            tn.AddWord(word);
        }

        internal IEnumerable<Pair> Words(bool[] path)
        {
            TreeNode tn = root;

            foreach (bool b in path)
            {
                tn = tn.Step(b);
                if (tn != null)
                {
                    foreach(string s in tn.Words())
                    {
                        yield return new Pair(tn.Depth,s);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

 

}
