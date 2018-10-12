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
        public static void Show()
        {
            Clear();
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

        public static void Execute()
        {
            switch(opt)
            {
                case 1:
                    Clear();
                    ForegroundColor = Blue;
                    WriteLine("[*] Fetching Version");
                    ResetColor();
                    var url = "https://raw.githubusercontent.com/tbhaxor/fission/master/.versioninfo";
                    var urlP = $"{url}?token=AbEloXTmnDJMsYcXr45MZC4skJ7gNuQEks5byYvuwA%3D%3D";
                    var asm = GetExecutingAssembly();
                    var fvi = GetVersionInfo(asm.Location);
                    var version = fvi.FileVersion;
                    var client = new RestClient(urlP);
                    var request = new RestRequest(Method.GET);
                    var response = client.Execute(request);
                    if (response.IsSuccessful)
                    {
                        var content = response.Content.Split('\n')[0];
                        WriteLine($"[#] Current Version {version}");
                        WriteLine($"[#] Fetched Version {content}");
                        if (content == version)
                        {
                            ForegroundColor = Green;
                            WriteLine("[!] You are already at latest version");
                            ResetColor();
                            Exit(0);
                        }
                        else
                        {
                            ForegroundColor = Cyan;
                            WriteLine($"[!] New version {response.Content.Split('\n')[0]} is available.");
                            WriteLine($"[!] Download it from https://github.com/tbhaxor/fission/archive/fission-v{response.Content.Split('\n')[0]}.zip");
                            ResetColor();
                            Exit(0);
                        }
                    }
                    else
                    {
                        ForegroundColor = Red;
                        WriteLine("Something went wrong");
                        WriteLine("Can't fetch version from remote url");
                        ResetColor();
                        Exit(1);
                    }
                    break;
                case 2:
                    Clear();
                    Write("Enter API Key : ");
                    string api = ReadLine();
                    if (!File.Exists(Path.Combine(Globals.AppData, "apikey")))
                    {
                        var file = File.Create(Path.Combine(Globals.AppData, "apikey"));
                        byte[] bytes = ASCII.GetBytes(api);
                        file.Write(bytes, 0, bytes.Length);
                        file.Close();
                    }
                    else
                    {
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
