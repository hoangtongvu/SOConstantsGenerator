using System;
using System.Runtime.InteropServices;

namespace Cores;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct TestProfileId : IEquatable<TestProfileId>
{
    [FieldOffset(0)] private readonly short _raw; // 2 bytes
    [FieldOffset(0)] public byte UnitType;
    [FieldOffset(1)] public byte VariantIndex;

    public bool Equals(TestProfileId other)
        => this._raw == other._raw;

    public override bool Equals(object obj)
        => obj is TestProfileId other && this._raw == other._raw;

    public override int GetHashCode()
        => this._raw.GetHashCode();

    public static bool operator ==(TestProfileId left, TestProfileId right)
        => left._raw == right._raw;

    public static bool operator !=(TestProfileId left, TestProfileId right)
        => left._raw != right._raw;

    public override string ToString()
    {
        return $"{nameof(UnitType)}: {UnitType}, {nameof(VariantIndex)}: {VariantIndex}";
    }
}