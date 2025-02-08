using System;
using BattleImprove.Patcher.BattleFeedback;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patcher.TakeHitPatcher;

[HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
public class CrossHairPatch : AttackFeedbackPatch{
    private static void Prefix(Hitbox __instance, out bool __state) {
        __state = __instance.Owner.UnitState is UnitState.Alive or UnitState.Incapacitated;;
    }
    
    private static void Postfix(Hitbox __instance, ref DamageSourceData source, bool __state) {
        if (StaticInstance.CrossHair == null) return;
        if(!TargetCheck(source, __instance)) return;
        if (!__state) return;
        
        PlayHitAnimation(__instance.Owner);
    }
    
    private static bool PlayHitAnimation(Unit unit) {
        bool isAliveOrIncapacitated = unit.UnitState is UnitState.Alive or UnitState.Incapacitated;
        bool isXCrossHairEnabled = Config.EnableXCrossHair.Value;
        
        if (isXCrossHairEnabled && isAliveOrIncapacitated) {
            StaticInstance.CrossHair.StartTrigger("Hit");
        } else {
            StaticInstance.CrossHair.StartTrigger("Kill");
        }

        return isAliveOrIncapacitated;
    }
}