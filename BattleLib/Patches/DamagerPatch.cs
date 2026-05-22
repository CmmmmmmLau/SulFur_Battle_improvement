using System;
using BattleLib.Contexts;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleLib.Patches;

[HarmonyWrapSafe]
[HarmonyPatch(typeof(Unit), "ReceiveDamage",
    new Type[] { typeof(float), typeof(DamageTypes), typeof(DamageSourceData), typeof(Hitmesh.Data), typeof(Vector3) })]
public class DamagerPatch {
    private static void Prefix(Unit __instance, float damage, DamageTypes damageType, DamageSourceData sourceData,
        Hitmesh.Data hitbox, Vector3 hitPosition) {
        var ctx = new DamageContext {
            DamageAmount = damage,
            DamageType = damageType,
            DamageSourceData = sourceData,
            Hitbox = hitbox,
            HitPoint = hitPosition
        };

        switch (__instance) {
            case Breakable:
                break;
            case Npc:
                EventBus.Enemy.OnDamage.FirePreEvent(ctx);
                break;
            default: {
                if (__instance.owner.isPlayer) {
                    EventBus.Player.OnDamage.FirePreEvent(ctx);
                }

                break;
            }
        }
    }

    private static void Postfix(Unit __instance, float damage, DamageTypes damageType, DamageSourceData sourceData,
        Hitmesh.Data hitbox, Vector3 hitPosition) {
        var ctx = new DamageContext {
            DamageAmount = damage,
            DamageType = damageType,
            DamageSourceData = sourceData,
            Hitbox = hitbox,
            HitPoint = hitPosition
        };

        switch (__instance) {
            case Breakable:
                break;
            case Npc:
                EventBus.Enemy.OnDamage.FirePreEvent(ctx);
                break;
            default: {
                if (__instance.owner.isPlayer) {
                    EventBus.Player.OnDamage.FirePreEvent(ctx);
                }

                break;
            }
        }
    }
}