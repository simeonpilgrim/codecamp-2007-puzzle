using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace MorseDecoder
{
    internal class PartialSolution
    {

        #region private fields

        ArrayList _decrypted;

        string _remainder;

        #endregion

        #region constructors

        internal PartialSolution(string remainder)
        {
            _decrypted = new ArrayList();
            _remainder = remainder;
        }

        internal PartialSolution(ArrayList decrypted, string remainder)
        {
            _decrypted = new ArrayList(decrypted); //new copy of ArrayList
            _remainder = remainder;
        }

        #endregion

        #region attributes

        internal string Solution
        {
            get
            {
                StringBuilder solution = new StringBuilder();
                foreach (string word in _decrypted)
                {
                    if (solution.Length > 0)
                    {
                        solution.Append(@" "); //add space
                    }
                    solution.Append(word);
                }
                return solution.ToString();
            }
        }

        internal string Remainder
        {
            get
            {
                return _remainder;
            }
        }

        internal bool Solved
        {
            get
            {
                return (_remainder.Length == 0);
            }
        }

        internal int WordCount
        {
            get
            {
                return _decrypted.Count;
            }
        }



        #endregion

        #region operations

        internal PartialSolution Clone()
        {
            //TODO: Clone must also clone invocation list for events
            PartialSolution p = new PartialSolution(this._decrypted, this._remainder);
            return p;
        }

        private void AddSolvedWord(KeyValuePair<string, string> theWord)
        {
            //add to decrypted list
            _decrypted.Add(theWord.Key);

            //remove related morse from remainder
            _remainder = Regex.Replace(_remainder, String.Format("^{0}", theWord.Value), string.Empty);

        }

        internal void Solve()
        {
            foreach (KeyValuePair<string, string> word in DecoderUtility.MilitaryDictionary)
            {
                string pattern = String.Format("^{0}", word.Value);
                Regex regex = new Regex(pattern);
                if (regex.IsMatch(this.Remainder))
                {
                    PartialSolution p = this.Clone();
                    p.AddSolvedWord(word); //returns Solved state, but not used for clarity of code
                    if (p.Solved)
                    {
                        if (p.CharacterCountsOK(CharacterCheckTypes.InBounds))
                        {
                        //add to solutions
                        DecoderUtility.CompleteSolutions.Add(p, Solutions.SolutionAddedReason.Solved);
                        }
                    }
                    else
                    {
                        if (p.WordCount < DecoderUtility.SolutionRequiredWordCount)
                        {
                            if (p.CharacterCountsOK(CharacterCheckTypes.InBounds))
                            {
                                p.Solve();

                            }
                            else
                            {
                                DecoderUtility.DiscardedPartialSolutions.Add(p, Solutions.SolutionAddedReason.CharacterCountOutOfRange);
                                //p.Dispose();

                            }
                        }
                        else
                        {
                            DecoderUtility.DiscardedPartialSolutions.Add(p, Solutions.SolutionAddedReason.WordCountOutOfRange);
                            //p.Dispose();
                        }
                    }
                }
            }

        }

        internal int GetCharacterCount(string character)
        {
            Regex regex = new Regex(character);
            return regex.Matches(this.Solution).Count;
        }

        internal enum CharacterCheckTypes
        {
            InBounds, MatchRequirements
        }

        internal bool CharacterCountsOK(CharacterCheckTypes checkType)
        {
            //return false if any character counts exceed DecoderUtilit
            bool result = true;
            foreach (DictionaryEntry de in DecoderUtility.SolutionRequiredCharacterCounts)
            {
                switch (checkType)
                {
                    case CharacterCheckTypes.InBounds:
                        if (this.GetCharacterCount(de.Key.ToString()) > Convert.ToInt32(de.Value))
                        {
                            result = false;
                        }
                        break;
                    case CharacterCheckTypes.MatchRequirements:
                        if (this.GetCharacterCount(de.Key.ToString()) != Convert.ToInt32(de.Value))
                        {
                            result = false;
                        }
                        break;

                }


            }
            return result;
        }

        //TODO: implement IDisposable

        #endregion

        #region events

        public event SolutionFoundHandler PartialSolutionFound;

        private void OnPartialSolutionFound(PartialSolution newSolution)
        {
            if (PartialSolutionFound != null)
            {
                SolutionFoundEventArgs args =
                    new SolutionFoundEventArgs(newSolution);
                PartialSolutionFound(this, args);
            }
        }

        public event SolutionFoundHandler CompleteSolutionFound;

        private void OnCompleteSolutionFound(PartialSolution newSolution)
        {
            if (CompleteSolutionFound != null)
            {
                SolutionFoundEventArgs args =
                    new SolutionFoundEventArgs(newSolution);
                CompleteSolutionFound(this, args);
            }
        }
        #endregion
    }


    internal delegate void SolutionFoundHandler(object sender, SolutionFoundEventArgs e);

    internal class SolutionFoundEventArgs : EventArgs
    {
        private PartialSolution _newSolution;

        internal SolutionFoundEventArgs(
            PartialSolution newSolution
                )
        {
            _newSolution = newSolution;
        }


        internal PartialSolution NewSolution
        {
            get
            {
                return _newSolution;
            }
        }


    }
}
