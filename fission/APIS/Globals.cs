/*
 * File to return some global values
 * */

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
        /// <summary>
        /// Gets the app data path.
        /// </summary>
        /// <value>The app data.</value>
        public static string AppData
        {
            get
            {
                return Combine(GetFolderPath(SpecialFolder.ApplicationData), "Fission");
            }
        }

        /// <summary>
        /// Gets the misc file path 
        /// </summary>
        /// <value>The misc file.</value>
        public static string MiscFile
        {
            get
            {
                return Combine(AppData, "Misc");
            }
        }

        /// <summary>
        /// Gets the API key file  path
        /// </summary>
        /// <value>The API key.</value>
        public static string ApiKey
        {
            get
            {
                return Combine(AppData, "apikey");
            }
        }

        /// <summary>
        /// Gets the current date time formated.
        /// </summary>
        /// <value>The current date time.</value>
        public static string CurrentDateTime
        {
            get
            {
                return $"{Now:dd/MM/yyyy hh:mm:ss tt}";
            }
        }

        /// <summary>
        /// Gets the SHA 256 of file.
        /// </summary>
        /// <returns>The SHA 256.</returns>
        /// <param name="file">File name whose SH256 you want</param>
        public static string GetSHA256(string file)
        {
            string h = string.Empty;
            try
            {
                // File Found
                using (var sha = SHA256.Create())  // created a sha256 stream
                using (var streamRead = File.OpenRead(file))  // input file stream
                {
                    var hash = sha.ComputeHash(streamRead); // computing hash
                    h =  System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant(); // coverting bytes to string
                }
            }
            catch(FileNotFoundException e)
            {
                // File not found therefore exiting
                ForegroundColor = Red;
                WriteLine(e.Message);
                ResetColor();
                Exit(1);
            }
            return h; // returning hash
        }
    }
}
