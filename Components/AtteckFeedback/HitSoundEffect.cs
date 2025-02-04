using UnityEngine;

public class HitSoundEffect : MonoBehaviour {
    public AudioClip[] hitClose;
    public AudioClip[] hitFar;
    public AudioClip[] hitCrit;

    public void PlayHitSound(Vector3 position, bool isCrit, bool isFar = false) {
        if (isCrit)
            AudioSource.PlayClipAtPoint(hitCrit[Random.Range(0, hitCrit.Length)], position, 1f);
        else if (isFar)
            AudioSource.PlayClipAtPoint(hitFar[Random.Range(0, hitFar.Length)], position, 1f);
        else
            AudioSource.PlayClipAtPoint(hitClose[Random.Range(0, hitClose.Length)], position, 1f);
    }
}