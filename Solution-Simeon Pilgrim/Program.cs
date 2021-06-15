using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BootCamp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Expects that the miltary_words.txt is place in c:\
            // Is run twice to remove the JIT compilation from the timing.

            Stopwatch sw = new Stopwatch();

            sw.Start();
            RadixSearchs.CountTESpaceSearch(".-.-...-..-.-.------.--.....-...--.---.-.------...--------.-..---.--...-.---.-..--.-.-.....-.---.-..-----.-.--.-....-..-.........");
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();
            RadixSearchs.CountTESpaceSearch(".-.-...-..-.-.------.--.....-...--.---.-.------...--------.-..---.--...-.---.-..--.-.-.....-.---.-..-----.-.--.-....-..-.........");
            sw.Stop();
            System.Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
