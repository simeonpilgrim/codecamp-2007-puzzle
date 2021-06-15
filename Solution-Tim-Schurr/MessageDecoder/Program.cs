using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MessageDecoder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message, "Message Decoder", MessageBoxButtons.OK, MessageBoxIcon.Stop);  
            }
        }
    }
}