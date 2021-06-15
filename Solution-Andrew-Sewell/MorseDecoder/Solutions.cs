using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using System.IO;


namespace MorseDecoder
{
    class Solutions : CollectionBase
    {

        string _logFilePath = "";
        
        #region attributes

        internal string LogFilePath
        {
            get
            {
                return _logFilePath;
            }
            set
            {
                _logFilePath = value;
            }
        }

        #endregion

        #region operations

        private void WriteSolution(PartialSolution newSolution, SolutionAddedReason reason)
        {
            //should use logging provider
            if (LogFilePath != "")
            {
                if (File.Exists(LogFilePath))
                {
                    using (StreamWriter sw = File.AppendText(LogFilePath))
                    {
                        sw.WriteLine(
                            String.Format("{2} at {1}: {0}", 
                                newSolution.Solution, 
                                DateTime.Now.ToString("ddd, dd-MMM-yyyy hh:mm:ss"),
                                reason.ToString()));
                        sw.Close();
                    }
                }
            }


        }

        internal int Add(PartialSolution newSolution)
        {
            return Add(newSolution, SolutionAddedReason.Unknown); 
        }

        internal int Add(PartialSolution newSolution, SolutionAddedReason reason)
        {
            if (!List.Contains(newSolution))
            {
                this.WriteSolution(newSolution, reason);
                return List.Add(newSolution);
            }
            else
            {
                return List.IndexOf(newSolution);
            }
        }

        internal enum SolutionAddedReason
        {
            Solved,
            WordCountOutOfRange,
            CharacterCountOutOfRange,
            Unknown
        }

        internal void Remove(PartialSolution solution)
        {
            List.Remove(solution);
        }


        #endregion
    }
}
