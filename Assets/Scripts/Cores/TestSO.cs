using AYellowpaper.SerializedCollections;
using Cores;
using SOConstantsGenerator;
using System.Collections.Generic;
using UnityEngine;

namespace SOConstGenerator
{
    [GenerateConstantsFor("GameBalance", "Core")]
    [CreateAssetMenu(fileName = "TestSO", menuName = "SO/TestSO")]
    public partial class TestSO : ScriptableObject
    {
        [ConstantField(typeof(FixedString64BytesConverter))] public string StringValue;
        [ConstantField] public float UnmanangedPrimitiveValue = 4.5f;

        [ConstantField(typeof(UserData.UnmanagedConverter))]
        public UserData.Managed UserData;

        [ConstantField(typeof(UserData.UnmanagedConverter))]
        public UserData.Managed[] UserDataArray;
        [ConstantField(typeof(UserData.UnmanagedConverter))]
        public List<UserData.Managed> UserDataList;

        [ConstantField(typeof(UserKey.UnmanagedConverter), typeof(UserData.UnmanagedConverter))]
        public SerializedDictionary<UserKey.Managed, UserData.Managed> UserDataMap0;
        [ConstantField(null, typeof(UserData.UnmanagedConverter))]
        public SerializedDictionary<UserKey.Unmanaged, UserData.Managed> UserDataMap1;
        [ConstantField(typeof(UserKey.UnmanagedConverter), null)]
        public SerializedDictionary<UserKey.Managed, UserData.Unmanaged> UserDataMap2;
        [ConstantField]
        public SerializedDictionary<UserKey.Unmanaged, UserData.Unmanaged> UserDataMap3;
    }
}