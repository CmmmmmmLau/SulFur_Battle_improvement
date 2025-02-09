using BattleImprove;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LootParticle : MonoBehaviour{
    public ParticleSystem particleSystem;
    private void Start() {
        var main = particleSystem.main;
        this.transform.localPosition = new Vector3(0, 0, 0);
    }
}