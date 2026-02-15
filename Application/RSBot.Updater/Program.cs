using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace RSBot.Updater;

internal static class Program
{
    private static void Main()
    {
        try
        {
            var directory = Environment.CurrentDirectory;

            Console.WriteLine("Starting RSBot Updater...");

            // Find process working on this directory and kill it
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName("RSBot");
            foreach (var process in processes)
            {
                if (process.StartInfo.WorkingDirectory == directory)
                {
                    Console.WriteLine($"Killing process {process.Id}...");
                    process.Kill();
                    process.WaitForExit();
                }
            }

            var tempDirectory = Path.Combine(directory, "update_temp");
            var zipFilePath = tempDirectory + "\\update.zip";

            if (Directory.Exists(tempDirectory) && File.Exists(zipFilePath))
            {
                ZipFile.ExtractToDirectory(zipFilePath, tempDirectory);

                File.Delete(zipFilePath);
                CopyDir(tempDirectory, directory);
                Directory.Delete(tempDirectory, true);
            }

            Console.WriteLine("Update applied successfully. Starting RSBot...");
            Process.Start(directory + "\\RSBot.exe");
        }
        catch (Exception ex)
        {
            File.WriteAllText("updater_error.log", ex.ToString());
        }

        Environment.Exit(0);
    }

    /// <summary>
    /// Copy directory to destination directory
    /// </summary>
    /// <param name="sourceFolder">The source folder</param>
    /// <param name="destFolder">The Destination folder</param>
    private static void CopyDir(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);

        // Get Files & Copy
        var files = Directory.GetFiles(sourceFolder);
        foreach (var file in files)
            File.Copy(file, Path.Combine(destFolder, Path.GetFileName(file)), true);

        // Get dirs recursively and copy files
        var folders = Directory.GetDirectories(sourceFolder);
        foreach (var folder in folders)
            CopyDir(folder, Path.Combine(destFolder, Path.GetFileName(folder)));
    }
}
