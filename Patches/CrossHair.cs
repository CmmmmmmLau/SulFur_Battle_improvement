using System;
using BattleImprove.MonoBehavior;
using BattleImprove.PluginData;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patches;

public class CrossHair {
    private static Hook hook;
    
    public static void Load() {
        if (hook == null) {
            hook = new Hook(AccessTools.Method(typeof(Hitbox), 
                    nameof(Hitbox.TakeHit), 
                    new []{typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})
                , CrossHairTrigger);
        }
    }
    
    public static void Unload() {
        if (hook != null) {
            hook.Undo();
        }
    }

    private static void CrossHairTrigger(Action<Hitbox, float, DamageType, DamageSourceData, Vector3> orig, Hitbox self, float damage, DamageType damageType, DamageSourceData source, Vector3 collisionPoint) {
        var isAlive = self.Owner.UnitState is UnitState.Alive or UnitState.Incapacitated;
        
        orig?.Invoke(self, damage, damageType, source, collisionPoint);

        if (CrossHairData.Instance.enable && isAlive && source.sourceUnit.isPlayer) {
            if (self.Owner is Breakable || self.Owner.isPlayer) return;
            
            var isDead = self.Owner.UnitState is UnitState.Dead;
            if (isDead) {
                CrossHairController.Instance.OnKill();
            } else {
                CrossHairController.Instance.OnHit();
            }
        }
    }
}