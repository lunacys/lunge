using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace lunge.Library.Scripting.TsDeclarations.Generator;

public static class GeneratorUtils
{
    public static string CSharpTypeToTS(string type)
    {
        var noGenerics = RemoveGenerics(type);
        if (noGenerics == "List" || noGenerics == "IEnumerable")
        {
            return type;
        }
        /*if (type.StartsWith("List"))
            return "[]";*/

        if (type == "Single[]")
            return "number[]";
        if (type == "Type") // as typeof operator returns string (or an actual type, but it is irrelevant here)
            return "string";

        type = RemoveGenerics(type);

        return type switch
        {
            "Int32" or "Int64" or "Int16"
                or "Double" or "Single" or "Byte"
                or "UInt32" or "UInt64" or nameof(UInt16) or nameof(SByte) => "number",
            "String" or "Char" => "string",
            nameof(Boolean) => "boolean",
            nameof(Object) => "any",
            "Void" => "void",
            "IEnumerable" => "[]",
            _ => type
        };
    }

    /// <summary>
    /// Gets the Nullable generic argument
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string SolveNullable(Type type)
    {
        return type.GetGenericArguments().First().Name;
    }

    /// <summary>
    /// Removes the ` symbol from the type name
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static string RemoveGenerics(string typeName)
    {
        var index = typeName.IndexOf('`');
        if (index >= 0)
            typeName = typeName.Remove(index);

        return typeName;
    }

    /// <summary>
    /// Solves an Action type to fit TypeScript's style
    /// </summary>
    /// <param name="type"></param>
    /// <param name="solveFunc"></param>
    /// <returns></returns>
    public static string GetGenericParamsForAction(Type type, Func<Type, string>? solveFunc = null)
    {
        var generics = type.GetGenericArguments();
        var genericStrs = new string[generics.Length];

        for (int i = 0; i < genericStrs.Length; i++)
        {
            var g = generics[i];

            if (solveFunc == null)
                genericStrs[i] = $"param{i}: {CSharpTypeToTS(g.Name)}";
            else
                genericStrs[i] = $"param{i}: {solveFunc(g)}";
        }

        return $"({string.Join(", ", genericStrs)}) => void";
    }

    /// <summary>
    /// Solves an EventHandler type to fit TypeScript's style. Differs from Action by having sender parameter.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="sender"></param>
    /// <param name="solveFunc"></param>
    /// <returns></returns>
    public static string GetGenericParamsForEventHandler(Type? type, string sender, Func<Type, string>? solveFunc = null)
    {
        if (type == null)
            return $"(sender: {sender}) => void";

        var generics = type.GetGenericArguments();
        var genericStrs = new string[generics.Length];

        for (int i = 0; i < genericStrs.Length; i++)
        {
            var g = generics[i];

            if (solveFunc == null)
                genericStrs[i] = $"param{i}: {CSharpTypeToTS(g.Name)}";
            else
                genericStrs[i] = $"param{i}: {solveFunc(g)}";
        }

        sender = CSharpTypeToTS(sender);

        return $"(sender: {sender}, {string.Join(", ", genericStrs)}) => void";
    }

    /// <summary>
    /// Solves a Func type to fit TypeScript's style
    /// </summary>
    /// <param name="type"></param>
    /// <param name="solveFunc"></param>
    /// <returns></returns>
    public static string GetGenericParamsForFunc(Type type, Func<Type, string>? solveFunc = null)
    {
        var generics = type.GetGenericArguments();
        var genericStrs = new string[generics.Length - 1];
        Type? returnType = null;

        for (int i = 0; i < generics.Length; i++)
        {
            var g = generics[i];

            if (i == 0)
            {
                returnType = g;
                continue;
            }

            if (solveFunc == null)
                genericStrs[i - 1] = $"param{i - 1}: {CSharpTypeToTS(g.Name)}";
            else
                genericStrs[i - 1] = $"param{i - 1}: {solveFunc(g)}";
        }

        if (solveFunc == null)
            return $"({string.Join(", ", genericStrs)}) => {CSharpTypeToTS(returnType!.Name)}";

        return $"({string.Join(", ", genericStrs)}) => {solveFunc(returnType!)}";
    }

    public static TypeWrapped WrapType(this Type type)
    {
        var attributes = GetAttributeData(type);

        return new TypeWrapped(
            type,
            attributes.PublicName,
            attributes.IsInterface,
            attributes.HasAttributes
        );
    }

    /// <summary>
    /// Retrieves all base types of the specified type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
        if (type.BaseType == null) return type.GetInterfaces();

        return Enumerable.Repeat(type.BaseType, 1)
            .Concat(type.GetInterfaces())
            .Concat(type.GetInterfaces().SelectMany(GetBaseTypes))
            .Concat(GetBaseTypes(type.BaseType))
            .DistinctBy(t => t.Name);
    }

    /// <summary>
    /// Tries to get TsInterface or TsClass attributes and get containing data. If no attribute presents,
    /// returns default Type's parameters.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static (string PublicName, bool IsInterface, bool HasAttributes) GetAttributeData(Type type)
    {
        var attributes = type.GetCustomAttributes();
        var typeName = type.Name;
        var isInterface = type.IsInterface;
        var hasAttributes = false;

        foreach (var attribute in attributes)
        {
            if (attribute is TsInterfaceAttribute a && a.Name != null)
            {
                typeName = a.Name;
                isInterface = true;
                hasAttributes = true;
            }
            else if (attribute is TsClassAttribute c && c.Name != null)
            {
                typeName = c.Name;
                hasAttributes = true;
            }
        }

        return (GeneratorUtils.RemoveGenerics(typeName), isInterface, hasAttributes);
    }

    /// <summary>
    /// Returns a ready-to-use generic declaration string with braces or without ones
    /// </summary>
    /// <param name="type"></param>
    /// <param name="includeBraces"></param>
    /// <param name="solveType"></param>
    /// <returns></returns>
    public static string GetDeclGenericsAsString(
        this Type type, bool includeBraces = true, Func<Type, string>? solveType = null
        )
    {
        if (!type.IsGenericType)
            return "";

        var generics = type
            .GetGenericArguments()
            .Select(g => solveType == null ? CSharpTypeToTS(g.Name) : solveType(g));

        if (includeBraces)
            return $"<{generics.JoinToString()}>";

        return $"{generics.JoinToString()}";
    }

    /// <summary>
    /// Joins an IEnumerable into a string
    /// </summary>
    /// <param name="param"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinToString(this IEnumerable<string> param, string separator = ", ")
    {
        return string.Join(separator, param);
    }

    /// <summary>
    /// Gets default enum values for a type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    public static IEnumerable<KeyValuePair<string, object?>> GetEnumValues(this Type type, FieldInfo[] fields)
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

            yield return new KeyValuePair<string, object?>(f.Name, v);
        }
    }
}