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

        if (GUILayout.Button(new GUIContent("Generate Dynamic Fields", "Generate static and dynamic loading fields. No need to regenerate except when the SO's InstanceID changed.\n<b>This is NOT Burst-compilable, and Editor only</b>.")))
        {
            GenerateDynamicFields(target, targetType, generateAttr);
        }

        if (GUILayout.Button(new GUIContent("Generate Constants", "Generate mix of constant (primitive) and static read-only (unmanaged struct) fields. Have to regenerate every time the fields changed.\n<b>This is Burst-compilable</b>.")))
        {
            GenerateConstants(target, targetType, generateAttr);
        }
    }

    private void GenerateDynamicFields(Object so, System.Type soType, GenerateConstantsForAttribute generateConstantsForAttribute)
    {
        var className = generateConstantsForAttribute.ConstHolderClassName;
        var classNamespace = generateConstantsForAttribute.ConstHolderClassNamespace;
        var outputPath = GetOutputPath(className, so, soType);

        GenerateDynamicFieldsFile(outputPath, so, soType, className, classNamespace);

        AssetDatabase.Refresh();
        Debug.Log("Generated dynamic fields: " + outputPath);
    }

    private void GenerateConstants(Object so, System.Type soType, GenerateConstantsForAttribute generateConstantsForAttribute)
    {
        var className = generateConstantsForAttribute.ConstHolderClassName;
        var classNamespace = generateConstantsForAttribute.ConstHolderClassNamespace;
        var outputPath = GetOutputPath(className, so, soType);

        GenerateConstantsFile(outputPath, so, soType, className, classNamespace);

        AssetDatabase.Refresh();
        Debug.Log("Generated constants: " + outputPath);
    }

    private static string GetOutputPath(string className, Object so, System.Type soType)
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

        return Path.Combine(folder, className + ".cs");
    }

    private static string GetScriptPath(Object so)
    {
        var script = MonoScript.FromScriptableObject((ScriptableObject)so);
        return AssetDatabase.GetAssetPath(script);
    }
}