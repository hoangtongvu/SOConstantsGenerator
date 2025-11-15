using SOConstantsGenerator.Editor.Common;
using SOConstantsGenerator.Editor.FieldHandlers.Common;
using SOConstantsGenerator.Editor.FieldProcessors;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SOConstantsGenerator.Editor;

public static class ConstantsGeneratorHelper
{
    public static void GenerateDynamicFieldsFile(string outputPath, Object so, System.Type soType, string className, string classNamespace)
    {
        var fields = soType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);
        var fieldProcessor = new DynamicFieldProcessor();
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

            var fieldInfo = new MyFieldInfo
            {
                Name = field.Name,
                Type = fieldType,
                Value = value,
            };

            var canHandleInput = new CanHandleInput
            {
                FieldInfo = fieldInfo,
            };

            var handleInput = new HandleInput
            {
                Writer = writer,
                FieldInfo = fieldInfo,
            };

            fieldProcessor.ProcessDeclaration(canHandleInput, handleInput);
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

            var fieldInfo = new MyFieldInfo
            {
                Name = field.Name,
                Type = fieldType,
                Value = value,
            };

            var canHandleInput = new CanHandleInput
            {
                FieldInfo = fieldInfo,
            };

            var handleInput = new HandleInput
            {
                Writer = writer,
                FieldInfo = fieldInfo,
            };

            fieldProcessor.ProcessAssignment(canHandleInput, handleInput);
        }

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
        var fieldProcessor = new ConstantFieldProcessor();
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

            var fieldInfo = new MyFieldInfo
            {
                Name = field.Name,
                Type = fieldType,
                Value = value,
            };

            var canHandleInput = new CanHandleInput
            {
                FieldInfo = fieldInfo,
            };

            var handleInput = new HandleInput
            {
                Writer = writer,
                FieldInfo = fieldInfo,
            };

            fieldProcessor.Process(canHandleInput, handleInput);
        }

        writer.Unindent();
        writer.WriteLine("}");
        writer.Flush();
    }
}