namespace lunge.Library.Scripting.TsDeclarations.Generator;

public class TypeWrapped
{
    public Type Type { get; }
    public string PublicName { get; }
    public bool IsInterface { get; }
    public bool HasAttributes { get; }

    public TypeWrapped(Type type, string publicName, bool isInterface, bool hasAttributes)
    {
        Type = type;
        PublicName = publicName;
        IsInterface = isInterface;
        HasAttributes = hasAttributes;
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode() + PublicName.GetHashCode() + IsInterface.GetHashCode() + HasAttributes.GetHashCode();
    }

    public static implicit operator Type(TypeWrapped wrapped) => wrapped.Type;
}