using System;
using BattleImprove.MonoBehavior;
using BattleImprove.PluginData;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Patches;

public class ImpactSound {
    private static Hook hook;

    public static void Load() {
        if (hook == null) {
            hook = new Hook(AccessTools.Method(typeof(Hitbox), 
                    nameof(Hitbox.TakeHit), 
                    new []{typeof(float), typeof(DamageType), typeof(DamageSourceData), typeof(Vector3)})
                , ImpactSoundPatch);
        } else {
            hook.Apply();
        }
    }
    
    public static void Unload() {
        if (hook != null) {
            hook.Undo();
        }
    }

    private static void ImpactSoundPatch(Action<Hitbox, float, DamageType, DamageSourceData, Vector3> orig, Hitbox self, float damage, DamageType damageType, DamageSourceData source, Vector3 collisionPoint) {
        orig?.Invoke(self, damage, damageType, source, collisionPoint);
        
        var isAlive = self.Owner.UnitState is UnitState.Alive or UnitState.Incapacitated;
        if (ImpactSoundData.Instance.enable && isAlive && source.sourceUnit.isPlayer) {
            if (self.Owner is Breakable || self.Owner.isPlayer) return;

            var target = self.Owner;
            var player = source.sourceUnit;
            
            var distance = Vector3.Distance(player.EyesPosition, target.transform.position);
            var controller = PluginInstance<ImpactSoundController>.Instance;
            
            var pos = Vector3.LerpUnclamped(player.EyesPosition, collisionPoint, 0.5f);

            controller.PlayImpactSound(pos, distance, self.GetOwner().Faction.identifier, self.bodyPart.label, damageType);
        }
    }
}