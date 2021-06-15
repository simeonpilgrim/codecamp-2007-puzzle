using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections;



namespace MorseDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestDataLoad();

            //Dictionary<string, string> militar = DecoderUtility.MilitaryDictionary;
            //StringBuilder s = new StringBuilder();
            //GenerateTestMorse();

            DecodeMystery();
            //TestMatch();
            //TestDecodedResultConfiguration();

            Console.ReadLine();
        }

        private static void TestDecodedResultConfiguration()
        {
            Console.WriteLine("SolutionRequiredCharacterCounts");
            Console.WriteLine("------------------------------------------");

            foreach (DictionaryEntry de in DecoderUtility.SolutionRequiredCharacterCounts )
            {
                Console.WriteLine(String.Format("{0} = {1}", de.Key.ToString(), de.Value.ToString()));
            }
            Console.WriteLine();
            Console.WriteLine(String.Format("SolutionRequiredWordCount = {0}", DecoderUtility.SolutionRequiredWordCount.ToString()));

        }

        private static void TestMatch()
        {
            string mystery = ".-...-.-.-";
            Regex regex = new Regex(@"^\.\-\.\.\.\-\.\-\.\-");
            if (regex.IsMatch(mystery))
            {
                Console.WriteLine("Match");
            }
            else
            {
                Console.WriteLine("No Match");
            }
        }



        private static void DecodeMystery()
        {
            string mysteryCode = "";
            string filePath = ConfigurationManager.AppSettings["mysteryCodeFilePath"];
            if (File.Exists(filePath))
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    mysteryCode = sr.ReadLine();
                    sr.Close();
                }
            }

            PartialSolution p = new PartialSolution(mysteryCode);
            p.Solve();

            Console.WriteLine("Solutions");
            Console.WriteLine("-------------------------------------------");

            foreach (PartialSolution  complete in DecoderUtility.CompleteSolutions )
            {
                Console.WriteLine(complete.Solution);
            }


        }

        private static void GenerateTestMorse()
        {

            string test = "THERE ALL ART";
            string morse = DecoderUtility.TranslateToMorse(test);

            Console.WriteLine(morse);

            using (StreamWriter file = new StreamWriter(@"H:\development\morseDecoder\data\testmessage.txt"))
            {
                file.WriteLine(morse);
            }
        }

        private static void TestDataLoad()
        {
            Dictionary<string, string> morseLookup;
            Dictionary<string, string> militaryDictionary;

            try
            {
                morseLookup = DecoderUtility.MorseLookup;

                Console.WriteLine(String.Format("morseLookup has {0} elements.", morseLookup.Count.ToString()));

                militaryDictionary = DecoderUtility.MilitaryDictionary;
                foreach (KeyValuePair<string, string> word in militaryDictionary)
                {
                    Console.WriteLine(
                            string.Format("{0} : {1}", word.Key, word.Value));
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.FileName);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal static void ReportSolutionFound(object sender, SolutionFoundEventArgs e)
        {
            PartialSolution p = (PartialSolution)sender;
            if (e.NewSolution.Solved)
            {
                Console.Write("Complete solution found = ");
            }
            else
            {
                Console.Write("Partial solution found = ");
            }
            Console.WriteLine(e.NewSolution.Solution);
        }

    }
}
