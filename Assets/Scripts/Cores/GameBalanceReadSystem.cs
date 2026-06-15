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

            Debug.Log($"<b>Single UserData:</b>");
            Debug.Log($"{GameBalance.UserData.ToFixedString()}");

            Debug.Log($"<b>Array:</b>");
            int count = 0;
            foreach (var testType in GameBalance.UserDataArray)
            {
                Debug.Log($"[{count}] {testType.ToFixedString()}");
                count++;
            }

            Debug.Log($"<b>List:</b>");
            count = 0;
            foreach (var testType in GameBalance.UserDataList)
            {
                Debug.Log($"[{count}] {testType}");
                count++;
            }

            Debug.Log($"<b>HashMap:</b>");
            count = 0;
            foreach (var kvPair in GameBalance.UserDataMap1.Enumerable)
            {
                Debug.Log($"[{count}] K[{kvPair.Key.ToFixedString()}] - V[{kvPair.Value.ToFixedString()}]");
                count++;
            }
        }
    }
}
