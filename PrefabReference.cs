using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BattleImprove;

public class PrefabReference {
    private static AssetBundle ab;
    
    internal static Dictionary<string, GameObject> loopDrops = new Dictionary<string, GameObject>();

    public static void Load() {
        ab = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("BattleImprove.AssetBundle.battle_improve"));
        
        
        for (int i = 1; i <= 5; i++) {
            string name = $"LoopDropTier{i}";
            var prefab = ab.LoadAsset<GameObject>(name);
            if (prefab != null) {
                loopDrops[$"T{i}"] = prefab;
            } else {
                Debug.LogWarning($"Prefab {name} not found in asset bundle.");
            }
        }
    }

    public static GameObject GetLoopDropVFX(string tier, Transform parent) {
        Plugin.Logger.LogInfo("GetLoopDropVFX called with tier: " + tier);
        return Object.Instantiate(loopDrops[tier], parent);
    }
}