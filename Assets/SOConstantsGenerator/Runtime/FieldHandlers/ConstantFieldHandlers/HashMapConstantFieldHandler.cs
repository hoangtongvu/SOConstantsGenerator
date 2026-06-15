using SOConstantsGenerator.Common;
using SOConstantsGenerator.FieldHandlers.Common;
using System;
using System.Collections;
using static SOConstantsGenerator.Common.CodeWriterHelper;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.ConstantFieldHandlers;

public class HashMapConstantFieldHandler : IConstantFieldHandler
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

    public void HandleInLineGeneration(HandleContext handleContext)
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

        GenerateCount(writer);
        GenerateKeyArray(writer, keyType, keyConverterType, destKeyFullName);
        GenerateValueArray(writer, valueType, valueConverterType, destValueFullName);
        GenerateEnumerableProperty(writer, destKeyFullName, destValueFullName);
        GenerateTryGetValue(writer, destKeyFullName, destValueFullName);
        GenerateGetValue(writer, destKeyFullName, destValueFullName);
        GenerateContainsKey(writer, destKeyFullName);
        GenerateContainsValue(writer, destValueFullName);

        writer.Unindent();
        writer.WriteLine("}");
    }

    private void GenerateCount(CodeWriter writer)
    {
        writer.WriteLine($"public const int Count = {this.dictionary.Count};");
    }

    private void GenerateKeyArray(CodeWriter writer, Type keyType, Type keyConverterType, string destKeyFullName)
    {
        writer.WriteLine($"public static readonly {destKeyFullName}[] Keys = new {destKeyFullName}[]");
        using (writer.Block(closing: "};"))
        {
            foreach (var entry in this.dictionary.Keys)
            {
                writer.Write();
                WriteConstValueLiteral(writer, entry, keyType, keyConverterType, punctuation: ",");
            }
        }
    }

    private void GenerateValueArray(CodeWriter writer, Type valueType, Type valueConverterType, string destValueFullName)
    {
        writer.WriteLine($"public static readonly {destValueFullName}[] Values = new {destValueFullName}[]");
        using (writer.Block(closing: "};"))
        {
            foreach (var entry in this.dictionary.Values)
            {
                writer.Write();
                WriteConstValueLiteral(writer, entry, valueType, valueConverterType, punctuation: ",");
            }
        }
    }

    private static void GenerateEnumerableProperty(CodeWriter writer, string destKeyFullName, string destValueFullName)
    {
        // Generate Enumerable
        writer.WriteLine("public static readonly t_Enumerable Enumerable;");

        writer.WriteLine($"public struct t_Enumerable : IEnumerable<KeyValuePair<{destKeyFullName}, {destValueFullName}>>");
        using (writer.Block())
        {
            writer.WriteLine("public Enumerator GetEnumerator() => new();");
            writer.WriteLine($"IEnumerator<KeyValuePair<{destKeyFullName}, {destValueFullName}>> IEnumerable<KeyValuePair<{destKeyFullName}, {destValueFullName}>>.GetEnumerator() {{ throw new NotImplementedException(); }}");
            writer.WriteLine($"IEnumerator IEnumerable.GetEnumerator() {{ throw new NotImplementedException(); }}");
        }

        // Generate Enumerator
        writer.WriteLine($"public struct Enumerator : IEnumerator<KeyValuePair<{destKeyFullName}, {destValueFullName}>>");
        using (writer.Block())
        {
            writer.WriteLine("private int index = -1;");
            writer.WriteLine($"public KeyValuePair<{destKeyFullName}, {destValueFullName}> Current => new(Keys[this.index], Values[this.index]);");
            writer.WriteLine("object IEnumerator.Current => Current;");
            writer.WriteLine("public Enumerator() { }");
            writer.WriteLine("public void Dispose() { }");

            writer.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
            writer.WriteLine("public bool MoveNext() { index++; return index < Count; }");
            writer.WriteLine("public void Reset() => this.index = -1;");
        }
    }

    private void GenerateTryGetValue(CodeWriter writer, string destKeyFullName, string destValueFullName)
    {
        writer.WriteLine($"public static bool TryGetValue({destKeyFullName} key, out {destValueFullName} value)");
        using (writer.Block())
        {
            writer.WriteLine("int keyHash = key.GetHashCode();");
            writer.WriteLine("switch (keyHash)");
            using (writer.Block())
            {
                int keyIndex = 0;
                foreach (var entry in this.dictionary.Keys)
                {
                    writer.WriteLine($"case {entry.GetHashCode()}:");
                    writer.Indent();
                    writer.WriteLine($"value = Values[{keyIndex}];");
                    writer.WriteLine("return true;");
                    writer.Unindent();
                    keyIndex++;
                }
            }

            writer.WriteLine("value = default;");
            writer.WriteLine("return false;");
        }
    }

    private static void GenerateGetValue(CodeWriter writer, string destKeyFullName, string destValueFullName)
    {
        writer.WriteLine($"public static {destValueFullName} GetValue({destKeyFullName} key)");
        using (writer.Block())
        {
            writer.WriteLine("if (TryGetValue(key, out var value)) return value;");
            writer.WriteLine("throw new System.Collections.Generic.KeyNotFoundException($\"Key {key} Not Found.\");");
        }
    }

    private void GenerateContainsKey(CodeWriter writer, string destKeyFullName)
    {
        writer.WriteLine($"public static bool ContainsKey({destKeyFullName} key)");
        using (writer.Block())
        {
            writer.WriteLine("int keyHash = key.GetHashCode();");
            writer.WriteLine("return keyHash switch");
            using (writer.Block(closing: "};"))
            {
                foreach (var entry in this.dictionary.Keys)
                {
                    writer.WriteLine($"{entry.GetHashCode()} => true,");
                }
                writer.WriteLine("_ => false,");
            }
        }
    }

    private static void GenerateContainsValue(CodeWriter writer, string destValueFullName)
    {
        writer.WriteLine($"public static bool ContainsValue({destValueFullName} value)");
        using (writer.Block())
        {
            writer.WriteLine($"foreach (var entry in Values)");
            using (writer.Block())
            {
                writer.WriteLine($"if (value.Equals(entry)) return true;");
            }
            writer.WriteLine("return false;");
        }
    }
}