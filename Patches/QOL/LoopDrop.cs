using System; using BattleImprove.MonoBehavior.Component;
using BattleImprove.PluginData;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using Object = UnityEngine.Object;

namespace BattleImprove.Patches.QOL;

public class LoopDrop {
    [HarmonyWrapSafe]
    [HarmonyPostfix, HarmonyPatch(typeof(Pickup), "SetupAndSpawn")]
    private static void AddLoopDropVfx(Pickup __instance) {
        
        var preVFX = __instance.GetComponentInChildren<LootDropVFX>();
        if (preVFX != null) {
            Object.Destroy(preVFX.gameObject);
            Plugin.Logger.LogInfo("LootParticle Destroyed");
        }
        
        var shadow = __instance.transform.Find("Shadow").gameObject;
        // Object.Destroy(shadow.GetComponent<DecalProjector>());
        
        if (!MiscData.Instance.loopDropEnable) return;
        
        var quality = __instance.ItemSO.itemQuality;
        var lootDropVFX = quality switch {
            ItemQuality.Common => PrefabReference.GetLoopDropVFX("T1", __instance.transform),
            ItemQuality.Uncommon => PrefabReference.GetLoopDropVFX("T2", __instance.transform),
            ItemQuality.Rare => PrefabReference.GetLoopDropVFX("T3", __instance.transform),
            ItemQuality.Epic => PrefabReference.GetLoopDropVFX("T4", __instance.transform),
            ItemQuality.Legendary => PrefabReference.GetLoopDropVFX("T5", __instance.transform),
            _ => PrefabReference.GetLoopDropVFX("T1", __instance.transform)
        };
        var components = lootDropVFX.GetComponent<LootDropVFX>();
        components.SetParent(shadow);
    }
}