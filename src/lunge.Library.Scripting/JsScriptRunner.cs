using System.Linq;
using System.Reflection;
using Jint;
using lunge.Library.Scripting.TsDeclarations;
using lunge.Library.Scripting.TsDeclarations.Generator;

namespace lunge.Library.Scripting;

public class JsScriptRunner
{
    public object? ScriptingContext { get; }
    private Engine JintEngine;
    private string? _contextName;

    internal DeclarationFileGeneratorOld DeclarationFileGeneratorOld { get; }
    internal DeclarationFileGenerator DeclarationFileGenerator { get; }

    private List<(string Name, object Value)> _savedTypes = new ();

    /// <summary>
    /// Init a JavaScript runner without context.
    /// In Debug mode, the script name will be the first row of the script, normalized.
    /// </summary>
    public static JsScriptRunner RunnerWithoutContext()
    {
        return new JsScriptRunner(scriptingContext: null, contextName: null);
    }

    /// <summary>
    /// Init a JavaScript runner with context.
    /// In Debug mode, the script name will be the first row of the script, normalized.
    /// </summary>
    public static JsScriptRunner RunnerWithContext(object? scriptingContext, string? contextName)
    {
        if (scriptingContext is null) throw new ArgumentNullException(nameof(scriptingContext));
        if (contextName is null) throw new ArgumentNullException(nameof(contextName));

        return new JsScriptRunner(scriptingContext: scriptingContext, contextName: contextName);
    }

    private JsScriptRunner(object? scriptingContext = null, string? contextName = null)
    {
        if (!(scriptingContext is null) && contextName is null) throw new ArgumentNullException(nameof(contextName));

        DeclarationFileGeneratorOld = new DeclarationFileGeneratorOld();
        DeclarationFileGenerator = new DeclarationFileGenerator();
        ScriptingContext = scriptingContext;
        _contextName = contextName;

        Reload();
    }

    public void AddHostObject(object hostObject, string name)
    {
        JintEngine.SetValue(name, hostObject);
        
        DeclarationFileGenerator.Register(hostObject, name);
        DeclarationFileGeneratorOld.Register(hostObject, name);
    }

    public void AddHostObject(Type hostObjectType)
    {
        var name = hostObjectType.Name;

        var index = name.IndexOf('`');
        if (index >= 0)
            name = name.Remove(index);

        var attrs = hostObjectType.GetCustomAttributes();

        foreach (var attribute in attrs)
        {
            if (attribute is TsInterfaceAttribute i && i.Name != null)
            {
                name = i.Name;
            }
            else if (attribute is TsClassAttribute c && c.Name != null)
            {
                name = c.Name;
            }
        }

        AddHostObject(hostObjectType, name);
    }

    public string GenerateDeclarationFileOld() => DeclarationFileGeneratorOld.Generate();
    public string GenerateDeclarationFile() => DeclarationFileGenerator.Generate();

    public void AddModule(string specifier, string code)
    {
        JintEngine.AddModule(specifier, code);
    }

    /// <summary>
    /// Load a folder content and run all scripts inside it.
    /// sort files with .OrderBy(x => x, StringComparer.OrdinalIgnoreCase) before executing them
    /// </summary>
    public void RunScriptFilesFromAFolder(string path)
    {
        foreach (string file in FileUtils.ListAllFilesInAPathRecursively(path).OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
            RunScriptFile(file);
    }

    /// <summary>
    /// Run a script from a file
    /// </summary>
    public void RunScriptFile(string file)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        Run(System.IO.File.ReadAllText(file, System.Text.Encoding.UTF8));
    }

    public void RunScriptsTexts(IEnumerable<string> scripts)
    {
        if (scripts is null) throw new ArgumentNullException(nameof(scripts));
        foreach (var script in scripts)
            Run(script);
    }

    public void Run(string script, bool discardFromDebugView = false)
    {
        if (script is null) throw new ArgumentNullException(nameof(script));

        try
        {
            JintEngine.Execute(script);
        }
        catch (Esprima.ParserException ex)
        {
            throw new ApplicationException($"{ex.Error} (Line {ex.LineNumber}, Column {ex.Column}), -> {ReadLine(script, ex.LineNumber)}", ex);
        }
        catch (Jint.Runtime.JavaScriptException ex)  // from https://github.com/sebastienros/jint/issues/112
        {
            throw new ApplicationException($"{ex.Error} (Location: {ex.Location}), -> {ReadLine(script, ex.Location.Start.Line)}", ex);
        }
    }

    /// <summary>
    /// Call a Js function by string, passing a single string argument to it (the argument can be a Json object parsed inside the function)
    /// </summary>
    public void JsRun(string function, string argument = null)
    {
        if (function is null) throw new ArgumentNullException(nameof(function));
        try
        {
            JintEngine.Invoke(function, argument);  // execute the function passing the argument
        }
        catch (Esprima.ParserException ex)
        {
            throw new ApplicationException($"{ex.Error} (Line {ex.LineNumber}, Column {ex.Column})", ex);
        }
        catch (Jint.Runtime.JavaScriptException ex)  // from https://github.com/sebastienros/jint/issues/112
        {
            throw new ApplicationException($"{ex.Error} (Location: {ex.Location})", ex);
        }
    }

    /// <summary>
    /// Evaluate a code and return a value
    /// </summary>
    /// <param name="script"></param>
    /// <param name="discardFromDebugView">true to discard from Debug View, in Debug mode (used to prevent pollution of executed modules in Visual Studio Code)</param>
    public object Evaluate(string script, bool discardFromDebugView = false)
    {
        if (script is null) throw new ArgumentNullException(nameof(script));

        try
        {
            return JintEngine.Evaluate(script)
                .ToObject();  // converts the value to .NET
        }
        catch (Esprima.ParserException ex)
        {
            throw new ApplicationException($"{ex.Error} (Line {ex.LineNumber}, Column {ex.Column}), -> {ReadLine(script, ex.LineNumber)}", ex);
        }
        catch (Jint.Runtime.JavaScriptException ex)  // from https://github.com/sebastienros/jint/issues/112
        {
            throw new ApplicationException($"{ex.Error} (Location: [{ex.Location}])\n" +
                                           $" -> {ReadLine(script, ex.Location.Start.Line)}", ex);
        }
    }

    public void CollectGarbage(bool exhaustive = false)
    {
        throw new NotSupportedException();
    }

    public void Reload()
    {
        // engine settings:
        // strict mode   https://stackoverflow.com/a/34302448   https://stackoverflow.com/questions/34301881/should-i-use-strict-for-every-single-javascript-function-i-write 
        JintEngine = new Engine(cfg =>
        {
            cfg.Strict(true);
        });
        //JintEngine = new Engine();
        if (!(ScriptingContext is null))
            JintEngine.SetValue(_contextName, ScriptingContext);  // pass 'scriptingConnector' to Js

        foreach (var tuple in _savedTypes)
        {
            JintEngine.SetValue(tuple.Name, tuple.Value);
        }
    }

    // used to build the description/title of a script run in debug mode
    private static string ReadAndNormalizeFirstNonEmptyLineOfAScript(string text)
    {
        using var reader = new System.IO.StringReader(text);

        string? line;

        do
        {
            line = reader.ReadLine();
            if (!(string.IsNullOrWhiteSpace(line)))
                return Normalize(line);
        }
        while (line != null);

        return string.Empty;

        // from https://stackoverflow.com/a/3210462/5288052 + https://stackoverflow.com/questions/3210393/how-do-i-remove-all-non-alphanumeric-characters-from-a-string-except-dash
        string Normalize(string stringToNormalize)
        {
            char[] arr = stringToNormalize.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                              || char.IsWhiteSpace(c)
                                              || c == '-'
                                              || c == '_'
                                              || c == '{'
                                              || c == '}'
                                              || c == '['
                                              || c == ']'
                                              || c == '('
                                              || c == ')'
                                              )));
            return new string(arr);
        }
    }

    // used to build error messages
    private static string? ReadLine(string text, int lineNumber)
    {
        using var reader = new System.IO.StringReader(text);

        string? line;
        int currentLineNumber = 0;

        do
        {
            currentLineNumber += 1;
            line = reader.ReadLine();
        }
        while (line != null && currentLineNumber < lineNumber);

        return (currentLineNumber == lineNumber) ? line : string.Empty;
    }
}