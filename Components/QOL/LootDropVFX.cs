using System;
using System.Collections;
using BattleImprove;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LootDropVFX : MonoBehaviour{
    public ParticleSystem[] systems;
    private GameObject parentObject;
    private bool isScaling;
    private void Start() {
        this.transform.localPosition = new Vector3(0, 0.1f, 0);
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        
        foreach (var system in systems) {
            var parentScale = parentObject.transform.localScale;
            system.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        
        this.isScaling = true;
        StartCoroutine(StopScale());
    }

    private void Update() {
        bool isPressed = Input.GetMouseButton(1);
        foreach (var system in systems) {
            system.gameObject.SetActive(!isPressed);
        }
        
        if (!isScaling) return;
    }

    private IEnumerator StopScale() {
        yield return new WaitForSeconds(3);
        this.isScaling = false;
    }

    public void SetParent(GameObject parent) {
        parentObject = parent;
    }
}