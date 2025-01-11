using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KillMessage : MonoBehaviour {
    private struct KillMessageStruct {
        public string EnemyName;
        public string WeaponName;
        public string Exp;
    }
    
    private readonly Queue<KillMessageStruct> messageQueue = new();
    private bool isShowing;
    
    public TMP_Text tmpEnemyName;
    public TMP_Text tmpWeaponName;
    public TMP_Text tmpExp;
    
    public AudioClip killSound;
    public AudioClip headShotKillSound;
    public AudioSource audioSource;
    
    public Animator killMessageAnim;
    void Start() {
        isShowing = false;
    }
    
    void Update() {
        if (messageQueue.Count == 0 || isShowing) return;
        this.ShowKIllMessage();
    }
    
    public void AddKillMessage(string enemyName, string weaponName, string exp) {
        messageQueue.Enqueue(new KillMessageStruct {EnemyName = enemyName, WeaponName = weaponName, Exp = exp});
    }

    private void ShowKIllMessage() {    
        KillMessageStruct message = messageQueue.Dequeue();
        tmpEnemyName.text = message.EnemyName;
        tmpWeaponName.text = message.WeaponName;
        tmpExp.text = message.Exp;
        
        StartCoroutine(Showing());
    }

    private IEnumerator Showing() {
        this.isShowing = true;
        killMessageAnim.SetTrigger("Pop");
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform as RectTransform);
        yield return messageQueue.Count > 1 ? new WaitForSeconds(1.25f) : new WaitForSeconds(2);
        killMessageAnim.SetTrigger("Fade");
        yield return new WaitForSeconds(0.3f);
        this.isShowing = false;
    }
    
    public void OnEnemyKill(bool isHeadShot) {
        audioSource.PlayOneShot(isHeadShot ? headShotKillSound : killSound, 0.5f);
    }
}