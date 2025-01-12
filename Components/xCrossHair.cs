using UnityEngine;

public class xCrossHair : MonoBehaviour {
    public Animator crossHairAnim;

    public void HitTrigger() {
        crossHairAnim.SetTrigger("Hit");
    }

    public void KillTrigger() {
        crossHairAnim.SetTrigger("Kill");
    }
}