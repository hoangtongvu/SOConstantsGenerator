using SOConstantsGenerator.Common;
using SOConstantsGenerator.FieldHandlers.Common;
using System;
using System.Linq;
using System.Reflection;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.FieldHandlers.ConstantFieldHandlers;

public class NormalConstantFieldHandler : IConstantFieldHandler
{
    public bool CanHandle(CanHandleInput canHandleInput)
    {
        return true;
    }

    public void HandleInLineGeneration(HandleInput handleInput)
    {
        var writer = handleInput.Writer;
        var fieldInfo = handleInput.FieldInfo;
        var converterType = handleInput.ConverterType;

        if (CanBeUnmanagedConst(fieldInfo.Type))
        {
            // Handle constants
            writer.WriteLine($"public const {GetCSharpFullName(fieldInfo.Type)} {fieldInfo.Name} = {FormatValue(fieldInfo.Value)};");
        }
        else
        {
            // Handle static readonly
            if (converterType == null)
            {
                writer.WriteLine($"public static readonly {GetCSharpFullName((fieldInfo.Type))} {fieldInfo.Name} =");
            }
            else
            {
                GetSourceAndDestTypes(converterType, out _, out var destType);
                writer.WriteLine($"public static readonly {GetCSharpFullName(destType)} {fieldInfo.Name} =");
            }

            writer.Indent();
            writer.WriteLine($"{ToCodeLiteral(fieldInfo.Value, fieldInfo.Type, converterType)};");
            writer.Unindent();
        }
    }

    private static string ToCodeLiteral(object o, Type type = null, Type converterType = null)
    {
        if (type == null)
            type = o.GetType();

        if (o == null)
            return "null";

        // Handle primitives
        if (type == typeof(int))
            return ((int)o).ToString();

        if (type == typeof(float))
            return ((float)o).ToString("R") + "f";

        if (type == typeof(double))
            return ((double)o).ToString("R");

        if (type == typeof(bool))
            return ((bool)o) ? "true" : "false";

        if (type == typeof(string))
            return $"@\"{((string)o).Replace("\"", "\"\"")}\"";

        if (type.IsEnum)
            return $"{type.FullName}.{o}";

        // Handle unmanaged structs
        if (type.IsValueType)
        {
            var bytesString = BoxedStructToBytesString(type, o);
            return $"Unsafe.As<byte, {type}>(ref new byte[] {{ {bytesString} }}[0])";
        }

        // Handle classes w/wo converters
        if (type.IsClass)
        {
            var sb = new CodeStringBuilder();

            // Handle base class's nested class, only need to reconstruct it field by field, ignore converter of any fields
            if (converterType == null)
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);

                sb.AppendLine($"new {GetCSharpFullName(type)}()");
                sb.AppendLine("{");
                sb.Indent();

                foreach (var field in fields)
                {
                    string valueLiteral = ToCodeLiteral(field.GetValue(o), field.FieldType);
                    sb.AppendLine($"{field.Name} = {valueLiteral},");
                }

                sb.Unindent();
                sb.AppendLine("}");

                return sb.ToString();
            }
            // Handle base class, require converter
            else
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);

                sb.AppendLine($"new {GetCSharpFullName(converterType)}().Convert(new()");
                sb.AppendLine("{");
                sb.Indent();

                foreach (var field in fields)
                {
                    var childConverterType = field.GetCustomAttribute<ConstantFieldAttribute>().ConverterType;
                    var valueLiteral = ToCodeLiteral(field.GetValue(o), field.FieldType, childConverterType);

                    sb.AppendLine($"{field.Name} = {valueLiteral},");
                }

                sb.Unindent();
                sb.AppendLine("})");

                return sb.ToString();
            }
        }

        throw new NotSupportedException(type.FullName);
    }
}