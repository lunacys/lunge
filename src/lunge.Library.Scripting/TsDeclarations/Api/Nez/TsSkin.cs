using Nez.UI;

namespace lunge.Library.Scripting.TsDeclarations.Api.Nez;

[TsInterface(typeof(Skin), "Skin")]
public class TsSkin
{
    [TsExclude]
    internal Skin OrigSkin { get; set; }

    public T Get<T>() => OrigSkin.Get<T>();
    public T Get<T>(string name) => OrigSkin.Get<T>(name);
    public T Add<T>(string name, T resource) => OrigSkin.Add<T>(name, resource);
}