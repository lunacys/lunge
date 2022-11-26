using System.Text;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsEvent : IMemberWritable
{
    public string AddName { get; }
    public string[] AddParameters { get; }
    public string AddReturnTypeName { get; }

    public string RemoveName { get; }
    public string[] RemoveParameters { get; }
    public string RemoveReturnTypeName { get; }

    public TsEvent(
        string addName, 
        string removeName, 
        string[] addParameters,
        string[] removeParameters, 
        string addReturnType, 
        string removeReturnType
        )
    {
        // <Name>(<Parameters>): <ReturnType>;
        AddName = addName;
        RemoveName = removeName;
        AddParameters = addParameters;
        RemoveParameters = removeParameters;
        AddReturnTypeName = addReturnType;
        RemoveReturnTypeName = removeReturnType;
    }

    public string WriteToString()
    {
        var result = new StringBuilder();

        result.AppendLine($"{AddName}({AddParameters.JoinToString()}): {AddReturnTypeName};");
        result.AppendLine($"\t{RemoveName}({RemoveParameters.JoinToString()}): {RemoveReturnTypeName};");

        return result.ToString();
    }

    public override string ToString() => WriteToString();
}