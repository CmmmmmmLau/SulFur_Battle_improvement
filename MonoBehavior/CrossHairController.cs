using System;
using BattleImprove.MonoBehavior.Component;
using BattleImprove.PluginData;
using UnityEngine;

namespace BattleImprove.MonoBehavior;

public class CrossHairController: PluginInstance<CrossHairController> {
    internal static GameObject canvas;
    private ICrossHair crossHair;

    private void Start() {
        if (PrefabReference.crossHair.TryGetValue(CrossHairData.Instance.style, out var crossHairPrefab)) {
            if (crossHairPrefab != null) {
                var crossHairObject = Instantiate(crossHairPrefab, canvas.transform);
                this.crossHair = crossHairObject.GetComponent<ICrossHair>();
            }
        } else {
            Debug.LogWarning($"Crosshair style '{CrossHairData.Instance.style}' not found.");
            var defaultPrefab = PrefabReference.crossHair["BF1"];
            if (defaultPrefab != null) {
                var crossHairObject = Instantiate(defaultPrefab, canvas.transform);
                this.crossHair = crossHairObject.GetComponent<ICrossHair>();
            }
        }

    }

    public void OnHit() {
        crossHair.OnHit();
    }

    public void OnKill() {
        crossHair.OnKill();
    }

    public void ChangeStyle(string style) {
        if (crossHair != null) {
            Destroy(crossHair as MonoBehaviour);
        }

        var crossHairPrefab = PrefabReference.crossHair[style];
        if (crossHairPrefab != null) {
            var crossHairObject = Instantiate(crossHairPrefab, canvas.transform);
            this.crossHair = crossHairObject.GetComponent<ICrossHair>();
        } else {
            Debug.LogWarning($"Crosshair style '{style}' not found.");
        }
    }
}