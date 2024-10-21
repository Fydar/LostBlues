#if UNITY_EDITOR
using UnityEditor;

namespace LostBluesUnity
{
    public static class Reserializer
    {
        [MenuItem("Toolkit/Reserialize")]
        public static void Reserialize()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
#endif
