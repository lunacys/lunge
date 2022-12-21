using System.Collections.Immutable;
using System.IO;

namespace lunge.Library.Scripting;

public class FileUtils
{
    /// <summary>
    /// List all files in a directory in a recursive way (list all directory levels).
    /// Return full paths.
    /// Return empty list if the directory is not found or if the directory is empty.
    /// </summary>
    public static ImmutableList<string> ListAllFilesInAPathRecursively(string pathToList)
    {
        List<string> filesFullPath = new List<string>();

        if (File.Exists(pathToList)) { filesFullPath.Add(pathToList); }  // if 'pathToList' is a file, add it as fullpath
        else if (Directory.Exists(pathToList)) { ProcessDirectory(pathToList); }  // if 'pathToList' is a directory, process it
        else { return ImmutableList<string>.Empty; }  // return empty list  

        return filesFullPath.ToImmutableList();

        #region local functions
        // Process all files in the directory 'targetDirectory', recurse on any found directories and process the contained files
        void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                filesFullPath.Add(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }
        #endregion local functions
    }

    /// <summary>
    /// Return the path of a folder searching the folder in the current execution folder and up in the folder tree. 
    /// Return failure if the file is not found.
    /// </summary>
    public static string SearchAFolderAboveTheCurrentDirectoryOfTheApplication(string folderToSearchWithoutPath)
    {
        string currentPath = Directory.GetCurrentDirectory();  // read the current directory (the execution folder of the program)
        string fullPath = Path.GetFullPath(Path.Combine(currentPath, folderToSearchWithoutPath));  // build the full path to search
        if (Directory.Exists(fullPath)) { return fullPath; }  // if the path is found, return it

        // if the execution is not already ended, loop until the folder is found or until the root folder is reached
        while (true)
        {
            string tempPath = Path.GetFullPath(Path.Combine(currentPath, "..")); // build a path up one level
            if (tempPath == currentPath) { break; }  // if the root folder is reached, end the search
            currentPath = tempPath;  // save the current path
            fullPath = Path.GetFullPath(Path.Combine(currentPath, folderToSearchWithoutPath));  // build the full path to search
            if (Directory.Exists(fullPath)) { return fullPath; }  // if the path is found, return it
        }

        return "not found";  // return failure if the execution reached this point
    }
}