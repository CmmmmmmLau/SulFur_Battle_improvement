
using System.Collections;
using BattleImprove;
using BattleImprove.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour{
    // Banner 
    private GameObject banner;
    private TMP_Text tmpEnemyName;
    private TMP_Text tmpWeaponName;
    private TMP_Text tmpExp;
    // Banner Animation
    private Animator bannerAnim;
    // Banner State
    private bool isShowing;
    
    // Kill Audio
    public AudioClip killSound;
    public AudioClip headShotKillSound;
    private AudioSource audioSource;
    
    // -----------DamageInfo-----------

    // Total Damage Counter
    private GameObject gameobjectTotalDamage;
    private TMP_Text tmpTotalDamage;
    
    
    private float timer;
    private int totalDamage;
    private int currentDamage;
    
    // DamageSource
    private GameObject damageInfoPlaceholder;
    private GameObject damageInfoContainer;
    
    private GameObject firstMessage;
    
    // Prefab
    public GameObject damageSourcePrefab;
    
    internal PluginData.AttackFeedback data;
    
    private void Awake() {
        isShowing = false;
        
        this.InitBanner();
        this.InitDamageInfo();

        this.ResetDamageCounter();
    }

    protected virtual GameObject InitDamageInfo() {
        var damageInfo = this.transform.Find("Damage").gameObject;
        gameobjectTotalDamage = damageInfo.transform.Find("Total").gameObject;
        tmpTotalDamage = gameobjectTotalDamage.GetComponent<TMP_Text>();
        
        damageInfoPlaceholder = damageInfo.transform.Find("Placeholder").gameObject;
        damageInfoContainer = damageInfo.transform.Find("Container").gameObject;

        return damageInfo;
    }

    protected virtual GameObject InitBanner() {
        banner = this.transform.Find("Banner").gameObject;
        tmpEnemyName = FindChild(banner.transform, "Enemy").GetComponent<TMP_Text>();
        tmpWeaponName = FindChild(banner.transform, "Weapon").GetComponent<TMP_Text>();
        tmpExp = FindChild(banner.transform, "Exp").GetComponent<TMP_Text>();
        bannerAnim = banner.GetComponent<Animator>();
        audioSource = banner.GetComponent<AudioSource>();

        return banner;
    }

    protected virtual void Start() {
        data = PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedback;
    }

    protected void Update() {
        UpdateTotalDamage();

        if (isShowing && (timer += Time.deltaTime) > 5f) {
            // Hide the damage info after 5 seconds
            this.ResetDamageCounter();
            
            // And hide the message
            HideMessage();
        }
#if DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            OnEnemyHit("Bullet Damage Type#" + Random.RandomRangeInt(0, 10), Random.RandomRangeInt(0, 100));
            OnEnemyKill("Enemy1", "Weapon1", "Exp1", false);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            OnEnemyHit("Bullet Damage Type#" + Random.RandomRangeInt(0, 10), Random.RandomRangeInt(0, 100));
            OnEnemyKill("Enemy2", "Weapon2", "Exp2", true);
        }
#endif
    }
    
    public virtual void OnEnemyKill(string enemyName, string weaponName, string exp, bool isHeadShot, bool isFar = false) {
        tmpEnemyName.text = enemyName;
        tmpWeaponName.text = weaponName;
        tmpExp.text = exp;
        
        StartCoroutine(ShowMessage());
        
        PlayKillAudio(isHeadShot, isFar);
    }

    public void OnEnemyHit(string type, int damage) {
        gameobjectTotalDamage.SetActive(true);
        totalDamage += damage;
        
        if (firstMessage != null && damageInfoContainer != null) {
            var components = damageInfoContainer.GetComponentsInChildren<DamageSource>();
            foreach (var damageSource in components) {
                if (damageSource.damageType == type) {
                    damageSource.damage += damage;
                    damageSource.Reset();
                    return;
                }
            }
        }
        
        AddDamageInfo(type, damage);
    }
    
    private static Transform FindChild(Transform parent, string name) {
        foreach (Transform child in parent) {
            if (child.name == name) {
                return child;
            }

            Transform result = FindChild(child, name);
            if (result != null) {
                return result;
            }
        }
        return null;
    }

    public void HideMessage() {
        isShowing = false;
        bannerAnim.SetTrigger("Fade");
    }

    private void ResetDamageCounter() {
        gameobjectTotalDamage.SetActive(false);
        totalDamage = 0;
        currentDamage = 0;
        timer = 0;
    }

    private IEnumerator ShowMessage() {
        if (!isShowing) {
            isShowing = true;
            bannerAnim.SetTrigger("Pop");
        }

        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(banner.transform as RectTransform);
    }

    private void UpdateTotalDamage() {
        // If the current damage is less than the total damage, update the total damage
        if (currentDamage < totalDamage) {
            timer = 0;

            var step = Mathf.Max(1, (totalDamage - currentDamage) / 100);
            currentDamage += step;
        
            tmpTotalDamage.text = currentDamage.ToString();
        };
    }

    protected virtual GameObject AddDamageInfo(string type, int damage) {
        var placeholder = this.GetPlaceholder();
        LayoutRebuilder.ForceRebuildLayoutImmediate(damageInfoPlaceholder.GetComponent<RectTransform>());
        
        firstMessage = GetDamageInfo(type, damage, placeholder);
        
        return firstMessage;
    }

    protected virtual GameObject GetPlaceholder() {
        var placeholder = new GameObject("Placeholder", typeof(RectTransform));
        placeholder.transform.SetParent(damageInfoPlaceholder.transform);
        placeholder.transform.SetAsFirstSibling();
        var rec = placeholder.GetComponent<RectTransform>();
        rec.sizeDelta = new Vector2(25, 25);

        return placeholder;
    }
    
    protected virtual GameObject GetDamageInfo(string type, int damage, GameObject placeholder) {
        var damageInfo = Instantiate(damageSourcePrefab, damageInfoContainer.transform);
        damageInfo.GetComponent<DamageSource>().InitMessage(type, damage, placeholder);
        
        return damageInfo;
    }

    protected virtual void PlayKillAudio(bool isHeadShot, bool isFar) {
        // audioSource.PlayOneShot(isFar ? headShotKillSound : killSound, 0.5f);
        audioSource.PlayOneShot(isFar ? headShotKillSound : killSound, data.messageVolume);
    }
}