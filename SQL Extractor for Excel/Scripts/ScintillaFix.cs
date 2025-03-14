using System;
using System.IO;
using System.Reflection;
using ScintillaNET;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class ScintillaFix
    {
        public static void CopyNativeFolderIfNotExistOrDifferentFixForScintillaBug()
        {
            // Determine the architecture-specific folder (win-x64 or win-x86)
            string architecture = Environment.Is64BitProcess ? "win-x64" : "win-x86";
            // Define the relative folder path for the native files
            string relativeFolder = Path.Combine("runtimes", architecture, "native");

            // Source folder is based on the application's base directory
            string sourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFolder);

            // Get the entry assembly location and then its directory
            string entryAssemblyLocation = Assembly.GetAssembly(typeof(Scintilla)).Location;
            if (string.IsNullOrWhiteSpace(entryAssemblyLocation))
            {
                throw new InvalidOperationException("Could not determine the entry assembly location.");
            }
            string targetBaseFolder = Path.GetDirectoryName(entryAssemblyLocation);
            if (string.IsNullOrWhiteSpace(targetBaseFolder))
            {
                throw new InvalidOperationException("Could not determine the target base directory.");
            }
            // Target folder where native files should be copied
            string targetFolder = Path.Combine(targetBaseFolder, relativeFolder);

            // Check if source folder exists.
            if (Directory.Exists(sourceFolder))
            {
                bool shouldCopy = false;

                // If the target folder does not exist, we need to copy.
                if (!Directory.Exists(targetFolder))
                {
                    shouldCopy = true;
                }
                else
                {
                    // If the target folder exists, check if the total file sizes differ.
                    long sourceSize = FileManager.GetDirectorySize(sourceFolder);
                    long targetSize = FileManager.GetDirectorySize(targetFolder);
                    if (sourceSize != targetSize)
                    {
                        shouldCopy = true;
                    }
                }

                if (shouldCopy)
                {
                    // Ensure the target folder exists (creates it if necessary).
                    Directory.CreateDirectory(targetFolder);
                    // Copy all files and subdirectories recursively from source to target.
                    FileManager.CopyDirectory(sourceFolder, targetFolder);
                }
            }
            else
            {
                throw new DirectoryNotFoundException($"Source folder does not exist: {sourceFolder}");
            }
        }
    }
}


