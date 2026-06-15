using SOConstantsGenerator.Common;
using SOConstantsGenerator.FieldHandlers.Common;
using System.Collections;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.DynamicFieldHandlers;

public class HashMapDynamicFieldHandler : IDynamicFieldHandler
{
    private IDictionary dictionary;
    private System.Type[] genericArguments;

    public bool CanHandle(CanHandleContext canHandleContext)
    {
        var fieldInfo = canHandleContext.FieldInfo;

        if (fieldInfo.Value is IDictionary dictionary)
        {
            this.dictionary = dictionary;
            this.genericArguments = fieldInfo.Type.GetGenericArguments();
            return true;
        }

        return false;
    }

    public void HandleDeclarationGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;

        var keyType = this.genericArguments[0];
        var keyConverterType = handleContext.ConverterTypes?[0];
        if (!TryGetSourceAndDestTypes(keyConverterType, out _, out var destKeyType))
            destKeyType = keyType;
        var destKeyFullName = GetCSharpFullName(destKeyType);

        var valueType = this.genericArguments[1];
        var valueConverterType = handleContext.ConverterTypes?[1];
        if (!TryGetSourceAndDestTypes(valueConverterType, out _, out var destValueType))
            destValueType = valueType;
        var destValueFullName = GetCSharpFullName(destValueType);

        writer.WriteLine($"public struct {fieldInfo.Name}");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"public static Dictionary<{destKeyFullName}, {destValueFullName}> _InternalDictionary;");
        writer.WriteLine($"public static int Count;");
        writer.WriteLine($"public static {destKeyFullName}[] Keys;");
        writer.WriteLine($"public static {destValueFullName}[] Values;");
        GenerateEnumerableProperty(writer, destKeyFullName, destValueFullName);
        GenerateTryGetValue(writer, destKeyFullName, destValueFullName);
        GenerateGetValue(writer, destKeyFullName, destValueFullName);
        GenerateContainsKey(writer, destKeyFullName);
        GenerateContainsValue(writer, destValueFullName);

        writer.Unindent();
        writer.WriteLine("}");
    }

    public void HandleAssignmentGeneration(HandleContext handleContext)
    {
        var writer = handleContext.Writer;
        var fieldInfo = handleContext.FieldInfo;

        var keyConverterType = handleContext.ConverterTypes?[0];
        var valueConverterType = handleContext.ConverterTypes?[1];

        writer.WriteLine($"{fieldInfo.Name}.Count = so.{fieldInfo.Name}.Count;");

        writer.Write($"{fieldInfo.Name}.Keys = ");
        writer.WriteLineNoIndent(keyConverterType == null
            ? $"so.{fieldInfo.Name}.Keys.ToArray().ToArray();"
            : $"SOConstantsGenerator.ITypeConverter.Convert(so.{fieldInfo.Name}.Keys.ToArray(), new {GetCSharpFullName(keyConverterType)}());"
        );

        writer.Write($"{fieldInfo.Name}.Values = ");
        writer.WriteLineNoIndent(valueConverterType == null
            ? $"so.{fieldInfo.Name}.Values.ToArray().ToArray();"
            : $"SOConstantsGenerator.ITypeConverter.Convert(so.{fieldInfo.Name}.Values.ToArray(), new {GetCSharpFullName(valueConverterType)}());"
        );

        writer.WriteLine($"{fieldInfo.Name}._InternalDictionary = {fieldInfo.Name}.Keys.Zip({fieldInfo.Name}.Values, (k, v) => new {{k,v}}).ToDictionary(pair => pair.k, pair => pair.v);");
    }

    private static void GenerateEnumerableProperty(CodeWriter writer, string destKeyFullName, string destValueFullName)
    {
        // Generate Enumerable
        writer.WriteLine("public static readonly t_Enumerable Enumerable;");

        writer.WriteLine($"public struct t_Enumerable : IEnumerable<KeyValuePair<{destKeyFullName}, {destValueFullName}>>");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"public IEnumerator<KeyValuePair<{destKeyFullName}, {destValueFullName}>> GetEnumerator() => _InternalDictionary.GetEnumerator();");
        writer.WriteLine("IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateTryGetValue(CodeWriter writer, string destKeyFullName, string destValueFullName)
    {
        writer.WriteLine($"public static bool TryGetValue({destKeyFullName} key, out {destValueFullName} value)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"return _InternalDictionary.TryGetValue(key, out value);");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateGetValue(CodeWriter writer, string destKeyFullName, string destValueFullName)
    {
        writer.WriteLine($"public static {destValueFullName} GetValue({destKeyFullName} key)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine("if (TryGetValue(key, out var value)) return value;");
        writer.WriteLine("throw new System.Collections.Generic.KeyNotFoundException($\"Key {key} Not Found.\");");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateContainsKey(CodeWriter writer, string destKeyFullName)
    {
        writer.WriteLine($"public static bool ContainsKey({destKeyFullName} key)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"return _InternalDictionary.ContainsKey(key);");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateContainsValue(CodeWriter writer, string destValueFullName)
    {
        writer.WriteLine($"public static bool ContainsValue({destValueFullName} value)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"return _InternalDictionary.ContainsValue(value);");

        writer.Unindent();
        writer.WriteLine("}");
    }
}