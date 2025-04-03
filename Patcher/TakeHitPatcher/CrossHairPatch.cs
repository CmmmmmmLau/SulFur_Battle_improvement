using System;
using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patcher.TakeHitPatcher;
[HarmonyWrapSafe]
[HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
public class CrossHairPatch : AttackFeedbackPatch{
    private static void Prefix(Hitbox __instance, out bool __state) {
        __state = __instance.Owner.UnitState is UnitState.Alive or UnitState.Incapacitated;;
    }
    
    private static void Postfix(Hitbox __instance, ref DamageSourceData source, bool __state) {
        if (PluginInstance<xCrossHair>.Instance == null) return;
        if(!TargetCheck(source, __instance)) return;
        if (!__state) return;
        
        PlayHitAnimation(__instance.Owner);
    }
    
    private static bool PlayHitAnimation(Unit unit) {
        bool isAliveOrIncapacitated = unit.UnitState is UnitState.Alive or UnitState.Incapacitated;
        bool isXCrossHairEnabled = Config.EnableXCrossHair.Value;
        
        if (isXCrossHairEnabled && isAliveOrIncapacitated) {
            PluginInstance<xCrossHair>.Instance.StartTrigger("Hit");
        } else {
            PluginInstance<xCrossHair>.Instance.StartTrigger("Kill");
        }

        return isAliveOrIncapacitated;
    }
}