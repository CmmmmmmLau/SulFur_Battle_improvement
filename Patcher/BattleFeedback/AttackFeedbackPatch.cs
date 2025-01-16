using System;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;
using UnityEngine;

namespace BattleImprove.Patcher.BattleFeedback;

[HarmonyPatch]
public class AttackFeedbackPatch {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Hitbox), "TakeHit")]
    private static void AddDamageMessage(Hitbox __instance, out float __state) {
        __state = __instance.Owner.GetCurrentHealth();
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Hitbox), "TakeHit")]
    // private static void tempname(Hitbox __instance, float damage, IDamager source, Vector3 collisionPoint) {
    //     if (!source.SourceUnit.isPlayer) return;
    //     if (__instance.Owner is Breakable || __instance.Owner.isPlayer  ||__instance.Owner.UnitState == UnitState.Alive) return;
    //     if (IsDead(__instance.Owner)) return;
    //     
    //     float distance = Vector3.Distance(StaticInstance<GameManager>.Instance.GetPlayerUnit().EyesPosition, collisionPoint);
    //     var weapon = source.SourceWeapon.holdableWeightClass;
    //     bool isFarRangeWeapon = weapon is HoldableWeightClass.Rifle or HoldableWeightClass.Sniper;
    //
    //     if (distance > 20 && isFarRangeWeapon){
    //         Plugin.StaticInstance.KillMessage.OnEnemyKill(__instance.bodyPart.label == "Head");
    //     } else {    
    //         Plugin.StaticInstance.KillMessage.OnEnemyKill(false);
    //     }
    // }
    //
    private static void AttackFeedback(Hitbox __instance, DamageType damageType, IDamager source,
        Vector3 collisionPoint, float __state) {
        // Ignore if the damage source is not the player
        if (!source.SourceUnit.isPlayer) return;
        // Ignore if the hitbox owner is a breakable, player
        if (__instance.Owner is Breakable || __instance.Owner.isPlayer) return;

        if (Config.EnableDamageMessage.Value) {
            var damage = __state - __instance.Owner.GetCurrentHealth();
            if (damage > 0) {
                var type = "";
                if (source.SourceWeapon.IsMelee)
                    type += source.SourceWeapon.weaponDefinition.displayName;
                else
                    type += damageType.shortLabel + " " + source.SourceProjectile.CurrentCaliber.label + " " +
                            source.SourceProjectile.projectileType;

                Plugin.StaticInstance.DamageInfo.ShowDamageInfo(type, Convert.ToInt32(damage));
            }
        }
        
        // Check if the hitbox owner is alive, if so, play hit animation and skip the rest
        if (PlayHitAnimation(__instance.Owner)) return;
        
        if (!Config.EnableDamageMessage.Value) return;
        // Show damage info
        // Check if the hitbox owner is dead on current shoot, if so, play kill audio and show kill message
        if (IsDead(__instance.Owner)) return;
        PlayKillAudio(__instance, source.SourceWeapon, collisionPoint);
        ShowKillMessage(__instance.Owner, source.SourceWeapon);
    }
    
    private static bool PlayHitAnimation(Unit unit) {
        bool isAliveOrIncapacitated = unit.UnitState is UnitState.Alive or UnitState.Incapacitated;
        bool isXCrossHairEnabled = Config.EnableXCrossHair.Value;

        if (isXCrossHairEnabled && isAliveOrIncapacitated) {
            Plugin.StaticInstance.CrossHair.StartTrigger("Hit");
        }

        return isAliveOrIncapacitated;
    }

    private static void PlayKillAudio(Hitbox hitbox, Weapon weapon, Vector3 collisionPoint) {
        var distance = Vector3.Distance(StaticInstance<GameManager>.Instance.GetPlayerUnit().EyesPosition,
            collisionPoint);
        var isFarRangeWeapon = weapon.holdableWeightClass is HoldableWeightClass.Rifle or HoldableWeightClass.Sniper;

        if (distance > 20 && isFarRangeWeapon)
            Plugin.StaticInstance.KillMessage.OnEnemyKill(hitbox.bodyPart.label == "Head");
        else
            Plugin.StaticInstance.KillMessage.OnEnemyKill(false);
    }

    private static void ShowKillMessage(Unit enemy, Weapon weapon) {
        var enemyName = enemy.SourceName;
        var weaponName = weapon.weaponDefinition.displayName;
        var exp = Convert.ToString(enemy.ExperienceOnKill);

        Plugin.StaticInstance.KillMessage.AddKillMessage(enemyName, weaponName, exp);
    }

    private static bool IsDead(Unit unit) {
        // If the unit is already killed, return true. used to make sure the kill feedback is only shown once
        if (Plugin.StaticInstance.KilledEnemies.Contains(unit)) return true;
        Plugin.StaticInstance.KilledEnemies.Add(unit);
        if (Config.EnableXCrossHair.Value) {
            Plugin.StaticInstance.CrossHair.StartTrigger("Kill");
        }
        return false;
    }
}