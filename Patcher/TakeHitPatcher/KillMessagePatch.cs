using System;
using BattleImprove.Patcher.BattleFeedback;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;
using UnityEngine;

namespace BattleImprove.Patcher.TakeHitPatcher;

[HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
public class KillMessagePatch : AttackFeedbackPatch{
    private static void Postfix(Hitbox __instance, ref DamageSourceData source, Vector3 collisionPoint) {
        if(!TargetCheck(source, __instance)) return;
        if (Plugin.StaticInstance.KillMessage == null) return;


        if (IsAlive(__instance.Owner)) return;
        PlayKillAudio(__instance, source.sourceWeapon, collisionPoint);
        ShowKillMessage(__instance.Owner, source.sourceWeapon);
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

    private static bool IsAlive(Unit unit) {
        bool isAliveOrIncapacitated = unit.UnitState is UnitState.Alive or UnitState.Incapacitated;
        if (isAliveOrIncapacitated) {
            return true;
        }
        
        // If the unit is already killed, return true. used to make sure the kill feedback is only shown once
        if (Plugin.StaticInstance.KilledEnemies.Contains(unit) || !unit.LastDamagedBy.sourceUnit.isPlayer) return true;
        Plugin.StaticInstance.KilledEnemies.Add(unit);
        return false;
    }
}