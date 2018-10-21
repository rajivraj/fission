using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.Threading.Thread;
using System.IO;

namespace fission
{
    static class MainClass
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        public static void Main()
        {
            // invoking functions
            Banner();
            Init();

            // switching menu
            switch(Menu())
            {
                case 1:
                    Scanner.Show();
                    Scanner.Execute();
                    break;
                case 2:
                    Settings.Show();
                    Settings.Execute();
                    break;
                case 3:
                    // say thanks and exit
                    Clear();
                    string s1 = "Thank you for using ";
                    string s2 = "Fission";
                    foreach(var s in s1)
                    {
                        Write(s);
                        Sleep(50);
                    }
                    ForegroundColor = Cyan;
                    foreach (var s in s2)
                    {
                        Write(s);
                        Sleep(50);
                    }
                    ResetColor();
                    WriteLine();
                    Exit(0);
                    break;
                default:
                    ForegroundColor = Red;
                    WriteLine("Invalid Selection");
                    ResetColor();
                    Exit(0);
                    break;
            }
        }

        /// <summary>
        /// Prints banner
        /// </summary>
        static void Banner()
        {
            Clear();
            ForegroundColor = Yellow;
            WriteLine(" _____ _         _         ");
            WriteLine("|   __|_|___ ___|_|___ ___ ");
            WriteLine("|   __| |_ -|_ -| | . |   |");
            WriteLine("|__|  |_|___|___|_|___|_|_|");
            WriteLine();
            ForegroundColor = Cyan;
            WriteLine("  Damn Easy File Scanner");
            ResetColor();
            WriteLine();
        }

        /// <summary>
        /// Handle menu prompting
        /// </summary>
        /// <returns>Option selected</returns>
        static int Menu()
        {
            ForegroundColor = Yellow;
            WriteLine("-- Menu --");
            ForegroundColor = Blue;
            Write("1.");
            ForegroundColor = Green;
            WriteLine(" Scan");
            ForegroundColor = Blue;
            Write("2.");
            ForegroundColor = Green;
            WriteLine(" Settings");
            ForegroundColor = Blue;
            Write("3.");
            ForegroundColor = Green;
            WriteLine(" Exit");
            ResetColor();
            Write("> ");
            int.TryParse(ReadLine(), out var opt);
            return opt;
        }

        /// <summary>
        /// Method to check if appdata exists, otherwise create appdata directory
        /// </summary>
        static void Init()
        {
            if(!Directory.Exists(Globals.AppData))
                Directory.CreateDirectory(Globals.AppData); 
        }
    }
}
