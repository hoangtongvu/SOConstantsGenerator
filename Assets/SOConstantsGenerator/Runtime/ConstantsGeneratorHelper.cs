using SOConstantsGenerator.Common;
using SOConstantsGenerator.FieldProcessors;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SOConstantsGenerator;

public static class ConstantsGeneratorHelper
{
    public static void GenerateDynamicFields(Object so)
    {
        var soType = so.GetType();
        var generateAttr = soType.GetCustomAttribute<GenerateConstantsForAttribute>();

        var className = generateAttr.ConstHolderClassName;
        var classNamespace = generateAttr.ConstHolderClassNamespace;
        var outputPath = GetOutputFilePath(className, so, soType);

        GenerateDynamicFieldsFile(outputPath, so, soType, className, classNamespace);

        AssetDatabase.Refresh();
        Debug.Log("Generated dynamic fields: " + outputPath);
    }

    public static void LoadDynamicFields(Object so)
    {
        var soType = so.GetType();
        var generateAttr = soType.GetCustomAttribute<GenerateConstantsForAttribute>();

        var className = generateAttr.ConstHolderClassName;
        var classNamespace = generateAttr.ConstHolderClassNamespace;
        var assemblyName = GetAssemblyNameFromFolder(GetOutputFolderPath(so, soType));
        string fullyQualifiedName = $"{classNamespace}.{className}, {assemblyName}";
        const string methodName = "LoadStaticFields";

        var type = System.Type.GetType(fullyQualifiedName);

        if (type != null)
        {
            var methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (methodInfo == null)
            {
                Debug.LogError($"Method '{methodName}' not found.");
                return;
            }

            methodInfo.Invoke(null, System.Array.Empty<object>());
            Debug.Log($"Loaded dynamic fields in [{fullyQualifiedName}] successfully.");
        }
        else
        {
            Debug.LogError($"Class '{fullyQualifiedName}' not found.");
        }
    }

    public static void GenerateConstants(Object so)
    {
        var soType = so.GetType();
        var generateAttr = soType.GetCustomAttribute<GenerateConstantsForAttribute>();

        var className = generateAttr.ConstHolderClassName;
        var classNamespace = generateAttr.ConstHolderClassNamespace;
        var outputPath = GetOutputFilePath(className, so, soType);

        GenerateConstantsFile(outputPath, so, soType, className, classNamespace);

        AssetDatabase.Refresh();
        Debug.Log("Generated constants: " + outputPath);
    }

    private static string GetOutputFilePath(string className, Object so, System.Type soType)
    {
        string folder = GetOutputFolderPath(so, soType);
        return Path.Combine(folder, className + ".cs");
    }

    private static string GetOutputFolderPath(Object so, System.Type soType)
    {
        // Look for the target assembly field
        var outputFolderField = soType.GetField("OutputFolder", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        string folder = null;

        if (outputFolderField != null)
        {
            var folderAsset = outputFolderField.GetValue(so) as DefaultAsset;

            if (folderAsset != null)
                folder = AssetDatabase.GetAssetPath(folderAsset);
        }

        // fallback to script directory if no asmdef is assigned
        if (string.IsNullOrEmpty(folder))
            folder = Path.GetDirectoryName(GetScriptPath(so));

        return folder;
    }

    private static string GetScriptPath(Object so)
    {
        var script = MonoScript.FromScriptableObject((ScriptableObject)so);
        return AssetDatabase.GetAssetPath(script);
    }

    public static string GetAssemblyNameFromFolder(string folderPath)
    {
        var asmdefFile = Directory.GetFiles(folderPath, "*.asmdef", SearchOption.TopDirectoryOnly);

        if (asmdefFile.Length > 0)
        {
            var asmdefContent = File.ReadAllText(asmdefFile[0]);
            var asmdefJson = JsonUtility.FromJson<AsmdefData>(asmdefContent);

            if (!string.IsNullOrEmpty(asmdefJson.name))
                return asmdefJson.name;
        }

        return "Assembly-CSharp"; // Default Unity runtime assembly
    }

    public static void GenerateDynamicFieldsFile(string outputPath, Object so, System.Type soType, string className, string classNamespace)
    {
        var fields = soType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);
        var fieldProcessor = new DynamicFieldProcessor();
        using var writer = new CodeWriter(new(outputPath, false));

        writer.WriteLine("// This file is auto-generated, do not change.");
        writer.WriteLine("using System.Collections;");
        writer.WriteLine("using System.Collections.Generic;");
        writer.WriteLine("using System.Linq;");
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

            var canHandleInput = new FieldHandlers.Common.CanHandleInput
            {
                FieldInfo = fieldInfo,
            };

            var handleInput = new FieldHandlers.Common.HandleInput
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

            var canHandleInput = new FieldHandlers.Common.CanHandleInput
            {
                FieldInfo = fieldInfo,
            };

            var handleInput = new FieldHandlers.Common.HandleInput
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
        var fieldProcessor = new FieldProcessors.ConstantFieldProcessor();
        using var writer = new Common.CodeWriter(new(outputPath, false));

        writer.WriteLine("// This file is auto-generated, do not change.");
        writer.WriteLine("using System;");
        writer.WriteLine("using System.Collections;");
        writer.WriteLine("using System.Collections.Generic;");
        writer.WriteLine("using System.Runtime.CompilerServices;");
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

            var canHandleInput = new FieldHandlers.Common.CanHandleInput
            {
                FieldInfo = fieldInfo,
            };

            var handleInput = new FieldHandlers.Common.HandleInput
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