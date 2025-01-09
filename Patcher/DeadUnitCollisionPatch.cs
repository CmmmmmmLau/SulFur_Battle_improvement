using System.Linq;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;

namespace ExpShare;

public class DeadUnitCollisionPatch {
    [HarmonyPrefix, HarmonyPatch(typeof(Projectile), "ReportBounceOnHitbox")]
    private static bool CheckPass(Projectile __instance, Hitbox hitbox) {
        return hitbox.GetOwner().UnitState != UnitState.Dead;
    }
}