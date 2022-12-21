namespace lunge.Library.Scripting.TsDeclarations;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class TsIncludeAttribute : Attribute
{
    public TsIncludeAttribute()
    { }
}

/// <summary>
/// Exclude type or member from processing
/// </summary>
[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class TsExcludeAttribute : Attribute
{
    public TsExcludeAttribute()
    { }
}

/// <summary>
/// The annotated class, struct or interface will be declared as TS interface. Constructor will not be included.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class TsInterfaceAttribute : Attribute
{
    public Type? OriginType { get; init; }
    public string? Name { get; init; }

    public TsInterfaceAttribute(Type? originType = null, string? name = null)
    {
        OriginType = originType;
        Name = name;
    }
}

/// <summary>
/// The annotated class or struct will be declared as TS class. Constructor will be included.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class TsClassAttribute : Attribute
{
    public Type? OriginType { get; init; }
    public string? Name { get; init; }

    public TsClassAttribute(Type? originType = null, string? name = null)
    {
        OriginType = originType;
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class TsProxyAttribute : Attribute
{
    public Type OriginType { get; init; }

    public TsProxyAttribute(Type originType)
    {
        OriginType = originType;
    }
}