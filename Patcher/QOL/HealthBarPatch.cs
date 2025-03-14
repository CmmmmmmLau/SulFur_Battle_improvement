using BattleImprove.Components;
using BattleImprove.Components.QOL;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.DevTools;
using PerfectRandom.Sulfur.Core.Units;

namespace BattleImprove.Patcher.QOL;

public class HealthBarPatch {
    [HarmonyWrapSafe]
    [HarmonyPrefix, HarmonyPatch(typeof(UnitDebugFrame), "Update")]
    private static bool UpdateHPBar(UnitDebugFrame __instance) {
        if (StaticInstance<DevToolsManager>.Instance.shouldShow) return true;

        __instance.transform.LookAt(StaticInstance<GameManager>.Instance.currentCamera.transform);
        var bar = __instance.owner.GetComponent<HealthBar>();
        if (bar.UpdateValue()) Traverse.Create(__instance).Method("UpdateValues").GetValue();

        if (__instance.owner.IsAlive) return false;

        bar.enabled = false;
        __instance.gameObject.SetActive(false);

        return false;
    }

    [HarmonyPostfix, HarmonyPatch(typeof(Npc), "Start")]
    private static void AddDebugFrame(Player __instance) {
        __instance.gameObject.AddComponent<HealthBar>();
    }
}