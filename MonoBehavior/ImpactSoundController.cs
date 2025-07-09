using BattleImprove.MonoBehavior.Component.ImpactSound;
using BattleImprove.PluginData;
using PerfectRandom.Sulfur.Core.Stats;
using UnityEngine;

namespace BattleImprove.MonoBehavior;

public class ImpactSoundController: PluginInstance<ImpactSoundController> {
    private GameObject impactSoundObject;
    private IImpactSound impactSound;
    
    private void Start() {
        if (PrefabReference.impactSound.TryGetValue(ImpactSoundData.Instance.style, out var soundPrefab)) {
            impactSoundObject = Instantiate(soundPrefab, this.transform);
        } else {
            Debug.LogWarning($"Impact Sound style '{ImpactSoundData.Instance.style}' not found.");
            var defaultPrefab = PrefabReference.impactSound["BF1"];
            if (defaultPrefab != null) {
                impactSoundObject = Instantiate(defaultPrefab, this.transform);
            }
        }
        this.impactSound = impactSoundObject.GetComponent<IImpactSound>();
    }
    
    public void PlayImpactSound(Vector3 position, float distance, string fraction, string hitBodyPart, DamageType damageType) {
        impactSound.PlayAudio(position, distance, fraction, hitBodyPart, damageType);
    }

    public void ChangeStyle(string style) {
        if (PrefabReference.impactSound.TryGetValue(style, out var value)) {
            if (this.impactSound != null) {
                Destroy(this.impactSoundObject);
            }
            this.impactSoundObject = Instantiate(value, this.transform);
            this.impactSound = this.impactSoundObject.GetComponent<IImpactSound>();
        } else {
            Plugin.Logger.LogWarning("Impact Sound style '" + style + "' not found.");
        }
    }
}