using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsMethod : IMemberWritable
{
    public string Name { get; }
    public string ReturnTypeName { get; }
    public string[] Parameters { get; }
    public bool IsAbstract { get; }
    public bool IsOverriden { get; }
    public bool IsStatic { get; }

    internal TsMethod(
        string name, 
        string returnType,
        string[] parameters,
        bool isAbstract, 
        bool isOverriden,
        bool isStatic
        )
    {
        // [abstract|override|static] <Name>(<Params>): <ReturnVal>;
        Name = name;
        ReturnTypeName = returnType;
        Parameters = parameters;
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

        result.Append(
            $"{isStaticStr}{isAbstractStr}{isOverrideStr}{Name}({Parameters.JoinToString()}): {ReturnTypeName}"
        );

        return result.ToString();
    }

    public override string ToString() => WriteToString();
}