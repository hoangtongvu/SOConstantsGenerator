
namespace Cores;

[System.Serializable]
public struct TestProfile
{
    public int Data0;
    public float Data1;

    public override string ToString()
    {
        return $"Data0: {Data0}, Data1: {Data1}";
    }
}