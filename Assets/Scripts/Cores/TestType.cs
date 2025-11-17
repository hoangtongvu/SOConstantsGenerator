using Unity.Collections;
using Unity.Mathematics;

namespace SOConstGenerator;

[System.Serializable]
public struct TestType
{
    public int Sf;
    public float G;
    public half AnotherHalf;

    public readonly FixedString32Bytes ToFixedString()
    {
        return $"{Sf}, {G}, {AnotherHalf.value}";
    }
}