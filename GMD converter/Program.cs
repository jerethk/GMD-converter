using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMD_converter
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void TestMe()
        {
            var input = new byte[]
            {
                0xc0,
                0x80,
                0x80,
                0x80,
            };

            var (len, bcount) = MidiEvent.GetLength(input, 0);
            Console.WriteLine(len);
        }
    }
}
