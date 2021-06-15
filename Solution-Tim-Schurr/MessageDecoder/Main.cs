using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MessageDecoder
{
    public partial class Main : Form
    {
        readonly Dictionary<string, Translation> dictionary = new Dictionary<string, Translation>();
        readonly string encodedTransmission = ".-.-...-..-.-.------.--.....-...--.---.-.------...--------.-..---.--...-.---.-..--.-.-.....-.---.-..-----.-.--.-....-..-.........";

        public Main()
        {
            BuildDictionary();
            InitializeComponent();
        }

        private void BuildDictionary()
        {
            // Load the list of military words
            Stream stream = this.GetType().Assembly.GetManifestResourceStream("MessageDecoder.military_words.txt");
            if (stream == null) throw new Exception("Failed to load miltary_words.txt from manifest");

            // Stuff the military words into a list
            StreamReader reader = new StreamReader(stream);

            while (reader.Peek() >= 0) 
            {
                string word = reader.ReadLine();
                string encWord = CodecTools.EngToEnc ( word );

                if (encodedTransmission.Contains(encWord))
                {
                    if ( dictionary.ContainsKey(encWord ) )
                    {
                        // A translation already exists
                        Translation trans = this.dictionary[encWord];
                        trans.Add(word);
                    }
                    else
                    {
                        this.dictionary.Add(encWord, new Translation(word, encWord));
                    }
                }
            }
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            this.DecodedMessageTextBox.Text = "";

            Random random = new Random(1000);

            List<Solution> solutions = new List<Solution>();

            int testSize = 100;

            for (int iterate = 0; iterate < testSize; iterate++)
            {
                solutions.Add(new Solution(encodedTransmission, dictionary, random));
            }

            int i = 0;

            try
            {
                while (i < 100000)
                {
                    for (int j = 0; j < testSize; j++)
                    {
                        Solution sol = solutions[j].Mutate(random);

                        if (sol.Evaluate > solutions[j].Evaluate)
                        {
                            solutions[j] = sol;
                        }
                    }

                    if (i % 100 == 0)
                    {
                        this.DumpBadSolutions(solutions, random);
                    }

                    this.Progress.Value = (int)i / 1000;
                    i++;
                }

                this.DecodedMessageTextBox.Text += "Here's the best 30 guesses:\r\n";

                solutions.Sort(new SolutionComparer());
                for (int j = 0; j < 30; j++)
                {
                    this.DecodedMessageTextBox.Text += solutions[j].ToString() + "\r\n";
                }
            }
            catch (Exception ex)
            {
                this.DecodedMessageTextBox.Text = ex.Message;
            }
        }

        private void DumpBadSolutions(List<Solution> solutions, Random random)
        {
            solutions.Sort(new SolutionComparer());

            int halfway = solutions.Count / 2;

            for (int iterate = halfway; iterate < solutions.Count; iterate++)
            {
                solutions[iterate] = solutions[iterate - halfway].HeavyMutate(random);
            }
        }

    }

    public sealed class Solution
    {
        private int messageLength;
        private string encodedMessage;
        private List<int> indexes = new List<int>();
        private Dictionary<string, Translation> dictionary;

        public Solution(string EncodedMessage, Dictionary<string, Translation> TheDictionary, Random random)
        {
            encodedMessage = EncodedMessage;
            messageLength = EncodedMessage.Length;
            dictionary = TheDictionary;

            indexes.Add(0);

            for (int i = 1; i < 10; i++)
            {
                indexes.Add(random.Next(indexes[i - 1], indexes[i - 1] + 10));
            }

            for (int i = 1; i < indexes.Count; i++)
            {
                if (indexes[i] <= indexes[i - 1]) indexes[i] = indexes[i - 1] + 1;
            }

            for (int i = indexes.Count - 1; i > 1; i--)
            {
                if (indexes[i] > this.messageLength) indexes[i] = this.messageLength;
                if (indexes[i - 1] >= indexes[i]) indexes[i - 1] = indexes[i] - 1;
            }
        }

        private Solution(string EncodedMessage, List<int> Indexes, Dictionary<string, Translation> TheDictionary)
        {
            encodedMessage = EncodedMessage;
            messageLength = EncodedMessage.Length;
            indexes = Indexes;
            dictionary = TheDictionary;
        }

        public IEnumerable<string> Tokens
        {
            get
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    yield return this.GetToken(i);
                }
            }
        }
        private string GetToken(int index)
        {
            if (index == indexes.Count - 1)
            {
                return encodedMessage.Substring(indexes[index]);
            }
            else
            {
                return encodedMessage.Substring(indexes[index], indexes[index + 1] - indexes[index]);
            }
        }
        private int GetTokenLength(int index)
        {
            if (index == indexes.Count - 1)
            {
                return messageLength - indexes[index];
            }
            else
            {
                return indexes[index + 1] - indexes[index];
            }
        }

        public Solution HeavyMutate(Random random)
        {
            List<int> newIndexes = new List<int>(this.indexes);

            for (int i = 1; i < newIndexes.Count; i++)
            {
                newIndexes[i] += random.Next(-15, 15);
            }

            for (int i = 1; i < newIndexes.Count; i++)
            {
                if (newIndexes[i] <= newIndexes[i - 1]) newIndexes[i] = newIndexes[i - 1] + random.Next(1, 5);
            }

            for (int i = newIndexes.Count - 1; i > 1; i--)
            {
                if (newIndexes[i] > this.messageLength) newIndexes[i] = this.messageLength;
                if (newIndexes[i - 1] >= newIndexes[i]) newIndexes[i - 1] = newIndexes[i] - random.Next(1, 5);
            }

            return new Solution(this.encodedMessage, newIndexes, this.dictionary);
        }

        public Solution Mutate(Random random)
        {
            List<int> newIndexes = new List<int>(this.indexes);

            for (int i = 0; i < newIndexes.Count - 1; i++)
            {
                string token = this.GetToken(i);

                if (dictionary.ContainsKey(token) == false)
                {
                    newIndexes[i + 1] += random.Next(-5, 5);
                }
            }

            for (int i = 1; i < newIndexes.Count; i++)
            {
                if (newIndexes[i] <= newIndexes[i - 1]) newIndexes[i] = newIndexes[i - 1] + random.Next(1, 5);
            }

            for (int i = newIndexes.Count - 1; i > 1; i--)
            {
                if (newIndexes[i] > this.messageLength) newIndexes[i] = this.messageLength;
                if (newIndexes[i - 1] >= newIndexes[i]) newIndexes[i - 1] = newIndexes[i] - random.Next(1, 5);
            }

            return new Solution(this.encodedMessage, newIndexes, this.dictionary);
        }

        public int Evaluate
        {
            get
            {
                int ret = 0;
                int countTokensMissing = 0;

                StringBuilder builder = new StringBuilder();

                foreach (string token in Tokens)
                {
                    if (dictionary.ContainsKey(token))
                    {
                        ret += token.Length * 2;
                        builder.Append(dictionary[token].FirstWord);
                    }
                    else
                    {
                        builder.Append(token);
                        ret -= token.Length;
                        countTokensMissing++;
                    }
                }

                if (countTokensMissing == 0)
                {
                    ret += 100;

                    string result = builder.ToString();

                    int numEs = CodecTools.NumberEs(result);
                    int numTs = CodecTools.NumberTs(result);

                    if (numEs == 4 && numTs == 5)
                    {
                        throw new Exception("Found solution: " + this.ToString());
                    }

                    ret += 10 - (Math.Abs(numEs - 4));
                    ret += 10 - (Math.Abs(numTs - 5));
                }

                return ret;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string token in Tokens)
            {
                if (dictionary.ContainsKey(token))
                {
                    builder.Append(dictionary[token].FirstWord);
                }
                else
                {
                    builder.Append(token);
                }
                builder.Append(" ");
            }
            return builder.ToString();
        }
    }

    public class SolutionComparer : IComparer<Solution>
    {
        public int Compare(Solution x, Solution y)
        {
            if (x.Evaluate < y.Evaluate) return 1;
            if (x.Evaluate > y.Evaluate) return -1;
            return 0;
        }
    }

    public class Translation
    {
        readonly private List<string> words = new List<string>();
        readonly private string encWord;

        public Translation(string Word, string EncWord)
        {
            words.Add(Word);
            encWord = EncWord;
        }
        public void Add ( string word )
        {
            words.Add (word);
        }
        public string EncWord
        { get { return encWord; } }
        public IEnumerable<string> Words
        { get { return words; } }
        public string FirstWord
        { get { return words[0]; } }
    }

    public class CodecTools
    {
        public static Dictionary<string, Translation> FilterDictionary(Dictionary<string, Translation> original, string message)
        {
            Dictionary<string, Translation> result = new Dictionary<string,Translation>();

            foreach (Translation translation in original.Values)
            {
                if (message.Contains(translation.EncWord))
                {
                    result.Add(translation.EncWord, translation);
                }
            }

            return result;
        }

        public static string EngToEnc(string theword)
        {
            StringBuilder builder = new StringBuilder();

            foreach ( char character in theword )
            {
                builder.Append(CharToEnc(character));
            }

            return builder.ToString();
        }

        public static bool IsStringDecoded(string theString)
        {
            if (theString.Contains("-")) return false;
            if (theString.Contains(".")) return false;
            return true;
        }

        public static int NumberEs(string theString)
        {
            int ret = 0;

            foreach (char c in theString)
            {
                if (c == 'E') ret++;
            }

            return ret;
        }

        public static int NumberTs(string theString)
        {
            int ret = 0;

            foreach (char c in theString)
            {
                if (c == 'T') ret++;
            }

            return ret;
        }

        private static string CharToEnc(char character)
        {
            switch (character)
            {
                case 'A': return ".-";
                case 'B': return "-...";
                case 'C': return "-.-.";
                case 'D': return "-..";
                case 'E': return ".";
                case 'F': return "..-.";
                case 'G': return "--.";
                case 'H': return "....";
                case 'I': return "..";
                case 'J': return ".---";
                case 'K': return "-.-";
                case 'L': return ".-..";
                case 'M': return "--";
                case 'N': return "-.";
                case 'O': return "---";
                case 'P': return ".--.";
                case 'Q': return "--.-";
                case 'R': return ".-.";
                case 'S': return "...";
                case 'T': return "-";
                case 'U': return "..-";
                case 'V': return "...-";
                case 'W': return ".--";
                case 'X': return "-..-";
                case 'Y': return "-.--";
                case 'Z': return "--..";
                default: throw new Exception("Invalid decoding character");
            }
        }
    }
}