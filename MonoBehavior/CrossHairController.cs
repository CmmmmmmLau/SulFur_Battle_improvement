using BattleImprove.MonoBehavior.Component;
using BattleImprove.PluginData;
using UnityEngine;

namespace BattleImprove.MonoBehavior;

public class CrossHairController: PluginInstance<CrossHairController> {
    internal static GameObject canvas;
    private GameObject crossHairObject;
    private ICrossHair crossHair;

    private void Start() {
        if (PrefabReference.crossHair.TryGetValue(CrossHairData.Instance.style, out var crossHairPrefab)) {
            if (crossHairPrefab != null) {
                crossHairObject = Instantiate(crossHairPrefab, canvas.transform);
            }
        } else {
            Debug.LogWarning($"Crosshair style '{CrossHairData.Instance.style}' not found.");
            var defaultPrefab = PrefabReference.crossHair["BF1"];
            if (defaultPrefab != null) {
                crossHairObject = Instantiate(defaultPrefab, canvas.transform);
            }
        }
        
        this.crossHair = crossHairObject.GetComponent<ICrossHair>();
    }

    public void OnHit() {
        crossHair.OnHit();
    }

    public void OnKill() {
        crossHair.OnKill();
    }

    public void ChangeStyle(string style) {
        if (PrefabReference.crossHair.TryGetValue(style, out var value)) {
            if (crossHair != null) {
                Destroy(crossHairObject);
            }
            crossHairObject = Instantiate(value, canvas.transform);
            this.crossHair = crossHairObject.GetComponent<ICrossHair>();
        } else {
            Plugin.Logger.LogWarning($"Crosshair style '{style}' not found.");
        }
    }
}