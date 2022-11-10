using System.Dynamic;
using System.Reflection;
using Jint.Native;
using lunge.DtsGenerator.TestTypes;
using lunge.Library.Debugging.Profiling;
using lunge.Library.Scripting;
using lunge.Library.Scripting.TsDeclarations.Api.Common;
using lunge.Library.Scripting.TsDeclarations.Api.Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace lunge.DtsGenerator;

public class GeneratorTests : Scene
{
    private ModHandler _modHandler;

    private string _directory = "./Scripts/lib/";

    public GeneratorTests()
    {
        
    }

    public override void Initialize()
    {
        base.Initialize();

        _modHandler = new ModHandler("./Scripts", Content, false);
        SaveGenerated();
    }

    public void SaveGenerated()
    {
        /*_modHandler.Register(typeof(TestCollectionOne<>));
        _modHandler.Register(typeof(TestCollectionTwo<,>));
        _modHandler.Register(typeof(TestCollectionConstraints1<>));

        _modHandler.Register(typeof(ITestInterface1));
        //_modHandler.Register(typeof(ITestInterface2));
        _modHandler.Register(typeof(TestAbstractClass));
        //_modHandler.Register(typeof(TestInheritance));
        _modHandler.Register(typeof(TestInheritance2));
        
        _modHandler.Register(typeof(TestEventObj));
        _modHandler.Register(typeof(TestEvents));*/
        
       /*
        foreach (var type in types)
        {
            newGenerator.Register(type);
        }

        newGenerator.Register(new Action<object>(Console.WriteLine), "log");
        newGenerator.Register(new Func<int, string>(i => $"The number is: {i}"), "getNumberFunc");
        newGenerator.Register(new TestInheritance<object>(), "TestInh1");
        newGenerator.Register(new TestInheritance2(), "TestInh2");*/

        /*foreach (var tsType in types)
        {
            Console.WriteLine(tsType);
        }*/
        /*var time = Debug.TimeAction(() =>
        {
            Console.WriteLine(newGenerator.Generate());
        });

        Console.WriteLine($"TIME SPENT: {time.TotalMilliseconds} ms");
        
        Console.WriteLine("Warnings:\n " + newGenerator.Warnings.Select(w => $" > {w}\n").JoinToString());*/

        //_modHandler.Register(typeof(TestEventHandling));
        _modHandler.Register(typeof(ModEntry));
        _modHandler.Register(typeof(TsBatcher));
        //_modHandler.Register(typeof(TsSprite));
        //_modHandler.Register(typeof(TsTexture2D));
        //_modHandler.Register(typeof(TsBatcher));
        _modHandler.Register(typeof(Color));
        _modHandler.Register(typeof(Point));
        _modHandler.Register(typeof(Vector2));
        _modHandler.Register(typeof(Rectangle));
        _modHandler.Register(typeof(RectangleF));

        var assembly = Assembly.GetAssembly(typeof(Core))!;
        var uiTypes = assembly
            .GetTypes()
            .Where(t => t.Namespace != null && t.Namespace.StartsWith("Nez.UI") && t.IsPublic)
            .DistinctBy((k) => k.Name)
            .ToList();

        var allTypes = new List<Type>();

        foreach (var type in uiTypes)
        {
            var methods = type.GetMethods();
            var fields = type.GetFields();
            
            allTypes.Add(type);
        }

        foreach (var type in allTypes)
        {
            _modHandler.Register(type);
        }
        
        _modHandler.Initialize();
  
        var decl = GlobalTimeManager.TimeFunc(() => _modHandler.GenerateDeclarationFile(), out var elapsed);

        Console.WriteLine($"Generation Done in {elapsed.TotalMilliseconds} ms");

        Directory.CreateDirectory(_directory);
        File.WriteAllText(_directory + "ui.d.ts", decl);

        Core.Exit();
        return;
        try
        {
            var code = _modHandler.LoadCode("/dist/index.js");
            // Func<JsValue, JsValue[], JsValue>
            var res = (ExpandoObject)_modHandler.Evaluate(code);
            Func<JsValue, JsValue[], JsValue>? init = null;
            Func<JsValue, JsValue[], JsValue>? update = null;
            Func<JsValue, JsValue[], JsValue>? render = null;
            ExpandoObject? entry = null;

            foreach (var kv in res)
            {
                if (kv.Key == "Init")
                    init = (Func<JsValue, JsValue[], JsValue>)kv.Value;
                else if (kv.Key == "Update")
                    update = (Func<JsValue, JsValue[], JsValue>) kv.Value;
                else if (kv.Key == "Render")
                    render = (Func<JsValue, JsValue[], JsValue>) kv.Value;
                else if (kv.Key == "Entry")
                    entry = (ExpandoObject)kv.Value;
            }

            var batcher = new TsBatcher();
            init(JsValue.Null, new JsValue[] { JsValue.Null });
            update(JsValue.Null, new JsValue[] { JsValue.Null, });
            // render(JsValue.Null, new JsValue[] { (JsValue)(object)batcher});

            //var a = res(JsValue.Null, new JsValue[] {"test" });
            //var a = res(JsValue.Undefined, null);
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Debug.DrawText($"{e.Message}", Color.Black, 10f);
        }
    }

    public override void Update()
    {
        if (Input.IsKeyPressed(Keys.Space))
        {
            ReloadScripts();
        }

        base.Update();
    }

    private void ReloadScripts()
    {
        try
        {
            _modHandler.Reload();
            var code = _modHandler.LoadCode("/dist/index.js");
            _modHandler.Execute(code);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Debug.DrawText($"{e.Message}", Color.Black, 10f);
        }
    }

    public class ModEntry
    {
        public virtual void Initialize() {}

        public (string Asd, int Woah) TestTuple;
    }

    public class TestEventHandling
    {
        public event EventHandler<string>? MyEvent;
        public event EventHandler? MyEvent2; 

        public void InvokeEvent(string str)
        {
            MyEvent?.Invoke(this, str);
        }

        public void InvokeEvent2()
        {
            MyEvent2?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return "I am TestEventHandling!";
        }
    }
}