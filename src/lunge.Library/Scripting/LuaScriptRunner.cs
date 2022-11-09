using System;

namespace lunge.Library.Scripting;

public enum LuaRunnerType
{
    MoonSharp,
    NLua
}

public class LuaScriptRunner
{
    // private Script _workingScript;
    private LuaRunnerType _type;

    //private Lua _nLua;

    public LuaScriptRunner(LuaRunnerType type)
    {
        _type = type;

        Init();
    }

    public void Run(string code)
    {
        switch (_type)
        {
            case LuaRunnerType.MoonSharp:
                //_workingScript.DoString(code);
                break;
            case LuaRunnerType.NLua:
                //_nLua.DoString(code);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Register(string name, object obj)
    {
        switch (_type)
        {
            case LuaRunnerType.MoonSharp:
                //_workingScript.Globals[name] = obj;
                break;
            case LuaRunnerType.NLua:
                //_nLua[name] = obj;
                //_nLua["io"] = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Init()
    {
        switch (_type)
        {
            case LuaRunnerType.MoonSharp:
                //_workingScript = new Script(CoreModules.Preset_HardSandbox);
                break;
            case LuaRunnerType.NLua:
                //_nLua = new Lua();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
        }
    }
}