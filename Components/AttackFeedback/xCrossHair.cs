using BattleImprove;
using BattleImprove.Utils;
using UnityEngine;
using UnityEngine.UI;

public class xCrossHair : PluginInstance<xCrossHair> {
    public Animator crossHairAnim;

    private PluginData.AttackFeedback data;
    private Image[] images;

    private void Start() {
        data = DataManager.AttackFeedbackData;
        
        images = GetComponentsInChildren<Image>();
    }

    public void StartTrigger(string type) {
        
        switch (type) {
            case "Hit":
                HitTrigger();
                break;
            case "Kill":
                KillTrigger();
                break;
        }
    }

    private void HitTrigger() {
        crossHairAnim.SetTrigger("Hit");
        this.SetHitColor();
    }

    private void KillTrigger() {
        crossHairAnim.SetTrigger("Kill");
        this.SetKillColor();
    }

    private void SetHitColor() {
        foreach (var image in images) {
            image.color = data.hitColor;
        }
    }

    private void SetKillColor() {
        foreach (var image in images) {
            image.color = data.killColor;
        }
    }
}