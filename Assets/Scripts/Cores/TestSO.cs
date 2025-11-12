using SOConstantsGenerator;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace SOConstGenerator
{
    [GenerateConstantsFor("GameBalance", "Core")]
    [CreateAssetMenu(fileName = "TestSO", menuName = "SO/TestSO")]
    public partial class TestSO : ScriptableObject
    {
        [ConstantField] public float PlayerBaseSpeed = 4.5f;
        [ConstantField] public int MaxLives = 3;
        [ConstantField] public half TestType0;
        [ConstantField] public TestType TestType1;
    }
}