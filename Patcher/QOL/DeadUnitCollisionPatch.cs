﻿using HarmonyLib;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;

namespace ExpShare.Patcher.BattleFeedback;

public class DeadUnitCollisionPatch {
    [HarmonyPrefix, HarmonyPatch(typeof(Projectile), "ReportBounceOnHitbox")]
    private static bool CheckPass(Projectile __instance, Hitbox hitbox) {
        return hitbox.GetOwner().UnitState == UnitState.Alive;
    }
}