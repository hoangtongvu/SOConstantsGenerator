using Unity.Collections;

namespace Cores;

[System.Serializable]
public struct TestProfile
{
    public int Data0;
    public float Data1;

    public readonly FixedString32Bytes ToFixedString()
    {
        return $"{Data0}, {Data1}";
    }
}