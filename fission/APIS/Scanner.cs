﻿/*
 * File to scan file
 * 
 * */
using static System.Console;
using static System.ConsoleColor;
using static System.Environment;
using static System.Threading.Thread;
using static System.DateTime;
using VirusTotalNET;
using VirusTotalNET.Results;
using VirusTotalNET.ResponseCodes;

using System.IO;

namespace fission
{
    public static class Scanner
    {
        static int opt;

        /// <summary>
        /// Method to show scanner menu
        /// </summary>
        public static void Show()
        {
            Clear();
            ForegroundColor = Yellow;
            WriteLine("-- Scanner --");
            ForegroundColor = Blue;
            Write("1.");
            ForegroundColor = Green;
            WriteLine(" Current Directory Scan");
            ForegroundColor = Blue;
            Write("2.");
            ForegroundColor = Green;
            WriteLine(" File Scan");
            ForegroundColor = Blue;
            Write("3.");
            ForegroundColor = Green;
            WriteLine(" Misc Files");
            ResetColor();
            Write("> ");
            int.TryParse(ReadLine(), out opt);
        }

        /// <summary>
        /// Method to handle user selection for scanner
        /// </summary>
        public static void Execute()
        {
            SafeGuardApi(); // safegaurd api check
            switch(opt)
            {
                case 1:
                    Clear();
                    ScanCurrDir();
                    break;
                case 2:
                    Clear();
                    ScanFile();
                    break;
                case 3:
                    Clear();
                    MiscScan();
                    break;
                default:
                    ForegroundColor = Red;
                    WriteLine("Invalid selection");
                    ResetColor();
                    Exit(1);
                    break;
            }
        }

        /// <summary>
        /// Method to get report report
        /// </summary>
        /// <returns>The report.</returns>
        /// <param name="sha256">Sha256.</param>
        static FileReport Report(string sha256)
        {
            VirusTotal virus = new VirusTotal(File.ReadAllText(Globals.ApiKey));
            var report = virus.GetFileReportAsync(sha256);
            return report.Result;
        }

        /// <summary>
        /// Method to send file on server for scanning
        /// </summary>
        /// <returns>The scan.</returns>
        /// <param name="file">File.</param>
        static ScanResult SendScan(FileInfo file)
        {

            VirusTotal vt = new VirusTotal(File.ReadAllText(Globals.ApiKey));

            System.Threading.Tasks.Task<ScanResult> report = null;
            try
            {
                report = vt.ScanFileAsync(file);
            }
            catch(System.AggregateException e)
            {
                WriteLine(e.Message);
                Exit(1);
            }
            return report.Result;

        }

        /// <summary>
        /// Checks if api exists and is valid or not
        /// </summary>
        static void SafeGuardApi()
        {
            if(!File.Exists(Globals.ApiKey) || File.ReadAllText(Globals.ApiKey) == "")
            {
                ForegroundColor = Red;
                WriteLine("Api Key not found");
                WriteLine("Reset Api now to use Scanner");
                ResetColor();
                Exit(1);
            }
        }

        /// <summary>
        /// Method Scans the curr dir.
        /// </summary>
        static void ScanCurrDir()
        {
            int total = 0;
            WriteLine("-- Directory Scanner --");
            WriteLine();
            ForegroundColor = Blue;
            WriteLine($"[*] Scan Started At {Globals.CurrentDateTime}");
            ResetColor();

            // getting filenames
            var files = Directory.GetFiles(CurrentDirectory, "*", SearchOption.AllDirectories);
            // instacing misc to store misc files
            var misc = new System.Collections.Generic.List<string>();
            var isMisc = false; // flag to check if any misc file
            foreach (var file in files)
            {
                if (++total > 5000)  // safeguard api calls
                {
                    ForegroundColor = Red;
                    WriteLine("You reached  5000/per day scan limit");
                    WriteLine("If you try to scan again then your API may be blocked");
                    ResetColor();
                    Exit(1);
                }
                var fileInfo = new FileInfo(file);
                if (fileInfo.Length < 1.28e+8)  // safeguard file size
                {
                    ForegroundColor = Yellow;
                    WriteLine($"[~] Scanning {fileInfo.FullName}");
                    ResetColor();
                    var report = Report(Globals.GetSHA256(fileInfo.FullName));
                    if (report.ResponseCode == FileReportResponseCode.Present) 
                    {
                        // execute detection and format detection if file is present on server
                        int detected = 0;
                        foreach(var av in report.Scans.Keys)
                        {
                            if (report.Scans[av].Detected)
                                detected++;
                        }
                        FormatDetection(detected);
                    }
                    // otherwise misc file is not there or is being scanned
                    else if(report.ResponseCode == FileReportResponseCode.Queued)
                    {
                        // file is queued
                        ForegroundColor = Magenta;
                        WriteLine("[!] Not found - Misc File");
                        ResetColor();
                    }
                    else
                    {
                        ForegroundColor = Magenta;
                        WriteLine("[!] Not found - Misc File");
                        ResetColor();
                        var scan = SendScan(fileInfo);
                        misc.Add($"{scan.SHA256}|{fileInfo.FullName}");  // adding to misc file
                        isMisc = true;
                        //break;
                    }
                    WriteLine($"[#] Next Scan At {Now.AddSeconds(30):dd/MM/yyyy hh:mm:ss tt}");
                    Sleep(30000); // sleep to prevent spamming

                }
                else
                {
                    ForegroundColor = Yellow;
                    WriteLine($"[!] Skipping {fileInfo.FullName} - Beyond 128mB");
                    ResetColor();
                }
            }
            ForegroundColor = Blue;
            WriteLine($"[*] Scan Completed At {Globals.CurrentDateTime}");
            ResetColor();
            if(isMisc)
            {
                // writing misc files
                File.WriteAllLines(Globals.MiscFile, misc.ToArray());
                WriteLine();
                WriteLine("We found some miscellaneous files");
                WriteLine($"Try scaning misc file after {Now.AddHours(5):dd/MM/yyyy hh:mm:ss tt}");
            }
        }

        /// <summary>
        /// Method scans the file.
        /// Note: Same implementation as of ScanDir but with one file only
        /// </summary>
        static void ScanFile()
        {
            WriteLine("-- File Scanner --");
            WriteLine();
            ForegroundColor = Cyan;
            Write("[?] Enter File Path : ");
            ResetColor();
            
            FileInfo fileInfo = new FileInfo(ReadLine());

            var vt = new VirusTotal(File.ReadAllText(Globals.ApiKey));
            ForegroundColor = Blue;
            WriteLine($"[*] Scan Started At {Globals.CurrentDateTime}");
            ResetColor();
            var report = Report(Globals.GetSHA256(fileInfo.FullName));
            ForegroundColor = Yellow;
            WriteLine($"[~] Scanning {fileInfo.FullName}");
            ResetColor();
            var isMisc = false;
            if(report.ResponseCode == FileReportResponseCode.Present)
            {
                int detection = 0;
                foreach(var av in report.Scans.Keys)
                {
                    if (report.Scans[av].Detected)
                        detection++;
                }
                FormatDetection(detection);
            }
            else
            {
                var scanReport = SendScan(fileInfo);
                var file = new string[] { $"{scanReport.SHA256}|{fileInfo.FullName}"};
                File.WriteAllLines(Globals.MiscFile, file);
                isMisc = true;
            }
            ForegroundColor = Blue;
            WriteLine($"[*] Scan Completed At {Globals.CurrentDateTime}");
            ResetColor();
            if (isMisc)
            {
                WriteLine();
                WriteLine("We found some miscellaneous files");
                WriteLine($"Try scaning misc file after {Now.AddHours(5):dd/MM/yyyy hh:mm:ss tt}");
            }
        }

        /// <summary>
        /// Method scans miscellaneous files in "Misc" file
        /// </summary>
        static void MiscScan()
        {
            WriteLine("-- Miscellaneous File Scanner --");
            WriteLine();

            if (!File.Exists(Globals.MiscFile))  // safeguard misc file search
            {
                ForegroundColor = Yellow;
                WriteLine("No miscellaneous file to scan");
                ResetColor();
                Exit(0);
            }

            var miscFeed = new System.Collections.Generic.Dictionary<string, string>();
            foreach(var entry in File.ReadAllLines(Globals.MiscFile))
            {
                var split = entry.Split('|');
                miscFeed.Add(split[0], split[1]);
            }

            ForegroundColor = Blue;
            WriteLine($"[*] Scan Started At {Globals.CurrentDateTime}");
            foreach(var hash in miscFeed.Keys)
            {
                ForegroundColor = Yellow;
                WriteLine($"[~] Scanning {miscFeed[hash]}");
                var report = Report(hash);
                int detection = 0;
                foreach(var av in report.Scans.Keys)
                {
                    if (report.Scans[av].Detected)
                        detection++;
                }
                FormatDetection(detection);
                WriteLine($"[#] Next Scan At {Now.AddSeconds(30):dd/MM/yyyy hh:mm:ss tt}");
                Sleep(30000);
            }
            ForegroundColor = Blue;
            WriteLine($"[*] Scan Completed At {Globals.CurrentDateTime}");
            ResetColor();
            File.Delete(Globals.MiscFile); // delete file
        }

        /// <summary>
        /// Formats the detection ratio.
        /// </summary>
        /// <param name="detection">Detection.</param>
        static void FormatDetection(int detection)
        {
            Write("[!] Result - ");
            if (detection == 0)
            {
                ForegroundColor = Green;
                WriteLine("Not Detected {Safe}");
                ResetColor();
            }
            else if (detection > 0 && detection < 20)
            {
                ForegroundColor = Cyan;
                WriteLine("Detected {May cause harm}");
                ResetColor();
            }
            else
            {
                ForegroundColor = Red;
                WriteLine("Detected {Malicious}");
                ResetColor();
            }
        }
    }
}
