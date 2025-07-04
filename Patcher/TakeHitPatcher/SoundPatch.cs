﻿using System;
using System.Linq;
using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patcher.TakeHitPatcher;

[HarmonyWrapSafe]
[HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
public class SoundPatch {
    private static PluginData.AttackFeedback data;
    
    private static void Postfix(Hitbox __instance, DamageType damageType, Vector3 collisionPoint) {
        if (PluginInstance<HitSoundEffect>.Instance == null) return;
        data ??= DataManager.AttackFeedbackData;
        
        if (__instance.Owner is Breakable || __instance.Owner.isPlayer) return;

        var target = __instance.GetOwner().gameObject.GetComponent<Npc>();

        if (target.UnitState == UnitState.Dead) return;
        if (!AttackFeedbackPatch.Enemies.Contains(target)) return;

        var player = StaticInstance<GameManager>.Instance.PlayerUnit;
        var distance = Vector3.Distance(player.EyesPosition
            , target.transform.position);

        
        if (damageType.id == DamageTypes.Critical || __instance.bodyPart.label == "Head") {
            var soundPosition = Vector3.LerpUnclamped(player.transform.position, collisionPoint, data.indicatorDistanceHeadShoot);
            PluginInstance<HitSoundEffect>.Instance.PlayHitSound(soundPosition, true, volume: data.indicatorVolume);
        }
        else {
            if (distance < 20) {
                var soundPosition = Vector3.LerpUnclamped(player.transform.position, collisionPoint, data.indicatorDistance);
                PluginInstance<HitSoundEffect>.Instance.PlayHitSound(soundPosition, false, volume: data.indicatorVolume);
            }else {
                var soundPosition = Vector3.LerpUnclamped(player.transform.position, collisionPoint, data.indicatorDistanceFar);
                PluginInstance<HitSoundEffect>.Instance.PlayHitSound(soundPosition, false, true, volume: data.indicatorVolume);
            }
        }
    }
}