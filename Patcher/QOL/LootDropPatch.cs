using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.World;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BattleImprove.Patcher.QOL;
public class LootDropPatch {
    [HarmonyWrapSafe]
    [HarmonyPostfix, HarmonyPatch(typeof(Pickup), "SetupAndSpawn")]
    private static void SetupAndSpawnPostfix(Pickup __instance) {
        Plugin.LoggingInfo("LootParticle Postfix", true);
        Plugin.LoggingInfo("LootParticle Hide Postfix", true);
        var preVFX = __instance.GetComponentInChildren<LootDropVFX>();
        if (preVFX != null) {
            Object.Destroy(preVFX.gameObject);
            Plugin.LoggingInfo("LootParticle Destroyed", true);
        } else {
            Plugin.LoggingInfo("LootParticle not found", true);
        }
        
        
        var shadow = __instance.transform.Find("Shadow").gameObject;
        // Object.Destroy(shadow.GetComponent<DecalProjector>());
        
        var quality = __instance.ItemSO.itemQuality;
        var lootDropVFX = quality switch {
            ItemQuality.Common => PrefabManager.LoadPrefab("LoopDropTier1", __instance.gameObject),
            ItemQuality.Uncommon => PrefabManager.LoadPrefab("LoopDropTier2", __instance.gameObject),
            ItemQuality.Rare => PrefabManager.LoadPrefab("LoopDropTier3", __instance.gameObject),
            ItemQuality.Epic => PrefabManager.LoadPrefab("LoopDropTier4", __instance.gameObject),
            ItemQuality.Legendary => PrefabManager.LoadPrefab("LoopDropTier5", __instance.gameObject),
            _ => PrefabManager.LoadPrefab("LoopDropTier1", __instance.gameObject)
        };
        var components = lootDropVFX.GetComponent<LootDropVFX>();
        components.SetParent(shadow);
    }
}