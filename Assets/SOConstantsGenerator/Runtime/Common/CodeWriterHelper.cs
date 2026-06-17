using System;
using System.Collections.Generic;
using System.Reflection;
using static SOConstantsGenerator.Common.Utilities;

namespace SOConstantsGenerator.Common;

public static class CodeWriterHelper
{
    private static readonly Dictionary<Type, object> converterInstanceCache = new();
    private static readonly Dictionary<Type, MethodInfo> convertMethodCache = new();

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

        if (type.IsClass)
        {
            if (converterType == null)
                throw new Exception("Class type requires 1 converter");

            if (!converterInstanceCache.TryGetValue(converterType, out var converter))
            {
                converter = Activator.CreateInstance(converterType);
                converterInstanceCache[converterType] = converter;
            }

            if (!convertMethodCache.TryGetValue(converterType, out var convertMethod))
            {
                convertMethod = converterType.GetMethod("Convert");
                convertMethodCache[converterType] = convertMethod;
            }

            object destValue = convertMethod.Invoke(converter, new[] { o });
            WriteConstValueLiteral(writer, destValue, destValue.GetType(), punctuation: punctuation);
            return;
        }

        throw new NotSupportedException(type.FullName);
    }
}