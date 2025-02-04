using System;
using UnityEngine;

public class xCrossHair : MonoBehaviour {
    public Animator crossHairAnim;

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
    }

    private void KillTrigger() {
        crossHairAnim.SetTrigger("Kill");
    }
}