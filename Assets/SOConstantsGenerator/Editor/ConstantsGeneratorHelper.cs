using SOConstantsGenerator.Editor.Common;
using System.Collections;
using System.Collections.Generic;
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

        using var writer = new CodeWriter(new(outputPath, false));

        writer.WriteLine("// This file is auto-generated, do not change.");
        writer.WriteLine("using UnityEditor;");
        writer.WriteLine("using UnityEngine;");
        writer.WriteLine();
        writer.WriteLine($"namespace {classNamespace};");
        writer.WriteLine();

        writer.WriteLine("public static class " + className);
        writer.WriteLine("{");
        writer.Indent();

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
                writer.WriteLine($"public static {elementType}[] {field.Name};");
            }
            else
            {
                // Handle normal structs
                writer.WriteLine($"public static {fieldType} {field.Name};");
            }
        }

        writer.Unindent();
        writer.WriteLine();
        writer.WriteLine("#if UNITY_EDITOR");

        writer.Indent();
        writer.WriteLine("[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]");
        writer.WriteLine("public static void OnLoad() => LoadStaticFields();");
        writer.Unindent();
        writer.WriteLine("#endif");

        writer.WriteLine();
        writer.Indent();
        writer.WriteLine($"private static void LoadStaticFields()");
        writer.WriteLine("{");

        writer.Indent();
        writer.WriteLine($"var so = ({soType})EditorUtility.InstanceIDToObject({so.GetInstanceID()});");

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
                writer.WriteLine($"{field.Name} = so.{field.Name};");
            }
        }

        writer.Write(ConstantArraysGeneratorHelper.GetDynamicAssignments(writer.IndentLevel, arrayFieldInfoList));

        writer.Unindent();
        writer.WriteLine("}");

        writer.Unindent();
        writer.WriteLine("}");
        writer.Flush();
    }

    public static void GenerateConstantsFile(string outputPath, Object so, System.Type soType, string className, string classNamespace)
    {
        var fields = soType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);
        var arrayFieldInfoList = new List<MyFieldInfo>();

        using var writer = new CodeWriter(new(outputPath, false));

        writer.WriteLine("// This file is auto-generated, do not change.");
        writer.WriteLine($"using System.Runtime.CompilerServices;");
        writer.WriteLine();
        writer.WriteLine($"namespace {classNamespace};");
        writer.WriteLine();

        writer.WriteLine("public static class " + className);
        writer.WriteLine("{");
        writer.Indent();

        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            var value = field.GetValue(so);

            if (CanBeConst(fieldType))
            {
                // Handle constants
                writer.WriteLine($"public const {fieldType} {field.Name} = {FormatValue(value)};");
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
                    writer.WriteLine($"public static readonly {fieldType} {field.Name} =");
                    writer.Indent();
                    writer.WriteLine($"Unsafe.As<byte, {fieldType}>(ref new byte[] {{ {bytesString} }}[0]);");
                    writer.Unindent();
                }
            }
        }

        writer.Write(ConstantArraysGeneratorHelper.GetHardCodedArraysInitialization(writer.IndentLevel, arrayFieldInfoList));

        writer.Unindent();
        writer.WriteLine("}");
        writer.Flush();
    }
}