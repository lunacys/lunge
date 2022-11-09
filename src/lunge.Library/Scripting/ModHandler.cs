using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using lunge.Library.Debugging.Logging;
using lunge.Library.Scripting.TsDeclarations.Api.Common;
using lunge.Library.Scripting.TsDeclarations.Api.Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Nez;
using Nez.Systems;
using Nez.UI;
using Direction = Nez.UI.Direction;
using Plane = Microsoft.Xna.Framework.Plane;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace lunge.Library.Scripting;

public class ModHandler
{
    private static readonly string _requireScript = @"
// Setup exports
var exports = {};

// Setup require() func
function setRequire() {
    if (typeof require !== 'undefined') {  // if require is defined, return it
        return require;
    }
    else {  // if require is not defined, build it
        const libraryPath = './lib/';

        if (typeof _scriptingContext === 'undefined') { throw '_scriptingContext not defined'; }

        const readFile = function (name) { return _scriptingContext.ReadFile(name) };

        const requireCache = Object.create(null);

        return function (name) {
            //console.log(`Evaluating file ${name}`);
            if (requiredFileIsALibrary(name)) { name = libraryPath + name; }
            if (requiredFileIsMissingExtension(name)) { name = name + '.js'; }
            if (!(name in requireCache)) {
                //console.log(`${name} is not in cache; reading from disk`);
                let code = readFile(name);
                let module = { exports: {} };
                requireCache[name] = module;
                let wrapper = Function('require, exports, module', code);
                wrapper(require, module.exports, module);
            }

            //console.log(`${name} is in cache. Returning it...`);
            return requireCache[name].exports;
        }

        function requiredFileIsALibrary(name) {
            name = name.trim();
            if (name.startsWith('./')) { return false; }
            if (name.startsWith('../')) { return false; }
            if (name.startsWith('.\\')) { return false; }
            if (name.startsWith('..\\')) { return false; }
            return true;
        }

        function requiredFileIsMissingExtension(name) {
            name = name.trim().toLowerCase();
            if (name.endsWith('.js')) { return false; }
            return true;
        }
    }
}

var require = setRequire();
";

    public string ModFolderPath { get; }

    private NezContentManager _contentManager;

    private readonly JsScriptRunner _scriptRunner;

    private readonly ILogger _logger = LoggerFactory.GetLogger("ModHandler");

    private UICanvas? _canvas;
    private Skin? _skin;

    private readonly List<Type> _customTypesToAdd = new List<Type>();
    private readonly List<(object Object, string Name)> _customObjectsToAdd = new ();

    private bool _includeCommonTypes;

    public ModHandler(string modFolderPath, NezContentManager contentManager, bool includeCommonTypes = true, UICanvas? uiCanvas = null, Skin? skin = null)
    {
        if (modFolderPath.EndsWith("/") || modFolderPath.EndsWith("\\"))
            modFolderPath = modFolderPath.Remove(0, 1);

        ModFolderPath = modFolderPath;
        _contentManager = contentManager;
        _canvas = uiCanvas;
        _skin = skin;
        _includeCommonTypes = includeCommonTypes;
        
        var sc = ScriptingContext.ScriptingContextWithRealFs(ModFolderPath);

        _scriptRunner = JsScriptRunner.RunnerWithContext(sc, "_scriptingContext");
    }

    public void Reload()
    {
        _scriptRunner.Reload();
        // Initialize();
    }

    public void Register(Type type)
    {
        _customTypesToAdd.Add(type);
    }

    public void RegisterObject(object obj, string name)
    {
        _customObjectsToAdd.Add((obj, name));
    }

    public void Execute(string? code = null)
    {
        Initialize();

        if (code == null)
            code = ReadIndexCode();

        _scriptRunner.Run(code);
    }

    public string LoadCode(string filepath)
    {
        return File.ReadAllText(ModFolderPath + "/" + filepath);
    }

    public object Evaluate(string? code = null)
    {
        Initialize();

        if (code == null)
            code = ReadIndexCode();

        return _scriptRunner.Evaluate(code);
    }

    private string ReadIndexCode()
    {
        var indexPath = Path.Combine(ModFolderPath, "index.js");

        if (!File.Exists(indexPath))
            throw new FileNotFoundException("Could not found index.js file: " + indexPath);

        return File.ReadAllText(indexPath);
    }

    public void Initialize()
    {
        InitializeValues();
    }

    public delegate void TestDelegate(int i);
    private void InitializeValues()
    {
        _scriptRunner.Run(_requireScript);
        //_scriptRunnerClearScript.Run(_requireScript);

        if (_includeCommonTypes)
            RegisterCommonTypes();

        // Add custom types
        foreach (var type in _customTypesToAdd)
            _scriptRunner.AddHostObject(type);

        if (_includeCommonTypes)
            RegisterCommonObjects();

        // Add custom objects 
        foreach (var obj in _customObjectsToAdd)
            _scriptRunner.AddHostObject(obj.Object, obj.Name);

        /*var decl = GlobalTimeManager.TimeFunc(() => _scriptRunner.GenerateDeclarationFile(), out var elapsed);

        File.WriteAllText("uilib.d.ts", decl);*/
    }

    public string GenerateDeclarationFile()
    {
        return _scriptRunner.GenerateDeclarationFile();
    }

    private void RegisterCommonTypes()
    {
        var logFunc = new Action<object>(o => _logger.Info(o));

        _scriptRunner.AddHostObject(logFunc, "log");

        var commonTypes = new Type[]
        {
                // structs
                typeof(Matrix),
                typeof(Matrix4x4),
                typeof(Matrix3x2),
                typeof(Matrix2D),
                typeof(Quaternion),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Rectangle),
                typeof(RectangleF),
                typeof(Color),
                typeof(Point),
                typeof(Ray),
                typeof(Ray2D),
                typeof(BoundingBox),
                typeof(Plane),
                typeof(BoundingSphere),
                typeof(TouchLocation),
                // collections
                typeof(TsDictionary<,>),
                // classes
                typeof(BoundingFrustum),
                // enums
                typeof(SpriteEffects),
                typeof(Buttons),
                typeof(Keys),
                typeof(Direction),
                typeof(Edge),
                typeof(VerticalAlign),
                typeof(HorizontalAlign),
                typeof(TouchLocationState),
                typeof(Touchable),
                typeof(ContainmentType),
                typeof(PlaneIntersectionType),
                typeof(Align),
                typeof(SurfaceFormat),
                typeof(DepthFormat),
                typeof(DisplayOrientation)
        };

        var interfaces = new Type[]
        {
                typeof(IFont),
                //typeof(IDrawable),
                typeof(IInputListener),
                typeof(IKeyboardListener),
                typeof(IGamepadFocusable),
                typeof(ICullable),
        };

        var classes = new Type[]
        {
            //typeof(List<>),
            typeof(TsGameScreen),
            typeof(TsScene),
            typeof(TsTexture2D),
            typeof(TsSprite),
            typeof(TsBatcher),
            typeof(TsNezContentManager),
            typeof(ComponentList),
            typeof(Entity),
            typeof(Transform),
            typeof(Component),
            typeof(Camera)
        };

        var typesToRegister = new Type[]
        {
                typeof(BoundingFrustum),
                typeof(Transform),
                typeof(ComponentList),
                typeof(PrimitiveDrawable),

                typeof(Camera),
                typeof(Group),

                typeof(Scene),
                typeof(Component),
                typeof(Dialog),


                typeof(Table),
                typeof(Value),
                typeof(Table.TableDebug),
                typeof(Cell),
                typeof(Label),
                typeof(LabelStyle),


                /*typeof(Button),
                typeof(TextButton),
                typeof(TextButtonStyle),
                typeof(WindowStyle),*/

                typeof(TsTexture2D),
                typeof(TsBatcher),
                typeof(TsBitmapFont),
                typeof(TsNezContentManager),
                typeof(TsSkin),
                typeof(TsSprite),
                typeof(Entity),
                typeof(Stage),
                typeof(TsUiCanvas)
        };

        /*foreach (var type in typesToRegister)
        {
            _scriptRunner.AddHostObject(type);
        }*/

        foreach (var type in commonTypes)
        {
            _scriptRunner.AddHostObject(type);
        }

        foreach (var type in interfaces)
        {
            _scriptRunner.AddHostObject(type);
        }

        foreach (var type in classes)
        {
            _scriptRunner.AddHostObject(type);
        }
    }

    private void RegisterCommonObjects()
    {
        /*if (_canvas != null)
            _scriptRunner.AddHostObject(_canvas, "UiCanvas");
        if (_skin != null)
            _scriptRunner.AddHostObject(_skin, "Skin");*/

        _scriptRunner.AddHostObject(new TsNezContentManager(_contentManager), "ContentManager");
    }

    private static string GetRelativePath(string modFolderPath, string basePath)
    {
        if (basePath.StartsWith("./"))
            basePath = basePath.Remove(0, 2);

        if (basePath.StartsWith("/"))
            basePath = basePath.Remove(0, 1);

        if (!basePath.EndsWith(".js"))
            basePath += ".js";

        return Path.Combine(modFolderPath, basePath);
    }
}