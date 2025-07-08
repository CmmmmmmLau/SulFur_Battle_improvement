using System.Collections.Generic;
using System.Reflection;
using BattleImprove.MonoBehavior;
using UnityEngine;

namespace BattleImprove;

public class PrefabReference {
    private static AssetBundle ab;
    
    internal static Dictionary<string, GameObject> loopDrops = new Dictionary<string, GameObject>();
    internal static Dictionary<string, GameObject> crossHair = new Dictionary<string, GameObject>();

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
        
        var prefab2 = ab.LoadAsset<GameObject>("BF1CrossHair");
        if (prefab2 != null) {
            crossHair["BF1"] = prefab2;
        } else {
            Debug.LogWarning("Prefab BF1CrossHair not found in asset bundle.");
        }
    }

    public static GameObject GetLoopDropVFX(string tier, Transform parent) {
        Plugin.Logger.LogInfo("GetLoopDropVFX called with tier: " + tier);
        return Object.Instantiate(loopDrops[tier], parent);
    }
    
    public static void RegisterCrossHair(string name, GameObject prefab) {
        if (crossHair.ContainsKey(name)) {
            Plugin.Logger.LogWarning($"Crosshair {name} already registered, overwriting.");
        }
        crossHair[name] = prefab;
    }
}