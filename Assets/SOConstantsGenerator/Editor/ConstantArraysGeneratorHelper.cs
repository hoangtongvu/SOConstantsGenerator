using System.Collections;
using System.Collections.Generic;
using System.Text;
using static SOConstantsGenerator.Editor.Utilities;

namespace SOConstantsGenerator.Editor;

public static class ConstantArraysGeneratorHelper
{
    public static string GetDynamicAssignments(
        int indentLevel
        , List<MyFieldInfo> arrayFieldInfoList)
    {
        var sb = new StringBuilder();
        void AddLine(string line) => sb.AppendLine(new string(' ', indentLevel * 4) + line);
        void AddEmptyLine() => sb.AppendLine();

        foreach (var field in arrayFieldInfoList)
        {
            if (field.Type.IsArray)
            {
                AddLine($"{field.Name} = so.{field.Name};");
            }
            else
            {
                AddLine($"{field.Name} = so.{field.Name}.ToArray();");
            }
        }

        return sb.ToString();
    }

    public static string GetHardCodedArraysInitialization(
        int indentLevel
        , List<MyFieldInfo> arrayFieldInfoList)
    {
        var sb = new StringBuilder();
        void AddLine(string line) => sb.AppendLine(new string(' ', indentLevel * 4) + line);
        void AddEmptyLine() => sb.AppendLine();

        foreach (var field in arrayFieldInfoList)
        {
            var fieldType = field.Type;

            var elementType = fieldType.IsArray
                ? fieldType.GetElementType()
                : fieldType.GetGenericArguments()[0];

            var enumerable = (IEnumerable)field.Value;

            AddLine($"public static readonly {elementType}[] {field.Name} = new {elementType}[]");
            AddLine("{");
            indentLevel++;

            foreach (var element in enumerable)
            {
                var bytesString = BoxedStructToBytesString(elementType, element);
                AddLine($"Unsafe.As<byte, {elementType}>(ref new byte[] {{ {bytesString} }}[0]),");
            }

            indentLevel--;
            AddLine("};");
        }

        return sb.ToString();
    }
}