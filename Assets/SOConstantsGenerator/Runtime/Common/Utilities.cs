using System;
using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;

namespace SOConstantsGenerator.Common;

public static class Utilities
{
    public static bool CanBeUnmanagedConst(System.Type t)
    {
        return t == typeof(int)
            || t == typeof(float)
            || t == typeof(double)
            || t == typeof(bool)
            || t == typeof(char)
            || t == typeof(byte)
            || t == typeof(sbyte)
            || t == typeof(short)
            || t == typeof(ushort)
            || t == typeof(uint)
            || t == typeof(long)
            || t == typeof(ulong);
    }

    private static string FormatStructInitializer(object value, System.Type type)
    {
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        // Example result:
        // new MyStruct { A = 1, B = 2f, C = "Hello" }
        var assignments = string.Join(", ",
            fields.Select(f => $"{f.Name} = {FormatValue(f.GetValue(value))}"));

        return $"new {type.Name} {{ {assignments} }}";
    }

    public static string FormatValue(object value)
    {
        return value switch
        {
            string s => $"\"{s}\"",
            float f => f.ToString("0.######") + "f",
            double d => d.ToString("0.######"),
            _ => value.ToString()
        };
    }

    public static string BoxedStructToBytesString(System.Type structType, object boxedStruct)
    {
        byte[] data = BoxedStructToBytes(structType, boxedStruct);
        return string.Join(", ", data.Select(b => b.ToString()));
    }

    public static byte[] BoxedStructToBytes(System.Type structType, object boxedStruct)
    {
        var method = typeof(Utilities).GetMethod(nameof(StructToBytes), BindingFlags.Static | BindingFlags.Public);
        var generic = method.MakeGenericMethod(structType);
        return (byte[])generic.Invoke(null, new object[] { boxedStruct });
    }

    public static byte[] StructToBytes<T>(T value) where T : struct
    {
        int size = UnsafeUtility.SizeOf<T>();
        byte[] bytes = new byte[size];

        unsafe
        {
            fixed (byte* destPtr = bytes)
            {
                UnsafeUtility.MemCpy(destPtr, UnsafeUtility.AddressOf(ref value), size);
            }
        }

        return bytes;
    }

    public static string GetCSharpFullName(System.Type type)
    {
        if (!type.IsNested)
            return type.FullName ?? type.Name;

        return $"{GetCSharpFullName(type.DeclaringType!)}.{type.Name}";
    }

    public static void GetSourceAndDestTypes(System.Type converterType, out System.Type sourceType, out System.Type destType)
    {
        sourceType = null;
        destType = null;

        if (converterType == null) return;

        var converterInterface = converterType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(ITypeConverter<,>));

        if (converterInterface != null)
        {
            var args = converterInterface.GetGenericArguments();

            sourceType = args[0];
            destType = args[1];
        }
    }

    public static string ToCodeLiteral(object o, Type type = null, Type converterType = null)
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
            return $"Unsafe.As<byte, {GetCSharpFullName(type)}>(ref new byte[] {{ {bytesString} }}[0])";
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
                    var childConverterTypes = field.GetCustomAttribute<ConstantFieldAttribute>().ConverterTypes;
                    var childConverterType = childConverterTypes?[0];
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