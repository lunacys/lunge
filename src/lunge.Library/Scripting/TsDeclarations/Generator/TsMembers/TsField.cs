using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsField : IMemberWritable
{
    public string Name { get; }
    public string FieldTypeName { get; }
    public object? DefaultValue { get; }
    public bool IsStatic { get; }
    public bool IsReadOnly { get; }

    internal TsField(string name, string fieldTypeName, object? value, bool isStatic, bool isReadOnly)
    {
        Name = name;
        FieldTypeName = fieldTypeName;
        DefaultValue = value;
        IsStatic = isStatic;
        IsReadOnly = isReadOnly;
    }

    public string WriteToString()
    {
        var result = new StringBuilder();

        var isStaticStr = IsStatic ? "static " : "";
        var isReadOnlyStr = IsReadOnly ? "readonly " : "";
        var typeName = string.IsNullOrEmpty(FieldTypeName) ? "" : $": {FieldTypeName}";
        var defaultValStr = DefaultValue == null ? "" : $" = {DefaultValue}";
        result.Append($"{isStaticStr}{isReadOnlyStr}{Name}{typeName}{defaultValStr}");

        return result.ToString();
    }

    public override string ToString() => WriteToString();
}