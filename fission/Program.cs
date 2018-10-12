using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.Threading.Thread;
using System.IO;

namespace fission
{
    static class MainClass
    {
        public static void Main(string[] args)
        {
            Banner();
            Init();
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

        static void Init()
        {
            if(!Directory.Exists(Globals.AppData))
                Directory.CreateDirectory(Globals.AppData); 
        }
    }
}
