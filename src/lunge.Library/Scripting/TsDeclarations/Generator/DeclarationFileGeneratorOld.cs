using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lunge.Library.Scripting.TsDeclarations.Generator;

[Obsolete]
public class DeclarationFileGeneratorOld
{
    private readonly HashSet<TypeWrapped> _savedTypes;
    private readonly Dictionary<string, object> _savedObjects;

    private readonly HashSet<string> _savedTypeNames;

    private static readonly string NL = Environment.NewLine;
    private static readonly string Tab = "    ";

    private static readonly HashSet<string> _bannedInterfaces = new HashSet<string>
    {
        "IEquatable",
        "IComparable"
    };

    private static readonly HashSet<string> _excludedMethods = new HashSet<string>
    {
        "GetType",
        "GetHashCode"
    };

    public DeclarationFileGeneratorOld()
    {
        _savedTypes = new HashSet<TypeWrapped>();
        _savedObjects = new Dictionary<string, object>();

        _savedTypeNames = new HashSet<string>();
    }

    public void Register(object obj, string? name = null)
    {
        if (obj is Type type)
        {
            var attrs = GeneratorUtils.GetAttributeData(type);

            _savedTypes.Add(new TypeWrapped(
                type,
                attrs.PublicName,
                attrs.IsInterface,
                attrs.HasAttributes
            ));

            _savedTypeNames.Add(attrs.PublicName);
        }
        else
        {
            var t = obj.GetType();

            if (string.IsNullOrEmpty(name))
                _savedObjects[t.Name] = obj;
            else
                _savedObjects[name] = obj;
        }
    }

    public string Generate()
    {
        var ts = "/* eslint-disable */\n\n";

        foreach (var savedType in _savedTypes)
        {
            ts += WriteType(savedType) + '\n';
        }

        ts += '\n';

        foreach (var savedObject in _savedObjects)
        {
            ts += WriteObject(savedObject.Key, savedObject.Value) + '\n';
        }

        return ts;
    }

    private string WriteType(TypeWrapped type)
    {
        // TODO:
        //  1) Handle 'extends' and 'implements' - DONE
        //  2) Handle methods
        //  3) Handle Properties
        //  4) Handle Generic types (params & methods)
        var result = GetDeclString(type);

        result += " {\n";

        if (!type.IsInterface)
        {
            result += WriteFields(type) + "\n";
        }
        if (!type.Type.IsEnum)
        {
            result += WriteProperties(type) + "\n";
            result += WriteMethods(type) + "\n";
        }

        if (!type.Type.IsEnum && !type.IsInterface && !type.Type.IsAbstract && !type.IsInterface)
        {
            result += WriteConstructors(type) + "\n";
        }

        result += WriteEvents(type) + "\n";

        result += "}\n";

        return result;
    }

    private string GetDeclString(TypeWrapped type)
    {
        var result = "";

        bool any = type.HasAttributes;

        var abstractStr = type.Type.IsAbstract && !type.Type.IsSealed ? "abstract " : "";

        if (!any)
        {
            if (type.Type.IsEnum)
            {
                result = $"declare enum {type.PublicName}";
            }
            else if (type.Type.IsInterface)
            {
                result = $"declare interface {type.PublicName}";
            }
            else
            {
                result = $"declare {abstractStr}class {type.PublicName}";
            }
        }
        else
        {
            if (type.IsInterface)
            {
                result = $"declare interface {type.PublicName}";
            }
            else
            {
                result = $"declare {abstractStr}class {type.PublicName}";
            }
        }


        if (type.Type.IsGenericType)
        {
            var generics = type.Type.GetGenericArguments();
            var genStrs = new List<string>();

            for (int i = 0; i < generics.Length; i++)
            {
                genStrs.Add(generics[i].Name);
            }

            result += $"<{string.Join(", ", genStrs)}>";
        }

        if (!type.Type.IsEnum)
        {
            var baseTypes = type.Type.GetBaseTypes().ToArray();
            if (baseTypes.Any())
            {

                var interfaces = baseTypes
                    .Where(t => t.IsInterface && !_bannedInterfaces.Contains(GeneratorUtils.RemoveGenerics(t.Name)) && t.IsPublic).ToArray();
                var baseClass = baseTypes.Where(t => !t.IsInterface && t.Name != "Object" && t.Name != "ValueType")
                    .ToArray();

                if (baseClass.Length > 0)
                {
                    result += $" extends {baseClass[0].Name}";
                }

                if (interfaces.Length > 0)
                {            var impl = interfaces.Select(t => t.Name);
                    result += $" implements {string.Join(", ", impl)}";
                }
            }
        }

        return result;
    }


    private string WriteObject(string name, object obj)
    {
        var type = obj.GetType();

        if (type.Name.StartsWith("Action"))
        {
            var genericTypes = type.GetGenericArguments();
            var parameters = new List<string>();

            for (int i = 0; i < genericTypes.Length; i++)
            {
                var t = genericTypes[i];
                parameters.Add($"param{i}: {GeneratorUtils.CSharpTypeToTS(t.Name)}");
            }

            return $"declare function {name}({string.Join(", ", parameters)}): void;";
        }

        if (type.Name.StartsWith("Func")) // TODO: Add support for Func<>
        {
            var genericTypes = type.GetGenericArguments();
            var parameters = new List<string>();

            for (int i = 0; i < genericTypes.Length; i++)
            {
                var t = genericTypes[i];
                parameters.Add($"param{i}: {GeneratorUtils.CSharpTypeToTS(t.Name)}");
            }

            return $"declare function {name}({string.Join(", ", parameters)}): void;";
        }

        var objTypeStr = "";
        var returnTypeObj = type;
        var existing = _savedTypes.FirstOrDefault(t => t.Type.Name == returnTypeObj.Name);
        if (existing != null)
        {
            objTypeStr = existing.PublicName;
        }
        else
        {
            objTypeStr = GeneratorUtils.CSharpTypeToTS(type.Name);
        }

        return $"declare var {name}: {objTypeStr};";
    }

    private string WriteEvents(TypeWrapped type)
    {
        var result = "\t // Events\n";

        var events = type.Type.GetEvents();
        var e = type.Type.GetEvent("EventEmpty");
        foreach (var eventInfo in events)
        {
            var sub = eventInfo.AddMethod;
            var unsub = eventInfo.RemoveMethod;
            var raise = eventInfo.RaiseMethod;

            if (sub != null)
            {
                var parameters = sub.GetParameters().Select(p =>
                {
                    var name = p.ParameterType.Name;

                    if (p.ParameterType.IsGenericType && p.ParameterType.Name.StartsWith("EventHandler`"))
                    {
                        name = GeneratorUtils.GetGenericParamsForAction(p.ParameterType);
                    }
                    else if (p.ParameterType.Name == "EventHandler")
                    {
                        name = "() => void";
                    }

                    return $"{p.Name}: {name}";
                });
                var returnType = GeneratorUtils.CSharpTypeToTS(sub.ReturnType.Name);

                result += $"\t{sub.Name}({string.Join(", ", parameters)}): {returnType};\n";
            }

            if (unsub != null)
            {
                var p = unsub.GetParameters();
                var returnType = GeneratorUtils.CSharpTypeToTS(unsub.ReturnType.Name);

                result += $"\t{unsub.Name}(): {returnType};\n";
            }

            if (raise != null)
            {
                result += $"\t{raise.Name}\n";
            }
        }

        return result;
    }

    private string WriteFields(Type type)
    {
        if (type.IsInterface)
            return "";

        var result = Tab + "// Fields\n";

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        if (type.IsEnum)
        {
            var underlyingType = Enum.GetUnderlyingType(type);
            var enumValues = Enum.GetValues(type);
            var length = enumValues.Length;
            var fieldVals = new object?[length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                var value = enumValues.GetValue(i);
                var underlyingValue = Convert.ChangeType(value, underlyingType);
                fieldVals[i] = underlyingValue;
            }

            fields = fields.Where(f => !f.IsSpecialName).ToArray();

            var totalLength = fields.Length;

            for (int i = 0; i < totalLength; i++)
            {
                var f = fields[i];
                var v = fieldVals[i];

                if (v != null)
                    result += $"\t{f.Name} = {v},\n";
                else
                    result += $"\t{f.Name},\n";
            }
        }
        else
        {
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.IsSpecialName)
                    continue;

                var fieldName = fieldInfo.Name;
                var fieldType = GeneratorUtils.CSharpTypeToTS(fieldInfo.FieldType.Name);

                var readonlyStr = fieldInfo.IsInitOnly ? "readonly " : "";

                if (fieldInfo.FieldType.IsGenericType)
                {
                    if (fieldType.StartsWith("Nullable`"))
                    {
                        fieldType = GeneratorUtils.SolveNullable(fieldInfo.FieldType); //fieldInfo.FieldType.GetGenericArguments().First().Name;

                        result += Tab + $"{fieldName}: {fieldType} | null;\n";
                    }
                    else
                    {
                        var genericTypes = fieldInfo.FieldType.GetGenericArguments()
                            .Select(t => $"{GeneratorUtils.CSharpTypeToTS(t.Name)}");

                        result += Tab + $"{fieldName}: {fieldType}<{string.Join(", ", genericTypes)}>;\n";
                    }
                }
                else
                {
                    result += Tab + $"{readonlyStr}{fieldName}: {fieldType};\n";
                }
            }
        }



        return result;
    }

    private string WriteProperties(Type type)
    {
        if (type.IsEnum)
            return "";

        var result = Tab + "// Properties\n";

        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        foreach (var propertyInfo in props)
        {
            if (propertyInfo.Name == "Item")
                continue; // skip 'this' getter and setter

            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();

            var t = GeneratorUtils.CSharpTypeToTS(propertyInfo.PropertyType.Name);

            MethodInfo? method = null;

            if (getMethod != null)
                method = getMethod;
            else if (setMethod != null)
                method = setMethod;

            if (method != null)
            {
                var paramAttrs = method.ReturnParameter.ParameterType.GetCustomAttributes();
                foreach (var attr in paramAttrs)
                {
                    if (attr is TsClassAttribute c && c.Name != null)
                        t = c.Name;
                    else if (attr is TsInterfaceAttribute i && i.Name != null)
                        t = i.Name;
                }
            }

            if (getMethod != null)
            {
                if (getMethod.IsStatic)
                    result += $"\tstatic get {propertyInfo.Name}(): {t};\n";
                else
                    result += $"\tget {propertyInfo.Name}(): {t};\n";
            }

            if (setMethod != null)
            {
                if (setMethod.IsStatic)
                    result += $"\tstatic set {propertyInfo.Name}(value: {t});\n";
                else
                    result += $"\tset {propertyInfo.Name}(value: {t});\n";
            }
        }

        return result;
    }

    private string WriteMethods(TypeWrapped type)
    {
        if (type.Type.IsEnum)
            return "";

        var result = Tab + "// Methods\n";

        var methods = type.Type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        foreach (var methodInfo in methods)
        {
            var paramsRaw = methodInfo.GetParameters().ToArray();
            var anyBannedType = paramsRaw.Any(p =>
                p.ParameterType.Name.Contains("&") || // ref/out params
                p.ParameterType.Name == "StringBuilder"
            );

            if (methodInfo.Name.StartsWith("get_") || // property getter
                methodInfo.Name.StartsWith("set_") || // property setter
                methodInfo.Name.StartsWith("op_") || // operator overrides
                methodInfo.Name.StartsWith("add_") || // event subscription
                methodInfo.Name.StartsWith("remove_") || // event unsub
                anyBannedType ||
                _excludedMethods.Contains(methodInfo.Name)
               )
                continue;

            var parameters = methodInfo.GetParameters()
                .Select(info =>
                {
                    var res = "";
                    if (info.ParameterType.IsGenericType)
                    {
                        var pos = info.ParameterType.Name.IndexOf('`');
                        var n = info.ParameterType.Name.Remove(pos);

                        if (info.ParameterType.Name.StartsWith("Nullable`"))
                        {
                            var paramType = GeneratorUtils.SolveNullable(info.ParameterType);
                            return $"{info.Name}: {paramType} | null";
                        }

                        var genericTypes =
                            info.ParameterType.GenericTypeArguments.Select(t => GeneratorUtils.CSharpTypeToTS(t.Name));

                        if (n == "Action")
                        {
                            n = GeneratorUtils.GetGenericParamsForAction(info.ParameterType);
                            res = $"{info.Name}: {n}";
                        }
                        else if (n == "Func")
                        {
                            n = GeneratorUtils.GetGenericParamsForFunc(info.ParameterType);
                            res = $"{info.Name}: {n}";
                        }
                        else
                        {
                            res = $"{info.Name}: {string.Join(", ", genericTypes)}{GeneratorUtils.CSharpTypeToTS(n)}";
                        }
                    }
                    else
                    {
                        var t = GeneratorUtils.CSharpTypeToTS(info.ParameterType.Name);

                        var paramAttrs = info.ParameterType.GetCustomAttributes();
                        foreach (var attr in paramAttrs)
                        {
                            if (attr is TsClassAttribute c && c.Name != null)
                                t = c.Name;
                            else if (attr is TsInterfaceAttribute i && i.Name != null)
                                t = i.Name;
                        }

                        res += $"{info.Name}: {t}";
                    }

                    return res.Replace("IEnumerable", "[]");
                });

            var returnType = "";
            var returnTypeObj = methodInfo.ReturnType;
            var existing = _savedTypes.FirstOrDefault(t => t.Type.Name == returnTypeObj.Name);
            if (existing != null)
            {
                returnType = existing.PublicName;
            }
            else
            {
                returnType = GeneratorUtils.CSharpTypeToTS(methodInfo.ReturnType.Name);
            }

            if (methodInfo.ReturnType.IsGenericType)
            {
                var index = returnType.IndexOf('`');
                if (index >= 0)
                    returnType = returnType.Remove(index);

                var genericTypes =
                    methodInfo.ReturnType.GenericTypeArguments.Select(t => GeneratorUtils.CSharpTypeToTS(t.Name));

                if (methodInfo.Name == "ToFloatArray")
                {
                    Console.WriteLine();
                }

                returnType = $"{string.Join(", ", genericTypes)}[]";
            }

            result += "\t";

            if (methodInfo.IsStatic)
                result += "static ";

            var methodName = $"{methodInfo.Name}";
            if (methodInfo.IsGenericMethod)
                methodName += "<T>";

            var abstractStr = methodInfo.IsAbstract && !type.Type.IsInterface ? "abstract " : "";
            
            var overrideStr =
                methodInfo.DeclaringType != null && // has declaring type
                _savedTypeNames.Contains(methodInfo.DeclaringType.Name) && type.PublicName != methodInfo.DeclaringType.Name && // is known type
                !_savedTypes.First(t => t.PublicName == methodInfo.DeclaringType.Name).Type.IsInterface // is not an interface's and is not parents' interface's member
                    ? "override "
                    : "";

            result += $"{overrideStr}{abstractStr}{methodName}({string.Join(", ", parameters)}): {returnType};\n";
        }

        return result;
    }

    private string WriteConstructors(Type type)
    {
        if (type.IsEnum || type.IsInterface)
            return "";

        var result = Tab + "// Constructors\n";

        var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        foreach (var ctor in ctors)
        {
            var parameters = ctor.GetParameters();
            var paramStrs = parameters.Select(info =>
            {
                var n = GeneratorUtils.CSharpTypeToTS(info.ParameterType.Name);

                if (n.StartsWith("Nullable`"))
                {
                    n = GeneratorUtils.SolveNullable(info.ParameterType);
                    return $"{info.Name}?: {n} | null";
                }

                var existing = _savedTypes.FirstOrDefault(t => t.Type.Name == info.ParameterType.Name);
                if (existing != null)
                {
                    n = existing.PublicName;
                }

                return $"{info.Name}: {n}";
            });

            result += $"\tconstructor({string.Join(", ", paramStrs)});\n";
        }

        if (type.IsValueType)
        {
            // write default struct constructor
            result += $"\tconstructor();\n";
        }

        return result;
    }
}