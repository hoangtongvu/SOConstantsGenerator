using SOConstantsGenerator.Editor.Common;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static SOConstantsGenerator.Editor.ConstantsGeneratorHelper;

namespace SOConstantsGenerator.Editor;

[CustomEditor(typeof(ScriptableObject), true)]
public class ConstantsGeneratorEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetType = target.GetType();
        var generateAttr = targetType.GetCustomAttribute<GenerateConstantsForAttribute>();
        if (generateAttr == null)
            return;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(
            new GUIContent("Generate Dynamic Fields", "Generate static and dynamic loading fields. No need to regenerate except when the SO's InstanceID changed.\n<b>This is NOT Burst-compilable, and Editor only</b>."),
            GUILayout.Width(EditorGUIUtility.currentViewWidth / 2 - 15)))
        {
            GenerateDynamicFields(target, targetType, generateAttr);
        }

        GUILayout.Space(5);

        if (GUILayout.Button(
            new GUIContent("Load Dynamic Fields", "Load Dynamic Fields manually."),
            GUILayout.Width(EditorGUIUtility.currentViewWidth / 2 - 15)))
        {
            LoadDynamicFields(target, targetType, generateAttr);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button(new GUIContent("Generate Constants", "Generate mix of constant (primitive) and static read-only (unmanaged struct) fields. Have to regenerate every time the fields changed.\n<b>This is Burst-compilable</b>.")))
        {
            GenerateConstants(target, targetType, generateAttr);
        }
    }

    private static void GenerateDynamicFields(Object so, System.Type soType, GenerateConstantsForAttribute generateConstantsForAttribute)
    {
        var className = generateConstantsForAttribute.ConstHolderClassName;
        var classNamespace = generateConstantsForAttribute.ConstHolderClassNamespace;
        var outputPath = GetOutputFilePath(className, so, soType);

        GenerateDynamicFieldsFile(outputPath, so, soType, className, classNamespace);

        AssetDatabase.Refresh();
        Debug.Log("Generated dynamic fields: " + outputPath);
    }

    private static void LoadDynamicFields(Object so, System.Type soType, GenerateConstantsForAttribute generateConstantsForAttribute)
    {
        var className = generateConstantsForAttribute.ConstHolderClassName;
        var classNamespace = generateConstantsForAttribute.ConstHolderClassNamespace;
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

    private static void GenerateConstants(Object so, System.Type soType, GenerateConstantsForAttribute generateConstantsForAttribute)
    {
        var className = generateConstantsForAttribute.ConstHolderClassName;
        var classNamespace = generateConstantsForAttribute.ConstHolderClassNamespace;
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
}