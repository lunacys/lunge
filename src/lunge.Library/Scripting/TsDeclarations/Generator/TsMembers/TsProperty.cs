using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsProperty
{
    public string Name { get; }
    public string PropTypeName { get; }
    public bool HasGet { get; }
    public bool HasSet { get; }
    public bool IsStatic { get; }

    internal TsProperty(string name, string propTypeName, bool hasGet, bool hasSet, bool isStatic)
    {
        Name = name;
        PropTypeName = propTypeName;
        HasGet = hasGet;
        HasSet = hasSet;
        IsStatic = isStatic;
    }

    public string WriteToString()
    {
        var result = new StringBuilder();
        var isStaticStr = IsStatic ? "static " : "";

        if (HasGet)
        {
            result.AppendLine($"{isStaticStr}get {Name}(): {PropTypeName};");
        }

        if (HasSet)
        {
            result.AppendLine($"\t{isStaticStr}set {Name}(value: {PropTypeName});");
        }

        return result.ToString();
    }

    public override string ToString() => WriteToString();
}