// This file is auto-generated, do not change.
using System.Runtime.CompilerServices;

namespace Core;

public static class GameBalance
{
    public const System.Single PlayerBaseSpeed = 4.5f;
    public const System.Int32 MaxLives = 3;
    public static readonly Unity.Mathematics.half TestType0 =
        Unsafe.As<byte, Unity.Mathematics.half>(ref new byte[] { 244, 1 }[0]);
    public static readonly SOConstGenerator.TestType TestType1 =
        Unsafe.As<byte, SOConstGenerator.TestType>(ref new byte[] { 15, 0, 0, 0, 51, 51, 51, 63, 100, 0, 0, 0 }[0]);
    public static readonly SOConstGenerator.TestType[] TestTypeArray = new SOConstGenerator.TestType[]
    {
        Unsafe.As<byte, SOConstGenerator.TestType>(ref new byte[] { 5, 0, 0, 0, 51, 51, 131, 64, 71, 0, 0, 0 }[0]),
        Unsafe.As<byte, SOConstGenerator.TestType>(ref new byte[] { 12, 0, 0, 0, 154, 153, 153, 63, 143, 0, 0, 0 }[0]),
    };
    public static readonly SOConstGenerator.TestType[] TestTypeList = new SOConstGenerator.TestType[]
    {
        Unsafe.As<byte, SOConstGenerator.TestType>(ref new byte[] { 7, 0, 0, 0, 0, 0, 156, 66, 123, 0, 0, 0 }[0]),
    };
    public struct Profiles
    {
        public const int Count = 3;
        public static readonly Cores.TestProfileId[] Keys = new Cores.TestProfileId[]
        {
            Unsafe.As<byte, Cores.TestProfileId>(ref new byte[] { 0, 0 }[0]),
            Unsafe.As<byte, Cores.TestProfileId>(ref new byte[] { 1, 0 }[0]),
            Unsafe.As<byte, Cores.TestProfileId>(ref new byte[] { 0, 1 }[0]),
        };
        public static readonly Cores.TestProfile[] Values = new Cores.TestProfile[]
        {
            Unsafe.As<byte, Cores.TestProfile>(ref new byte[] { 1, 0, 0, 0, 154, 153, 153, 63 }[0]),
            Unsafe.As<byte, Cores.TestProfile>(ref new byte[] { 2, 0, 0, 0, 51, 51, 19, 64 }[0]),
            Unsafe.As<byte, Cores.TestProfile>(ref new byte[] { 3, 0, 0, 0, 154, 153, 89, 64 }[0]),
        };
        public static bool TryGetValue(Cores.TestProfileId key, out Cores.TestProfile value)
        {
            int keyHash = key.GetHashCode();
            switch (keyHash)
            {
                case 0:
                    value = Values[0];
                    return true;
                case 65537:
                    value = Values[1];
                    return true;
                case 16777472:
                    value = Values[2];
                    return true;
            }
            value = default;
            return false;
        }
        public static Cores.TestProfile GetValue(Cores.TestProfileId key)
        {
            if (TryGetValue(key, out var value)) return value;
            throw new System.Collections.Generic.KeyNotFoundException($"Key {key} Not Found.");
        }
        public static bool ContainsKey(Cores.TestProfileId key)
        {
            int keyHash = key.GetHashCode();
            return keyHash switch
            {
                0 => true,
                65537 => true,
                16777472 => true,
                _ => false,
            };
        }
        public static bool ContainsValue(Cores.TestProfile value)
        {
            foreach (var entry in Values)
            {
                if (value.Equals(entry)) return true;
            }
            return false;
        }
    }
}
