﻿using System;
using System.Linq;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patcher.BattleFeedback;

[HarmonyPatch]
public class SoundPatch {
    private static PluginData.AttackFeedbackData data;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Hitbox), "TakeHit", new Type[] {typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})]
    private static void PlayHitSound(Hitbox __instance, DamageType damageType, Vector3 collisionPoint) {
        data ??= PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedbackData;
        
        if (__instance.Owner is Breakable || __instance.Owner.isPlayer) return;

        var target = __instance.GetOwner().gameObject.GetComponent<Npc>();

        if (target.UnitState == UnitState.Dead) return;
        if (!Plugin.StaticInstance.Enemies.Contains(target)) return;

        
        var distance = Vector3.Distance(StaticInstance<GameManager>.Instance.GetPlayerUnit().EyesPosition
            , target.transform.position);
        var player = StaticInstance<GameManager>.Instance.GetPlayer();
        
        if (damageType.id == DamageTypes.Critical || __instance.bodyPart.label == "Head") {
            var soundPosition = Vector3.LerpUnclamped(player.transform.position, collisionPoint, data.indicatorDistanceHeadShoot);
            Plugin.StaticInstance.HitSoundClips.PlayHitSound(soundPosition, true, volume: data.indicatorVolume);
        }
        else {
            if (distance < 20) {
                var soundPosition = Vector3.LerpUnclamped(player.transform.position, collisionPoint, data.indicatorDistance);
                Plugin.StaticInstance.HitSoundClips.PlayHitSound(soundPosition, false, volume: data.indicatorVolume);
            }else {
                var soundPosition = Vector3.LerpUnclamped(player.transform.position, collisionPoint, data.indicatorDistanceFar);
                Plugin.StaticInstance.HitSoundClips.PlayHitSound(soundPosition, false, true, volume: data.indicatorVolume);
            }
        }
    }
}