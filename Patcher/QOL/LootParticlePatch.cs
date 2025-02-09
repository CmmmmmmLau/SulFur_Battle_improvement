using BattleImprove.Components;
using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.World;
using UnityEngine;

namespace BattleImprove.Patcher;


public class LootParticlePatch {
    [HarmonyPostfix, HarmonyPatch(typeof(Pickup), "Awake")]
    private static void Postfix(ref Pickup __instance) {
        Plugin.instance.LoggingInfo("LootParticle Postfix", true);
        Prefab.LoadLootParticlePrefab(__instance.gameObject);
    }
}