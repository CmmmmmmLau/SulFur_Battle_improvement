using PerfectRandom.Sulfur.Core.Stats;
using UnityEngine;

namespace BattleImprove.MonoBehavior.Component.ImpactSound;

public interface IImpactSound {
    void PlayAudio(Vector3 position, float distance, string fraction, string hitBodyPart, DamageType damageType);
}