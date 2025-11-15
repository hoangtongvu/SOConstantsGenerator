using Cores;
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

            Debug.Log($"Array:");
            int count = 0;
            foreach (var testType in GameBalance.TestTypeArray)
            {
                Debug.Log($"[{count}] {testType.G}");
                Debug.Log($"[{count}] {testType.AnotherHalf.value}");
                count++;
            }

            Debug.Log($"List:");
            count = 0;
            foreach (var testType in GameBalance.TestTypeList)
            {
                Debug.Log($"[{count}] {testType.G}");
                Debug.Log($"[{count}] {testType.AnotherHalf.value}");
                count++;
            }

            Debug.Log($"HashMap:");
            count = 0;
            foreach (var value in GameBalance.Profiles.Values)
            {
                Debug.Log($"[{count}] {value.Data0} - {value.Data1}");
                count++;
            }

            var key = new TestProfileId { UnitType = 0, VariantIndex = 1 };
            GameBalance.Profiles.TryGetValue(key, out var profile);
            Debug.Log($"Key: {key} - Value: {profile.Data0} - {profile.Data1}");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
        }
    }
}
