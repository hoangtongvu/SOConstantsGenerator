using System;
using System.Linq;
using System.Reflection;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.Common;

public static class CodeWriterHelper
{
    public static void WriteConstValueLiteral(
        CodeWriter writer,
        object o,
        Type type = null,
        Type converterType = null,
        string punctuation = "")
    {
        if (type == null)
            type = o.GetType();

        if (o == null)
        {
            writer.WriteLineNoIndent($"null{punctuation}");
            return;
        }

        // Handle primitives
        if (type == typeof(int))
        {
            writer.WriteLineNoIndent($"{(int)o}{punctuation}");
            return;
        }

        if (type == typeof(float))
        {
            writer.WriteLineNoIndent($"{$"{(float)o:R}f"}{punctuation}");
            return;
        }

        if (type == typeof(double))
        {
            writer.WriteLineNoIndent($"{(double)o:R}{punctuation}");
            return;
        }

        if (type == typeof(bool))
        {
            writer.WriteLineNoIndent($"{(bool)o}{punctuation}");
            return;
        }

        if (type == typeof(string))
        {
            var value = $"@\"{((string)o).Replace("\"", "\"\"")}\"";
            writer.WriteLineNoIndent($"{value}{punctuation}");
            return;
        }

        if (type.IsEnum)
        {
            writer.WriteLineNoIndent($"{type.FullName}.{o}{punctuation}");
            return;
        }

        // Handle unmanaged structs
        if (type.IsValueType)
        {
            var bytesString = BoxedStructToBytesString(type, o);
            writer.WriteLineNoIndent($"Unsafe.As<byte, {GetCSharpFullName(type)}>(ref new byte[] {{ {bytesString} }}[0]){punctuation}");
            return;
        }

        // Handle classes w/wo converters
        if (type.IsClass)
        {
            // Handle base class's nested class, only need to reconstruct it field by field, ignore converter of any fields
            if (converterType == null)
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);

                writer.WriteLineNoIndent($"new {GetCSharpFullName(type)}()");
                writer.WriteLine("{");
                writer.Indent();

                foreach (var field in fields)
                {
                    writer.Write($"{field.Name} = ");
                    WriteConstValueLiteral(writer, field.GetValue(o), field.FieldType, punctuation: ",");
                }

                writer.Unindent();
                writer.WriteLine($"}}{punctuation}");

                return;
            }
            // Handle base class, require converter
            else
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute<ConstantFieldAttribute>() != null);

                writer.WriteLineNoIndent($"new {GetCSharpFullName(converterType)}().Convert(new()");
                writer.WriteLine("{");
                writer.Indent();

                foreach (var field in fields)
                {
                    var childConverterTypes = field.GetCustomAttribute<ConstantFieldAttribute>().ConverterTypes;
                    var childConverterType = childConverterTypes?[0];

                    writer.Write($"{field.Name} = ");
                    WriteConstValueLiteral(writer, field.GetValue(o), field.FieldType, childConverterType, punctuation: ",");
                }

                writer.Unindent();
                writer.WriteLine($"}}){punctuation}");

                return;
            }
        }

        throw new NotSupportedException(type.FullName);
    }
}