using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using lunge.Library.Scripting;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace lunge.UiEditor.Components;

public class EditorUiViewerComponent : Component
{
    private UICanvas _canvas;
    private ModHandler? _modHandler;

    public override void OnAddedToEntity()
    {
        Reset();
    }

    public void Apply(string code)
    {
        try
        {
            InitializeEngine();
            Reset();
            _modHandler!.Execute(code);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void Reset()
    {
        Entity.RemoveComponent<UICanvas>();
        _canvas = null;
        _canvas = Entity.AddComponent(new UICanvas());
        _canvas.IsFullScreen = true;

        _canvas.DebugRenderEnabled = true;
        
    }

    private void InitializeEngine()
    {
        if (_modHandler == null)
        {
            //Directory.CreateDirectory("/scripts");
            _modHandler = new ModHandler(".", Core.Content, false);

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
            
            _modHandler.RegisterObject(_canvas.Stage, "MainStage");

            _modHandler.Initialize();
        }
        else
        {
            _modHandler.Reload();
        }
    }
}