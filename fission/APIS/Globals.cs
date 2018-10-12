using static System.Environment;
using static System.IO.Path;
using static System.DateTime;
using static System.Console;
using static System.ConsoleColor;
using System.Security.Cryptography;
using System.IO;

namespace fission
{
    public static class Globals
    {
        public static string AppData
        {
            get
            {
                return Combine(GetFolderPath(SpecialFolder.ApplicationData), "Fission");
            }
        }

        public static string MiscFile
        {
            get
            {
                return Combine(AppData, "Misc");
            }
        }

        public static string ApiKey
        {
            get
            {
                return Combine(AppData, "apikey");
            }
        }

        public static string CurrentDateTime
        {
            get
            {
                return $"{Now:dd/MM/yyyy hh:mm:ss tt}";
            }
        }

        public static string GetSHA256(string file)
        {
            string h = string.Empty;
            try
            {
                using (var sha = SHA256.Create())
                using (var streamRead = File.OpenRead(file))
                {
                    var hash = sha.ComputeHash(streamRead);
                    h =  System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            catch(FileNotFoundException e)
            {
                ForegroundColor = Red;
                WriteLine(e.Message);
                ResetColor();
                Exit(1);
            }
            return h;
        }
    }
}
