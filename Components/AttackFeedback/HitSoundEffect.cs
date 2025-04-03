using BattleImprove.Utils;
using UnityEngine;

public class HitSoundEffect : PluginInstance<HitSoundEffect> {
    public AudioClip[] hitClose;
    public AudioClip[] hitFar;
    public AudioClip[] hitCrit;

    public void PlayHitSound(Vector3 position, bool isCrit, bool isFar = false, float volume = 1f) {
        if (isCrit)
            AudioSource.PlayClipAtPoint(hitCrit[Random.Range(0, hitCrit.Length)], position, volume);
        else if (isFar)
            AudioSource.PlayClipAtPoint(hitFar[Random.Range(0, hitFar.Length)], position, volume);
        else
            AudioSource.PlayClipAtPoint(hitClose[Random.Range(0, hitClose.Length)], position, volume);
    }
}