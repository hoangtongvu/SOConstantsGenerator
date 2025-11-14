using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;

namespace SOConstantsGenerator.Editor;

public static class Utilities
{
    public static bool CanBeConst(System.Type t)
    {
        return t == typeof(int)
            || t == typeof(float)
            || t == typeof(double)
            || t == typeof(bool)
            || t == typeof(string)
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
}