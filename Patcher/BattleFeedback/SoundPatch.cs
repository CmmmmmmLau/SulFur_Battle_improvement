using System.Linq;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace ExpShare.Patcher.BattleFeedback;

public class SoundPatch {
    [HarmonyPostfix, HarmonyPatch(typeof(Hitbox), "TakeHit")]
    private static void PlayHitSound(Hitbox __instance, DamageType damageType, IDamager source, Vector3 collisionPoint) {
        if (__instance.Owner is Breakable || __instance.Owner.isPlayer) return;
        
        Npc target = __instance.GetOwner().gameObject.GetComponent<Npc>();
        
        if (target.UnitState == UnitState.Dead) return;
        if (!Plugin.StaticInstance.Enemies.Contains(target)) return;
        
        float distance = Vector3.Distance(StaticInstance<GameManager>.Instance.GetPlayerUnit().EyesPosition
            , target.transform.position);
        if (damageType.id == DamageTypes.Critical || __instance.bodyPart.label == "Head") {
            Plugin.StaticInstance.HitSoundClips.PlayHitSound(collisionPoint, true);
        } else {
            GameObject player = StaticInstance<GameManager>.Instance.GetPlayer();
            Vector3 soundPosition = Vector3.Normalize(collisionPoint - player.transform.position);
                    
            soundPosition = player.transform.position + soundPosition * 3;
            if (distance < 20) {
                Plugin.StaticInstance.HitSoundClips.PlayHitSound(soundPosition, false);
            } else {
                Plugin.StaticInstance.HitSoundClips.PlayHitSound(soundPosition, false, true);
            }
        }
    }
}