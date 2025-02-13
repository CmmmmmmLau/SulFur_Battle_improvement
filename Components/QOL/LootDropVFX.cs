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
        this.transform.localPosition = new Vector3(0, 0.01f, 0);
        this.isScaling = true;
        StartCoroutine(StopScale());
    }

    private void Update() {
        bool isPressed = Input.GetMouseButton(1);
        foreach (var system in systems) {
            system.gameObject.SetActive(!isPressed);
        }
        
        if (!isScaling) return;
        this.gameObject.transform.localScale = parentObject.transform.localScale;
        foreach (var system in systems) {
            var parentScale = parentObject.transform.localScale;
            system.transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);
        }
    }

    private IEnumerator StopScale() {
        yield return new WaitForSeconds(3);
        this.isScaling = false;
    }

    public void SetParent(GameObject parent) {
        parentObject = parent;
    }
}