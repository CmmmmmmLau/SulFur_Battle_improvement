using System;
using BattleImprove.PluginData;
using UnityEngine;
using UnityEngine.UI;

namespace BattleImprove.MonoBehavior.Component.CrossHair;

public class BF1CrossHair: MonoBehaviour, ICrossHair {
    private static readonly int Trigger = Animator.StringToHash("hit");

    [SerializeField] 
    private Animator animator;

    [SerializeField] 
    private Image[] image;

    private void Awake() {
        this.transform.position = new Vector3(0f, 0f, 0f);
        this.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void OnKill() {
        foreach (var image1 in image) {
            image1.enabled = true;
            image1.color = CrossHairData.Instance.killColor;
        }
        animator.SetTrigger(Trigger);
    }

    public void OnHit() {
        foreach (var image1 in image) {
            image1.enabled = true;
            image1.color = CrossHairData.Instance.hitColor;
        }
        animator.SetTrigger(Trigger);
    }

    public void OnIdle() {
        foreach (var image1 in image) {
            image1.color = CrossHairData.Instance.hitColor;
            image1.enabled = false;
        }
    }
}