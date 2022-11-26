using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsVariable : IMemberWritable
{
    public string Name { get; }
    public object Object { get; }
    public TsType? TsType { get; }

    private DeclarationFileGenerator _generator;

    public TsVariable(DeclarationFileGenerator generator, string? name, object obj, TsType? tsType)
    {
        _generator = generator;

        if (string.IsNullOrEmpty(name))
            Name = obj.GetType().Name;
        else
            Name = name;

        Object = obj;
        TsType = tsType;
    }

    public string WriteToString()
    {
        var sb = new StringBuilder($"declare var {Name}: ");

        var type = Object.GetType();
        var typeName = _generator.SolveType(type);
        
        sb.Append($"{typeName};");

        return sb.ToString();
    }

    public override string ToString() => WriteToString();
}