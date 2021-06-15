using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;

namespace MorseDecoder
{
    static class DecoderUtility
    {
        static Dictionary<string, string> _morseLookup = null;
        static Dictionary<string, string> _militaryDictionary = null;
        static Solutions _completeSolutions;
        static Solutions _discardedPartialSolutions;
        static Hashtable _characterCounts = null;

        public static Dictionary<string, string> MorseLookup
        {
            get
            {
                //singleton
                if (_morseLookup == null)
                {
                    _morseLookup = new Dictionary<string, string>();

                    //load from file
                    string filePath = DecoderUtility.MorseLookupFilePath;


                    if (File.Exists(filePath))
                    {
                        using (StreamReader sr = File.OpenText(filePath))
                        {
                            string input;
                            string[] split;
                            char[] comma = { ',' };

                            while ((input = sr.ReadLine()) != null)
                            {
                                split = input.Split(comma);
                                _morseLookup.Add(split[0], split[1]);
                            }
                            sr.Close();
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException(
                            "Morse Lookup file not found.",
                            MorseLookupFilePath);
                    }
                }
                return _morseLookup;
            }
        }

        public static Dictionary<string, string> MilitaryDictionary
        {
            get
            {
                //singleton
                if (_militaryDictionary == null)
                {
                    _militaryDictionary = new Dictionary<string, string>();


                    string filePath = DecoderUtility.MilitaryDictionaryFilePath;

                    //load from file
                    if (File.Exists(filePath))
                    {
                        using (StreamReader sr = File.OpenText(filePath))
                        {
                            string input;
                            string morse;
                            while ((input = sr.ReadLine()) != null)
                            {
                                morse = TranslateToMorse(input);
                                _militaryDictionary.Add(input, morse);
                            }
                            sr.Close();
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException(
                            "Military Dictionary Lookup file not found.",
                            MilitaryDictionaryFilePath);
                    }
                }
                return _militaryDictionary;
            }
        }

        public static string TranslateToMorse(string test)
        {
            StringBuilder sb = new StringBuilder(null);
            Dictionary<string, string> _morseLookup = MorseLookup;

            foreach (char c in test.ToUpper())
            {
                if (_morseLookup.ContainsKey(c.ToString()))
                {
                    sb.Append(_morseLookup[c.ToString()]);
                }
            }
            return sb.ToString();
        }

        private static string MorseLookupFilePath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["morseLookupFilePath"];
                }
                catch (ConfigurationErrorsException ex)
                {
                    throw;
                }

            }
        }

        private static string MilitaryDictionaryFilePath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["militaryDictionaryFilePath"];
                }
                catch (ConfigurationErrorsException ex)
                {
                    //TODO: better exception handling
                    throw;
                }

            }
        }

        internal static string SolutionLogFilePath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["solutionLogFilePath"];
                }
                catch (ConfigurationErrorsException ex)
                {
                    //TODO: better exception handling
                    throw;
                }

            }
        }

        internal static string DiscardedLogFilePath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["discardedLogFilePath"];
                }
                catch (ConfigurationErrorsException ex)
                {
                    //TODO: better exception handling
                    throw;
                }

            }
        }

        internal static int SolutionRequiredWordCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["solutionRequiredWordCount"]);
                }
                catch (ConfigurationErrorsException ex)
                {
                    //TODO: better exception handling
                    throw;
                }

            }
        }
        
        internal static Hashtable SolutionRequiredCharacterCounts
        {
            get
            {
                try
                {
                    if (_characterCounts == null)
                    {
                        char[] semicolon = new char[] { ';' };
                        char[] equal = new char[] { '=' };

                        string config = ConfigurationManager.AppSettings["solutionRequiredCharacterCounts"];
                        string[] characters = config.Split(semicolon);
                        _characterCounts = new Hashtable();
                        foreach (string character in characters)
                        {
                            string[] keyValue = character.Split(equal);
                            _characterCounts.Add(keyValue[0], Convert.ToInt32(keyValue[1]));
                        }
                    }
                    return _characterCounts;

                }
                catch (ConfigurationErrorsException ex)
                {
                    //TODO: better exception handling
                    throw;
                }

            }
        }

        public static Solutions CompleteSolutions
        {
            get
            {
                if (_completeSolutions == null)
                {
                    _completeSolutions = new Solutions();
                    _completeSolutions.LogFilePath = DecoderUtility.SolutionLogFilePath;
                }
                return _completeSolutions;
            }
        }

        public static Solutions DiscardedPartialSolutions
        {
            get
            {
                if (_discardedPartialSolutions == null)
                {
                    _discardedPartialSolutions = new Solutions();
                    _discardedPartialSolutions.LogFilePath = DecoderUtility.DiscardedLogFilePath;
                }
                return _discardedPartialSolutions;
            }
        }

    }
}
