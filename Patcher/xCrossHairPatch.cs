using System;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace ExpShare;

public class xCrossHairPatch {
    [HarmonyPostfix, HarmonyPatch(typeof(Hitbox), "TakeHit")]
    private static void PlayHitSound(Hitbox __instance) {
        if (__instance.Owner is Breakable || __instance.Owner.isPlayer || __instance.Owner.UnitState == UnitState.Dead) return;
        
        Plugin.StaticInstance.CrossHair.HitTrigger();
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(UnitManager), "OnUnitDeath")]
    private static void PlayKillSound(Unit unit) {
        if (unit is Breakable || unit.isPlayer) return;
        
        Plugin.StaticInstance.CrossHair.KillTrigger();
    }
}