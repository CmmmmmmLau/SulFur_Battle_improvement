using HarmonyLib;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;
using Unity.Mathematics;
using UnityEngine;

namespace BattleImprove.Patcher.QOL;

public class DeadUnitCollisionPatch {
    [HarmonyPrefix, HarmonyPatch(typeof(Projectile), "ReportBounceOnHitbox")]
    private static bool CheckPass(Projectile __instance, Hitbox hitbox) {
        Plugin.instance.LoggingInfo($"Projectile {__instance.name} hit {hitbox.name}");
        return false;
    }
}