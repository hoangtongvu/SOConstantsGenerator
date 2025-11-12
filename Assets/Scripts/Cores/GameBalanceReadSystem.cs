using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Core
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [BurstCompile]
    public partial struct GameBalanceReadSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            Debug.Log($"{GameBalance.TestType0.value}");
            Debug.Log($"{GameBalance.TestType1.Sf + 5}");
            Debug.Log($"{GameBalance.TestType1.G}");
            Debug.Log($"{GameBalance.TestType1.AnotherHalf.value}");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}
