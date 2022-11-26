using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMembers;

public class TsType : IMemberWritable
{
    public bool IsInterface { get; }
    public bool IsAbstract { get; }
    public bool IsEnum { get; }
    public bool IsSealed { get; }
    public bool IsGeneric { get; }
    public bool IsValueType { get; }

    public string PublicName => TypeWrapped.PublicName;

    public TypeWrapped? BaseClass { get; private set; }
    public IEnumerable<TypeWrapped> Interfaces { get; private set; }
    
    public IEnumerable<TsField> Fields { get; }
    public IEnumerable<TsProperty> Properties { get; }
    public IEnumerable<TsMethod> Methods { get; }
    public IEnumerable<TsConstructor> Constructors { get; }
    public IEnumerable<TsEvent> Events { get; }

    public TypeWrapped TypeWrapped { get; }

    private DeclarationFileGenerator _generator;

    private static readonly BindingFlags _defaultBindingFlags =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    public static readonly IEnumerable<string> ExcludedInterfaces = new[]
    {
        "IEquatable",
        "IComparable"
    };

    internal TsType(Type baseType, DeclarationFileGenerator generator)
    {
        Interfaces = new List<TypeWrapped>();

        _generator = generator;
        
        TypeWrapped = baseType.WrapType();

        IsInterface = TypeWrapped.IsInterface;
        IsAbstract = TypeWrapped.Type.IsAbstract;
        IsEnum = TypeWrapped.Type.IsEnum;
        IsSealed = TypeWrapped.Type.IsSealed;
        IsGeneric = TypeWrapped.Type.IsGenericType;
        IsValueType = TypeWrapped.Type.IsValueType;

        // Add Fields
        if (!IsInterface)
        {
            if (IsEnum)
            {
                var fieldsRaw = TypeWrapped.Type
                    .GetFields(_defaultBindingFlags)
                    .Where(f => f.Name != "value__")
                    .ToArray();
                var enumVals = TypeWrapped.Type.GetEnumValues(fieldsRaw).ToArray();
                var result = new List<TsField>();

                for (int i = 0; i < fieldsRaw.Length; i++)
                {
                    var valKv = enumVals[i];

                    result.Add(new TsField(valKv.Key, "", valKv.Value, false, false));
                }

                Fields = result;
            }
            else
            {
                Fields = TypeWrapped.Type
                    .GetFields(_defaultBindingFlags)
                    .Select(f =>
                    {
                        var t = _generator.SolveType(f.FieldType);
                        var isStatic = f.IsStatic;
                        var isReadOnly = f.IsInitOnly;

                        return new TsField(f.Name, t, null, isStatic, isReadOnly);
                    })
                    .ToArray();
            }
        }
        else Fields = Array.Empty<TsField>();

        // Add Properties
        if (!IsEnum)
        {
            Properties = TypeWrapped.Type
                .GetProperties(_defaultBindingFlags)
                .Where(p => p.Name != "Item") // Skip 'this' getters and setters
                .Select(p =>
                {
                    var t = _generator.SolveType(p.PropertyType);
                    var isStatic = false;
                    if (p.GetGetMethod() != null)
                        isStatic = p.GetGetMethod()!.IsStatic;
                    if (p.GetSetMethod() != null)
                        isStatic = p.GetSetMethod()!.IsStatic;
                    return new TsProperty(p.Name, t, p.GetMethod != null, p.SetMethod != null, isStatic);
                })
                .ToArray();
        }
        else Properties = Array.Empty<TsProperty>();

        // Add Methods
        if (!IsEnum)
        {
            Methods = TypeWrapped.Type
                .GetMethods(_defaultBindingFlags)
                .Where(m =>
                    !m.Name.StartsWith("get_") && // property getter
                    !m.Name.StartsWith("set_") && // property setter
                    !m.Name.StartsWith("op_") && // operator overrides
                    !m.Name.StartsWith("add_") && // event subscription
                    !m.Name.StartsWith("remove_") && // event unsubscription
                    m.Name != "GetType" &&
                    !m.GetParameters()
                        .Any(p =>
                            p.ParameterType.Name.Contains("&") || // ref/out params
                            p.ParameterType.Name == "StringBuilder"
                            )
                )
                .Select(m =>
                {
                    var parametersRaw = m.GetParameters();
                    var parameters = _generator.SolveParameters(parametersRaw);
                    var generics = m.GetGenericArguments().Select(t => t.Name);

                    return new TsMethod(
                        m.Name,
                        _generator.SolveType(m.ReturnType),
                        parameters,
                        generics.ToArray(),
                        m.IsAbstract && !IsInterface,
                        false, // TODO: This
                        m.IsStatic
                    );
                })
                .ToArray();
        }
        else Methods = Array.Empty<TsMethod>();

        // Add Constructors
        if (!IsEnum && !IsInterface)
        {
            var ctorList = TypeWrapped.Type
                .GetConstructors(_defaultBindingFlags)
                .Select(c =>
                {
                    var parametersRaw = c.GetParameters();
                    var parameters = _generator.SolveParameters(parametersRaw);

                    return new TsConstructor(parameters);
                })
                .ToList();

            if (IsValueType)
                ctorList.Add(new TsConstructor(null));

            Constructors = ctorList.ToArray();
        }
        else Constructors = Array.Empty<TsConstructor>();

        // Add Events
        if (!IsEnum)
        {
            Events = TypeWrapped.Type
                .GetEvents(_defaultBindingFlags)
                .Select(e =>
                {
                    var addMethod = e.GetAddMethod();
                    var removeMethod = e.GetRemoveMethod();

                    return new TsEvent(
                        addMethod.Name,
                        removeMethod.Name,
                        _generator.SolveParameters(addMethod.GetParameters()),
                        _generator.SolveParameters(removeMethod.GetParameters()),
                        _generator.SolveType(addMethod.ReturnType),
                        _generator.SolveType(removeMethod.ReturnType)
                    );
                })
                .ToArray();
        }
        else Events = Array.Empty<TsEvent>();
    }

    public string WriteToString()
    {
        var result = WriteDecl() + " {";

        if (Fields.Any())
        {
            if (!IsEnum) // enums have all members as fields
            {
                result += "\n\t // Fields\n";
                result += Fields.Select(f => $"\t{f};\n").JoinToString("");
            }
            else
            {
                result += "\n";
                result += Fields.Select(f => $"\t{f},\n").JoinToString("");
            }
        }

        if (Properties.Any())
        {
            result += "\n\t // Properties\n";
            result += Properties
                .Select(f => $"\t{f}")
                .JoinToString("");
        }

        if (Methods.Any())
        {
            result += "\n\t // Methods\n";
            result += Methods.Select(f => $"\t{f};\n").JoinToString("");
        }

        if (Constructors.Any())
        {
            result += "\n\t // Constructors\n";
            result += Constructors.Select(f => $"\t{f};\n").JoinToString("");
        }

        if (Events.Any())
        {
            result += "\n\t // Events\n";
            result += Events.Select(f => $"\t{f}\n").JoinToString("");
        }

        result += "\n}\n";

        return result;
    }

    private string WriteDecl()
    {
        var abstractStr = IsAbstract && !IsSealed && !IsInterface ? "abstract " : "";
        string typeStr;
        if (IsEnum) typeStr = "enum";
        else if (IsInterface) typeStr = "interface";
        else typeStr = "class";

        // example: declare abstract class MyClass
        var result = $"declare {abstractStr}{typeStr} {TypeWrapped.PublicName}";

        // example: declare abstract class MyClass<TKey, TValue>
        result += TypeWrapped.Type.GetDeclGenericsAsString();

        if (!IsEnum)
        {
            var baseTypes = TypeWrapped.Type.GetBaseTypes().ToArray();

            if (baseTypes.Length > 0)
            {
                var interfaces = baseTypes
                    .Where(t =>
                        t.IsInterface &&
                        // Don't select excluded interfaces
                        !ExcludedInterfaces.Contains(GeneratorUtils.RemoveGenerics(t.Name)) &&
                        t.IsPublic)
                    .Select(i => $"{GeneratorUtils.RemoveGenerics(i.Name)}{i.GetDeclGenericsAsString()}")
                    .ToArray();

                var baseClass = baseTypes
                    .FirstOrDefault(t => !t.IsInterface &&
                                         // All ByRef types are implicitly derived from Object class
                                         t.Name != "Object" && 
                                         // Skip structs
                                         t.Name != "ValueType");

                // example: declare abstract class MyClass<TKey, TValue> extends MyBaseClass
                if (baseClass != null)
                {
                    var className = GeneratorUtils.RemoveGenerics(baseClass.Name);

                    /*if (_generator.Types.All(t => t.PublicName != className))
                        _generator.AddWarning($"No type registered with name: {className}");*/

                    result += $" extends {className}";
                    result += baseClass.GetDeclGenericsAsString();
                }

                // example:
                // declare abstract class MyClass<TKey, TValue> extends MyBaseClass implements IMyInterface
                if (interfaces.Length > 0)
                {
                    /*foreach (var i in interfaces)
                    {
                        if (!_generator.Types.Any(t => t.PublicName == i))
                            _generator.AddWarning($"No type registered with name {i}");
                    }*/
                    result += $" implements {interfaces.JoinToString()}";
                }
            }
        }

        return result;
    }

    public override string ToString() => WriteToString();
}