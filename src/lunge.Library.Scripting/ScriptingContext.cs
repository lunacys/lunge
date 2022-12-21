using System.IO;
using System.Text;

namespace lunge.Library.Scripting;

public class ScriptingContext
{
    private FsTypes FsType { get; }
    private Dictionary<string, string>? InMemoryFs { get; }
    private string? ScriptsPathForRealFs { get; }

    /// <summary>
    /// Start scripting context with an in-memory filesystem, and normalize its paths
    /// </summary>
    /// <exception cref="ArgumentException">if the keys of 'inMemoryFs' are not unique after normalization</exception>
    public static ScriptingContext ScriptingContextWithInMemoryFs(IDictionary<string, string> inMemoryFs)
    {
        if (inMemoryFs is null) throw new ArgumentNullException(nameof(inMemoryFs));

        var normalizedInMemoryFs = new Dictionary<string, string>();

        // normalize the InMemoryFs paths and build another Filesystem dictionary
        foreach (KeyValuePair<string, string> entry in inMemoryFs)
        {
            normalizedInMemoryFs.Add(NormalizeInMemoryPath(entry.Key), entry.Value);
        }

        return new ScriptingContext(FsTypes.InMemoryFs, null, normalizedInMemoryFs);
    }

    public static ScriptingContext ScriptingContextWithRealFs(string scriptsPath)
    {
        if (scriptsPath is null) throw new ArgumentNullException(nameof(scriptsPath));
        return new ScriptingContext(FsTypes.RealFs, scriptsPath, null);
    }

    public static ScriptingContext ScriptingContextWithNoFs()
    {
        return new ScriptingContext(FsTypes.NoFs, null, null);
    }

    // init only via static methods
    private ScriptingContext(FsTypes fsType, string? scriptsPathForRealFs, Dictionary<string, string>? inMemoryFs)
    {
        if (fsType == FsTypes.InMemoryFs && scriptsPathForRealFs != null)
            throw new ArgumentException($"{nameof(scriptsPathForRealFs)} should not have a value if {nameof(fsType)} is of {FsTypes.InMemoryFs.ToString()} type");
        if (fsType == FsTypes.InMemoryFs && inMemoryFs == null)
            throw new ArgumentException($"{nameof(inMemoryFs)} should have a value if {nameof(fsType)} is of {FsTypes.InMemoryFs.ToString()} type");

        if (fsType == FsTypes.RealFs && scriptsPathForRealFs == null)
            throw new ArgumentNullException($"{nameof(scriptsPathForRealFs)} should have a value if {nameof(fsType)} is of {FsTypes.RealFs.ToString()} type");
        if (fsType == FsTypes.RealFs && inMemoryFs != null)
            throw new ArgumentNullException($"{nameof(inMemoryFs)} should not have a value if {nameof(fsType)} is of {FsTypes.RealFs.ToString()} type");

        if (fsType == FsTypes.NoFs && scriptsPathForRealFs != null)
            throw new ArgumentException($"{nameof(scriptsPathForRealFs)} should not have a value if {nameof(fsType)} is of {FsTypes.NoFs.ToString()} type");
        if (fsType == FsTypes.NoFs && inMemoryFs != null)
            throw new ArgumentNullException($"{nameof(inMemoryFs)} should not have a value if {nameof(fsType)} is of {FsTypes.NoFs.ToString()} type");

        FsType = fsType;
        InMemoryFs = inMemoryFs;
        ScriptsPathForRealFs = scriptsPathForRealFs;
    }

    /// <summary>
    /// Read file from in-memory or read filesystem
    /// </summary>
    /// <param name="path">with RealFs, doesn't allow going up to folders above 'ScriptsPathForRealFs' then, if normalized path contains ".." path, throws exception</param>
    public string ReadFile(string path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        // read file from InMemoryFs doing little normalizations to path
        if (FsType == FsTypes.InMemoryFs)
        {
            string _path = NormalizeInMemoryPath(path);
            if (InMemoryFs != null && InMemoryFs.ContainsKey(_path))  // try to read *normalized* path
                return InMemoryFs[_path];
            throw new ArgumentException("file not found");  // return error
        }
        // read file from FileSystem, adding ScriptsPathForRealFs to the path to read
        // and throwing exception with invalid paths (containing ".." characters)
        else if (FsType == FsTypes.RealFs)
        {
            if (IsForbiddedPath(path))
                throw new ArgumentException($"{nameof(path)} contains '..' path");
            if (ScriptsPathForRealFs == null)
                throw new ArgumentNullException();
            return System.IO.File.ReadAllText(Path.Combine(ScriptsPathForRealFs, path), System.Text.Encoding.UTF8);
        }
        else if (FsType == FsTypes.NoFs)
            throw new InvalidOperationException($"{nameof(FsType)} Fs not loaded");
        else
            throw new InvalidOperationException($"{nameof(FsType)} not recognized");
    }

    /// <summary>
    /// Append a file to in-memory or to the filesystem
    /// </summary>
    /// <param name="path">with RealFs, doesn't allow going up to folders above 'ScriptsPathForRealFs' then, if normalized path contains ".." path, throws exception</param>
    public void AppendFile(string path, string contents)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));
        if (contents is null) throw new ArgumentNullException(nameof(contents));

        // read file from InMemoryFs doing little normalizations to path
        if (FsType == FsTypes.InMemoryFs)
        {
            string _path = NormalizeInMemoryPath(path);
            if (InMemoryFs.ContainsKey(_path))  // if the path is present
                                                      // see https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder.appendline
                InMemoryFs[_path] = new StringBuilder().AppendLine(InMemoryFs[_path]).AppendLine(contents).ToString();
            else  // if the path is not present
                InMemoryFs.Add(_path, contents);
        }
        // append file to FileSystem
        else if (FsType == FsTypes.RealFs)
        {
            if (IsForbiddedPath(path))
                throw new ArgumentException($"{nameof(path)} contains '..' path");
            File.AppendAllText(Path.Combine(ScriptsPathForRealFs, path), contents);  // https://docs.microsoft.com/en-us/dotnet/api/system.io.file.appendalltext
        }
        else if (FsType == FsTypes.NoFs)
            throw new InvalidOperationException($"{nameof(FsType)} Fs not loaded");
        else
            throw new InvalidOperationException($"{nameof(FsType)} not recognized");
    }

    /// <summary>
    /// Delete a file. Doesn't go in error if the file path is non existent.
    /// </summary>
    public void DeleteFile(string path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        // delete file from InMemoryFs doing little normalizations to path
        if (FsType == FsTypes.InMemoryFs)
        {
            string _path = NormalizeInMemoryPath(path);
            if (InMemoryFs.ContainsKey(_path))  // if the path is present
                InMemoryFs.Remove(_path);
        }
        // delete file from FileSystem
        else if (FsType == FsTypes.RealFs)
        {
            if (IsForbiddedPath(path))
                throw new ArgumentException($"{nameof(path)} contains '..' path");
            File.Delete(Path.Combine(ScriptsPathForRealFs, path));  // https://docs.microsoft.com/en-us/dotnet/api/system.io.file.delete
        }
        else if (FsType == FsTypes.NoFs)
            throw new InvalidOperationException($"{nameof(FsType)} Fs not loaded");
        else
            throw new InvalidOperationException($"{nameof(FsType)} not recognized");
    }

    public bool Exists(string path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        // test if file exists inside InMemoryFs
        if (FsType == FsTypes.InMemoryFs)
        {
            string _path = NormalizeInMemoryPath(path);
            if (InMemoryFs.ContainsKey(_path))  // if the path is present
                return true;
            return false;
        }
        // test if file exists inside FileSystem
        else if (FsType == FsTypes.RealFs)
        {
            if (IsForbiddedPath(path))
                throw new ArgumentException($"{nameof(path)} contains '..' path");
            return File.Exists(Path.Combine(ScriptsPathForRealFs, path));  // https://docs.microsoft.com/en-us/dotnet/api/system.io.file.exists
        }
        else if (FsType == FsTypes.NoFs)
            throw new InvalidOperationException($"{nameof(FsType)} Fs not loaded");
        else
            throw new InvalidOperationException($"{nameof(FsType)} not recognized");
    }

    // Normalize InMemory path:
    // * trim
    // * replace "\" with "/"
    // * removing leading "./" if present
    private static string NormalizeInMemoryPath(string path)
    {
        string retPath = path.Trim();
        retPath = retPath.Replace("\\", "/", StringComparison.InvariantCultureIgnoreCase);
        if (retPath.StartsWith("./", StringComparison.InvariantCulture))
            retPath = retPath.Remove(0, 2);
        return retPath;
    }

    private static bool IsForbiddedPath(string path)
    {
        if (NormalizeInMemoryPath(path).StartsWith("/..", StringComparison.InvariantCultureIgnoreCase))
            return true;
        if (NormalizeInMemoryPath(path).StartsWith("../", StringComparison.InvariantCultureIgnoreCase))
            return true;
        if (NormalizeInMemoryPath(path).Contains("/../", StringComparison.InvariantCultureIgnoreCase))
            return true;
        return false;
    }

    public enum FsTypes
    {
        NoFs = 0,
        InMemoryFs,
        RealFs,
    }
}