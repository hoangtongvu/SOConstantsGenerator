using SOConstantsGenerator.Editor.Common;
using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System;
using System.Collections;
using static SOConstantsGenerator.Editor.Utilities;

namespace SOConstantsGenerator.Editor.FieldHandlers.ConstantFieldHandlers;

public class HashMapConstantFieldHandler : IConstantFieldHandler
{
    private IDictionary dictionary;
    private System.Type[] genericArguments;

    public bool CanHandle(CanHandleInput canHandleInput)
    {
        var fieldInfo = canHandleInput.FieldInfo;

        if (fieldInfo.Value is IDictionary dictionary)
        {
            this.dictionary = dictionary;
            this.genericArguments = fieldInfo.Type.GetGenericArguments();
            return true;
        }

        return false;
    }

    public void HandleInLineGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var keyType = this.genericArguments[0];
        var valueType = this.genericArguments[1];

        writer.WriteLine($"public struct {fieldInfo.Name}");
        writer.WriteLine("{");
        writer.Indent();

        GenerateCount(writer);
        GenerateKeyArray(writer, keyType);
        GenerateValueArray(writer, valueType);
        GenerateTryGetValue(writer, keyType, valueType);
        GenerateGetValue(writer, keyType, valueType);
        GenerateContainsKey(writer, keyType);
        GenerateContainsValue(writer, valueType);

        writer.Unindent();
        writer.WriteLine("}");
    }

    private void GenerateKeyArray(CodeWriter writer, Type keyType)
    {
        writer.WriteLine($"public static readonly {keyType}[] Keys = new {keyType}[]");
        writer.WriteLine("{");
        writer.Indent();

        foreach (var entry in this.dictionary.Keys)
        {
            var bytesString = BoxedStructToBytesString(keyType, entry);
            writer.WriteLine($"Unsafe.As<byte, {keyType}>(ref new byte[] {{ {bytesString} }}[0]),");
        }

        writer.Unindent();
        writer.WriteLine("};");
    }

    private void GenerateValueArray(CodeWriter writer, Type valueType)
    {
        writer.WriteLine($"public static readonly {valueType}[] Values = new {valueType}[]");
        writer.WriteLine("{");
        writer.Indent();

        foreach (var entry in this.dictionary.Values)
        {
            var bytesString = BoxedStructToBytesString(valueType, entry);
            writer.WriteLine($"Unsafe.As<byte, {valueType}>(ref new byte[] {{ {bytesString} }}[0]),");
        }

        writer.Unindent();
        writer.WriteLine("};");
    }

    private void GenerateCount(CodeWriter writer)
    {
        writer.WriteLine($"public const int Count = {this.dictionary.Count};");
    }

    private void GenerateTryGetValue(CodeWriter writer, Type keyType, Type valueType)
    {
        writer.WriteLine($"public static bool TryGetValue({keyType} key, out {valueType} value)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine("int keyHash = key.GetHashCode();");
        writer.WriteLine("switch (keyHash)");
        writer.WriteLine("{");
        writer.Indent();

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

        writer.Unindent();
        writer.WriteLine("}");

        writer.WriteLine("value = default;");
        writer.WriteLine("return false;");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateGetValue(CodeWriter writer, Type keyType, Type valueType)
    {
        writer.WriteLine($"public static {valueType} GetValue({keyType} key)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine("if (TryGetValue(key, out var value)) return value;");
        writer.WriteLine("throw new System.Collections.Generic.KeyNotFoundException($\"Key {key} Not Found.\");");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private void GenerateContainsKey(CodeWriter writer, Type keyType)
    {
        writer.WriteLine($"public static bool ContainsKey({keyType} key)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine("int keyHash = key.GetHashCode();");
        writer.WriteLine("return keyHash switch");
        writer.WriteLine("{");
        writer.Indent();

        foreach (var entry in this.dictionary.Keys)
        {
            writer.WriteLine($"{entry.GetHashCode()} => true,");
        }
        writer.WriteLine("_ => false,");

        writer.Unindent();
        writer.WriteLine("};");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateContainsValue(CodeWriter writer, Type valueType)
    {
        writer.WriteLine($"public static bool ContainsValue({valueType} value)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"foreach (var entry in Values)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"if (value.Equals(entry)) return true;");

        writer.Unindent();
        writer.WriteLine("}");

        writer.WriteLine("return false;");

        writer.Unindent();
        writer.WriteLine("}");
    }
}