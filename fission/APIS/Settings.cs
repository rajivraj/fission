/*
 * File to perfom some tasks 
 * 
 * */

using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.Text.Encoding;
using static System.Diagnostics.FileVersionInfo;
using static System.Reflection.Assembly;
using System.IO;
using RestSharp;

namespace fission
{

    public static class Settings
    { 
        static int opt;

        /// <summary>
        /// Public method to show the menu for settings
        /// </summary>
        public static void Show()
        {
            Clear();  // clearing the console
            ForegroundColor = Yellow;
            WriteLine("-- Settings --");
            ForegroundColor = Blue;
            Write("1.");
            ForegroundColor = Green;
            WriteLine(" Check for latest updates");
            ForegroundColor = Blue;
            Write("2.");
            ForegroundColor = Green;
            WriteLine(" Set API Key");
            ResetColor();
            Write("> ");
            int.TryParse(ReadLine(), out opt);
        }

        /// <summary>
        /// Using opt to switch functions
        /// </summary>
        public static void Execute()
        {
            switch(opt)
            {
                // Case to handle application update
                case 1:
                    Clear();
                    ForegroundColor = Yellow;
                    WriteLine("--- Application Updator ---");
                    ResetColor();
                    ForegroundColor = Blue;
                    WriteLine("[*] Fetching Version");
                    ResetColor();
                    var url = "https://raw.githubusercontent.com/tbhaxor/fission/master/.versioninfo";
                    // getting executable file version
                    var asm = GetExecutingAssembly();
                    var fvi = GetVersionInfo(asm.Location);
                    var version = fvi.FileVersion;

                    // creating Restclient to fetch version info from github repo
                    var client = new RestClient(url);
                    var request = new RestRequest(Method.GET);
                    var response = client.Execute(request);
                    if (response.IsSuccessful)
                    {
                        var content = response.Content.Split('\n')[0];
                        WriteLine($"[#] Current Version {version}");
                        WriteLine($"[#] Fetched Version {content}");
                        // comparing version
                        if (content == version)
                        {
                            // currently on highest version
                            ForegroundColor = Green;
                            WriteLine("[!] You are already at latest version");
                            ResetColor();
                            Exit(0);
                        }
                        else
                        {
                            // prompting new version available
                            ForegroundColor = Cyan;
                            WriteLine($"[!] New version {response.Content.Split('\n')[0]} is available.");
                            WriteLine($"[!] Download it from https://github.com/tbhaxor/fission/archive/fission-v{response.Content.Split('\n')[0]}.zip");
                            ResetColor();
                            Exit(0);
                        }
                    }
                    else
                    {
                        // if request is not successful then prompt to check internet connection
                        ForegroundColor = Red;
                        WriteLine("Something went wrong");
                        WriteLine("Can't fetch version from remote url");
                        ResetColor();
                        Exit(1);
                    }
                    break;
                
                // Case to handle API Registeration
                case 2:
                    Clear();
                    ForegroundColor = Yellow;
                    WriteLine("--- API registering ---");
                    ResetColor();
                    Write("Enter API Key : ");
                    string api = ReadLine();
                    // checking if api file exists
                    if (!File.Exists(Path.Combine(Globals.AppData, "apikey")))
                    {
                        // creating files
                        var file = File.Create(Globals.ApiKey); // creating file
                        byte[] bytes = ASCII.GetBytes(api);  // getting byte array of api
                        file.Write(bytes, 0, bytes.Length); // writing to files
                        file.Close();  // closing and saving
                    }
                    else
                    {
                        // overwriting api if already exists
                        File.WriteAllText(Path.Combine(Globals.AppData, "apikey"), api);
                    }
                    WriteLine($"Key has been saved in {Path.Combine(Globals.AppData, "apikey")}");
                    break;
                default:
                    ForegroundColor = Red;
                    WriteLine("Invalid Input");
                    ResetColor();
                    Exit(1);
                    break;
            }
        }
    }
}
