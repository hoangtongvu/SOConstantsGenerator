using SOConstantsGenerator.Editor.Common;
using SOConstantsGenerator.Editor.FieldHandlers.Common;
using System;
using System.Collections;

namespace SOConstantsGenerator.Editor.FieldHandlers.DynamicFieldHandlers;

public class HashMapDynamicFieldHandler : IDynamicFieldHandler
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

    public void HandleDeclarationGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var keyType = this.genericArguments[0];
        var valueType = this.genericArguments[1];

        writer.WriteLine($"public struct {fieldInfo.Name}");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"public static Dictionary<{keyType}, {valueType}> _InternalDictionary;");
        writer.WriteLine($"public static int Count;");
        writer.WriteLine($"public static {keyType}[] Keys;");
        writer.WriteLine($"public static {valueType}[] Values;");
        GenerateTryGetValue(writer, keyType, valueType);
        GenerateGetValue(writer, keyType, valueType);
        GenerateContainsKey(writer, keyType);
        GenerateContainsValue(writer, valueType);

        writer.Unindent();
        writer.WriteLine("}");
    }

    public void HandleAssignmentGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var keyType = this.genericArguments[0];
        var valueType = this.genericArguments[1];

        writer.WriteLine($"{fieldInfo.Name}._InternalDictionary = so.{fieldInfo.Name};");
        writer.WriteLine($"{fieldInfo.Name}.Count = so.{fieldInfo.Name}.Count;");
        writer.WriteLine($"{fieldInfo.Name}.Keys = so.{fieldInfo.Name}.Keys.ToArray();");
        writer.WriteLine($"{fieldInfo.Name}.Values = so.{fieldInfo.Name}.Values.ToArray();");
    }

    private static void GenerateTryGetValue(CodeWriter writer, Type keyType, Type valueType)
    {
        writer.WriteLine($"public static bool TryGetValue({keyType} key, out {valueType} value)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"return _InternalDictionary.TryGetValue(key, out value);");

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

    private static void GenerateContainsKey(CodeWriter writer, Type keyType)
    {
        writer.WriteLine($"public static bool ContainsKey({keyType} key)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"return _InternalDictionary.ContainsKey(key);");

        writer.Unindent();
        writer.WriteLine("}");
    }

    private static void GenerateContainsValue(CodeWriter writer, Type valueType)
    {
        writer.WriteLine($"public static bool ContainsValue({valueType} value)");
        writer.WriteLine("{");
        writer.Indent();

        writer.WriteLine($"return _InternalDictionary.ContainsValue(value);");

        writer.Unindent();
        writer.WriteLine("}");
    }
}