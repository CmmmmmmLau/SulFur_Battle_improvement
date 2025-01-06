using HarmonyLib;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace ExpShare;

public class DeadUnitCollisionPatch {
    // TODO: Implement this. This should remove the dead unit collision.
    
    // [HarmonyPrefix, HarmonyPatch(typeof(Hitbox), "GiveExperience")]
    // private static bool OnCollisionEnterPreFix(Hitbox __instance, Collision collision) {
    //     var projectilesLayer = Traverse.Create(__instance).Field("projectilesLayer").GetValue<int>();
    //     var npcProjectilesLayer = Traverse.Create(__instance).Field("npcProjectilesLayer").GetValue<int>();
    //     if (collision.gameObject.layer == projectilesLayer || collision.gameObject.layer == npcProjectilesLayer) {
    //         
    //     }
    //
    //     return true;
    // }
}