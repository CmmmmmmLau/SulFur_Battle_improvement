using System;
using BattleImprove.Patcher.BattleFeedback;
using HarmonyLib;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patcher.TakeHitPatcher;

[HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
public class DamageInfoPatch : AttackFeedbackPatch {
    private static void Prefix(Hitbox __instance, out float __state) {
        __state = __instance.Owner.GetCurrentHealth();
    }

    private static void Postfix(Hitbox __instance, DamageType damageType, ref DamageSourceData source, Vector3 collisionPoint, float __state) {
        if(!TargetCheck(source, __instance)) return;
        
        if (Config.EnableDamageMessage.Value) {
            var damage = __state - __instance.Owner.GetCurrentHealth();
            if (damage > 0) {
                var type = "";
                if (source.sourceWeapon.IsMelee)
                    type += source.sourceWeapon.weaponDefinition.displayName;
                else
                    type += damageType.shortLabel + " " + source.sourceProjectile.CurrentCaliber.label + " " +
                            source.sourceProjectile.projectileType;

                Plugin.StaticInstance.DamageInfo.ShowDamageInfo(type, Convert.ToInt32(damage));
            }
        }
    }
}