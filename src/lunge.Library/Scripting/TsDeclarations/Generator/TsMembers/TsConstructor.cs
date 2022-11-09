using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsConstructor : IMemberWritable
{
    public string[]? Parameters { get; }

    internal TsConstructor(string[]? parameters)
    {
        // constructor(<parameters>);
        Parameters = parameters;
    }

    public string WriteToString()
    {
        var result = new StringBuilder();

        if (Parameters == null)
            result.Append($"constructor()");
        else
            result.Append($"constructor({Parameters.JoinToString()})");

        return result.ToString();
    }

    public override string ToString() => WriteToString();
}