using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.World;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BattleImprove.Patcher.QOL;
public class LootParticlePatch {
    [HarmonyPostfix, HarmonyPatch(typeof(Pickup), "Awake")]
    private static void Postfix(ref Pickup __instance) {
        Plugin.instance.LoggingInfo("LootParticle Postfix", true);
        var shadow = __instance.transform.Find("Shadow").gameObject;
        Object.Destroy(shadow.GetComponent<DecalProjector>());
        var lootdrop = Prefab.LoadPrefab("LoopDropTier1", __instance.gameObject);
        var components = lootdrop.GetComponent<LootDropVFX>();
        components.SetParent(shadow);
    }
}