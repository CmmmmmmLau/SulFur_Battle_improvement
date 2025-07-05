using System;
using System.Collections;
using UnityEngine;

namespace BattleImprove.MonoBehavior.Component;

public class LootDropVFX: MonoBehaviour {
    public ParticleSystem[] systems;
    private GameObject parentObject;
    private bool isScaling;

    private void Awake() {
        this.transform.localPosition = new Vector3(0, 0.1f, 0);
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Start() {
        foreach (var system in systems) {
            var parentScale = parentObject.transform.localScale;
            system.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }

    private void Update() {
        bool isPressed = Input.GetMouseButton(1);
        foreach (var system in systems) {
            system.gameObject.SetActive(!isPressed);
        }
    }

    public void SetParent(GameObject parent) {
        parentObject = parent;
    }
}