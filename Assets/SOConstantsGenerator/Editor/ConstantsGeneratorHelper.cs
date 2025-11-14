using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static SOConstantsGenerator.Editor.Utilities;

namespace SOConstantsGenerator.Editor;

public static class ConstantsGeneratorHelper
{
    public static void GenerateDynamicFieldsFile(string outputPath, Object so, System.Type soType, string className, string classNamespace)
    {
        var fields = soType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);
        var arrayFieldInfoList = new List<MyFieldInfo>();

        int indentLevel = 0;
        using var writer = new StreamWriter(outputPath, false);
        void AddLine(string line) => writer.WriteLine(new string(' ', indentLevel * 4) + line);
        void AddEmptyLine() => writer.WriteLine();

        AddLine("// This file is auto-generated, do not change.");
        AddLine($"using UnityEditor;");
        AddLine($"using UnityEngine;");
        AddEmptyLine();
        AddLine($"namespace {classNamespace};");
        AddEmptyLine();

        AddLine("public static class " + className);
        AddLine("{");
        indentLevel++;

        // Generate Declarations
        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            var value = field.GetValue(so);

            if (value is IEnumerable enumerable)
            {
                // Handle Array/Lists
                var elementType = fieldType.IsArray
                    ? fieldType.GetElementType()
                    : fieldType.GetGenericArguments()[0];
                AddLine($"public static {elementType}[] {field.Name};");
            }
            else
            {
                // Handle normal structs
                AddLine($"public static {fieldType} {field.Name};");
            }
        }

        indentLevel--;
        AddEmptyLine();
        AddLine("#if UNITY_EDITOR");

        indentLevel++;
        AddLine("[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]");
        AddLine("public static void OnLoad() => LoadStaticFields();");
        indentLevel--;
        AddLine("#endif");

        AddEmptyLine();
        indentLevel++;
        AddLine($"private static void LoadStaticFields()");
        AddLine("{");

        indentLevel++;
        AddLine($"var so = ({soType})EditorUtility.InstanceIDToObject({so.GetInstanceID()});");

        // Generate Assignments
        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            var value = field.GetValue(so);

            if (value is IEnumerable enumerable)
            {
                // Handle Array/Lists
                arrayFieldInfoList.Add(new()
                {
                    Name = field.Name,
                    Type = fieldType,
                    Value = value,
                });
            }
            else
            {
                // Handle normal structs
                AddLine($"{field.Name} = so.{field.Name};");
            }
        }

        writer.Write(ConstantArraysGeneratorHelper.GetDynamicAssignments(indentLevel, arrayFieldInfoList));

        indentLevel--;
        AddLine("}");

        indentLevel--;
        AddLine("}");
        writer.Flush();
    }

    public static void GenerateConstantsFile(string outputPath, Object so, System.Type soType, string className, string classNamespace)
    {
        var fields = soType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);
        var arrayFieldInfoList = new List<MyFieldInfo>();

        int indentLevel = 0;
        using var writer = new StreamWriter(outputPath, false);
        void AddLine(string line) => writer.WriteLine(new string(' ', indentLevel * 4) + line);
        void AddEmptyLine() => writer.WriteLine();

        AddLine("// This file is auto-generated, do not change.");
        AddLine($"using System.Runtime.CompilerServices;");
        AddEmptyLine();
        AddLine($"namespace {classNamespace};");
        AddEmptyLine();

        AddLine("public static class " + className);
        AddLine("{");
        indentLevel++;

        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            var value = field.GetValue(so);

            if (CanBeConst(fieldType))
            {
                // Handle constants
                AddLine($"public const {fieldType} {field.Name} = {FormatValue(value)};");
            }
            else
            {
                // Handle static readonly
                if (value is IEnumerable enumerable)
                {
                    // Handle Array/Lists
                    arrayFieldInfoList.Add(new()
                    {
                        Name = field.Name,
                        Type = fieldType,
                        Value = value,
                    });
                }
                else
                {
                    // Handle normal structs
                    var bytesString = BoxedStructToBytesString(fieldType, value);
                    AddLine($"public static readonly {fieldType} {field.Name} =");
                    indentLevel++;
                    AddLine($"Unsafe.As<byte, {fieldType}>(ref new byte[] {{ {bytesString} }}[0]);");
                    indentLevel--;
                }
            }
        }

        writer.Write(ConstantArraysGeneratorHelper.GetHardCodedArraysInitialization(indentLevel, arrayFieldInfoList));

        indentLevel--;
        AddLine("}");
        writer.Flush();
    }
}