using System.Collections;
using System.Collections.Generic;
using BattleImprove;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillMessage : MonoBehaviour {
    public TMP_Text tmpEnemyName;
    public TMP_Text tmpWeaponName;
    public TMP_Text tmpExp;

    public AudioClip killSound;
    public AudioClip headShotKillSound;
    public AudioSource audioSource;

    public Animator killMessageAnim;

    private readonly Queue<KillMessageStruct> messageQueue = new();
    private bool isShowing;
    private PluginData.AttackFeedback data;

    private void Start() {
        isShowing = false;
    }

    private void Update() {
        if (messageQueue.Count == 0 || isShowing) return;
        ShowKIllMessage();
    }

    public void AddKillMessage(string enemyName, string weaponName, string exp) {
        messageQueue.Enqueue(new KillMessageStruct {EnemyName = enemyName, WeaponName = weaponName, Exp = exp});
    }

    private void ShowKIllMessage() {
        var message = messageQueue.Dequeue();
        tmpEnemyName.text = message.EnemyName;
        tmpWeaponName.text = message.WeaponName;
        tmpExp.text = message.Exp;

        StartCoroutine(Showing());
    }

    private IEnumerator Showing() {
        isShowing = true;
        killMessageAnim.SetTrigger("Pop");
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        yield return messageQueue.Count > 1 ? new WaitForSeconds(0.8f) : new WaitForSeconds(1.5f);
        killMessageAnim.SetTrigger("Fade");
        yield return new WaitForSeconds(0.3f);
        isShowing = false;
    }

    public void OnEnemyKill(bool isHeadShot) {
        if (this.data == null) {
            this.data = PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedback;
        }
        audioSource.PlayOneShot(isHeadShot ? headShotKillSound : killSound, data.messageVolume);
    }

    private struct KillMessageStruct {
        public string EnemyName;
        public string WeaponName;
        public string Exp;
    }
}