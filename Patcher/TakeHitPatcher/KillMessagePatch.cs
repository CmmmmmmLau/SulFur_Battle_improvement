using System;
using System.Collections.Generic;
using BattleImprove.Patcher.BattleFeedback;
using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;
using UnityEngine;

namespace BattleImprove.Patcher.TakeHitPatcher;

[HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
public class KillMessagePatch : AttackFeedbackPatch{
    private static List<Unit> KilledEnemies;
    
    private static void Prefix(Hitbox __instance, out float __state) {
        __state = __instance.Owner.GetCurrentHealth();
    }
    
    private static void Postfix(Hitbox __instance, ref DamageSourceData source, Vector3 collisionPoint) {
        if (StaticInstance.KillMessage == null) return;
        if(!TargetCheck(source, __instance)) return;


        if (IsAlive(__instance.Owner)) return;
        
        var distance = Vector3.Distance(StaticInstance<GameManager>.Instance.GetPlayerUnit().EyesPosition,
            collisionPoint);
        var isFarRangeWeapon = source.sourceWeapon.holdableWeightClass is HoldableWeightClass.Rifle or HoldableWeightClass.Sniper;
        
        var enemyName = __instance.Owner.SourceName;
        var weaponName = source.sourceWeapon.weaponDefinition.displayName;
        var exp = Convert.ToString(__instance.Owner.ExperienceOnKill);
        
        StaticInstance.KillMessage.OnEnemyKill(enemyName, weaponName, exp, __instance.bodyPart.label == "Head", distance > 20 && isFarRangeWeapon);
    }

    private static bool IsAlive(Unit unit) {
        bool isAliveOrIncapacitated = unit.UnitState is UnitState.Alive or UnitState.Incapacitated;
        if (isAliveOrIncapacitated) {
            return true;
        }
        
        // If the unit is already killed, return true. used to make sure the kill feedback is only shown once
        if (KilledEnemies.Contains(unit) || !unit.LastDamagedBy.sourceUnit.isPlayer) return true;
        KilledEnemies.Add(unit);
        return false;
    }
    
    [HarmonyPrefix, HarmonyPatch(typeof(InputReader), "LoadingContinue")]
    private static void InitList() {
        KilledEnemies = new List<Unit>();
    }
}