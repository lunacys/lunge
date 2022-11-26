using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsMethod : IMemberWritable
{
    public string Name { get; }
    public string ReturnTypeName { get; }
    public string[] Parameters { get; }
    public string[]? Generics { get; }
    public bool IsAbstract { get; }
    public bool IsOverriden { get; }
    public bool IsStatic { get; }

    internal TsMethod(
        string name, 
        string returnType,
        string[] parameters,
        string[]? generics,
        bool isAbstract, 
        bool isOverriden,
        bool isStatic
        )
    {
        // [abstract|override|static] <Name>[<<Generics>>](<Params>): <ReturnVal>;
        Name = name;
        ReturnTypeName = returnType;
        Parameters = parameters;
        Generics = generics;
        IsAbstract = isAbstract;
        IsOverriden = isOverriden;
        IsStatic = isStatic;
    }

    public string WriteToString()
    {
        var result = new StringBuilder();

        var isAbstractStr = IsAbstract ? "abstract " : "";
        var isStaticStr = IsStatic ? "static " : "";
        var isOverrideStr = IsOverriden ? "override " : "";
        var genericsStr = Generics != null && Generics.Length > 0 ? $"<{string.Join(", ", Generics)}>" : "";

        result.Append(
            $"{isStaticStr}{isAbstractStr}{isOverrideStr}{Name}{genericsStr}({Parameters.JoinToString()}): {ReturnTypeName}"
        );

        return result.ToString();
    }

    public override string ToString() => WriteToString();
}