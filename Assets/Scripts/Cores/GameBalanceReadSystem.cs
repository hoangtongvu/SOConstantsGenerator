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
            this.Log();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Runtime test
            if (!Input.GetKeyDown(KeyCode.D)) return;
            this.Log();
        }

        [BurstCompile]
        private void Log()
        {
            Debug.Log("___________________________________________________________");
            Debug.Log($"{GameBalance.TestType0.value}");
            Debug.Log($"{GameBalance.TestType1.ToFixedString()}");

            Debug.Log($"<b>Array:</b>");
            int count = 0;
            foreach (var testType in GameBalance.TestTypeArray)
            {
                Debug.Log($"[{count}] {testType.ToFixedString()}");
                count++;
            }

            Debug.Log($"<b>List:</b>");
            count = 0;
            foreach (var testType in GameBalance.TestTypeList)
            {
                Debug.Log($"[{count}] {testType.ToFixedString()}");
                count++;
            }

            Debug.Log($"<b>HashMap:</b>");
            count = 0;
            foreach (var kvPair in GameBalance.Profiles.Enumerable)
            {
                Debug.Log($"[{count}] K[{kvPair.Key.ToFixedString()}] - V[{kvPair.Value.ToFixedString()}]");
                count++;
            }

            var key = new TestProfileId { UnitType = 0, VariantIndex = 1 };
            GameBalance.Profiles.TryGetValue(key, out var profile);
            Debug.Log($"K[{key.ToFixedString()}] - V[{profile.ToFixedString()}]");
        }
    }
}
