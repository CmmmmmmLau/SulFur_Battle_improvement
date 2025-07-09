using BattleImprove.PluginData;
using PerfectRandom.Sulfur.Core.Stats;
using UnityEngine;

namespace BattleImprove.MonoBehavior.Component.ImpactSound;

public class BF1Sound: MonoBehaviour, IImpactSound {
    [SerializeField]
    private AudioClip[] audioClip;
    [SerializeField]
    private AudioClip[] audioClipFar;
    [SerializeField]
    private AudioClip[] audioClipCrit;
        
    public void PlayAudio(Vector3 position, float distance, string fraction, string hitBodyPart, DamageType damageType) {
        if (hitBodyPart == "Head") {
            AudioSource.PlayClipAtPoint(audioClipCrit[Random.Range(0, audioClipCrit.Length)], position, ImpactSoundData.Instance.volumeCrit);
        } else {
            if (distance < 20) {
                AudioSource.PlayClipAtPoint(audioClip[Random.Range(0, audioClip.Length)], position, ImpactSoundData.Instance.volume);
            } else {
                AudioSource.PlayClipAtPoint(audioClipFar[Random.Range(0, audioClipFar.Length)], position, ImpactSoundData.Instance.volumeFar);
            }
        }
    }
}