using System;
using BattleLib.Context;
using BattleLib.Events;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleLib.Patches.Combat;

[HarmonyPatch(typeof(Unit), nameof(Unit.ReceiveDamage),  new Type[] { typeof(float), typeof(DamageTypes), typeof(DamageSourceData), typeof(Hitmesh.Data), typeof(Vector3) })]
public class DamagedPatch {
    private static void Prefix(Unit __instance, float damage, DamageTypes damageType, DamageSourceData sourceData, Hitmesh.Data hitbox, Vector3 hitPosition, out HandlerStateContainer? __state) {
        var context = new DamagedContext(__instance, damage, damageType, sourceData, hitbox, hitPosition);
        
        __state = EventBus.Enemy.OnDamaged.InvokePrefix(context);
    }
    
    private static void Postfix(Unit __instance, float damage, DamageTypes damageType, DamageSourceData sourceData, Hitmesh.Data hitbox, Vector3 hitPosition, HandlerStateContainer? __state) {
        var context = new DamagedContext(__instance, damage, damageType, sourceData, hitbox, hitPosition);
        
        EventBus.Enemy.OnDamaged.InvokePostfix(context, __state);
    }
}