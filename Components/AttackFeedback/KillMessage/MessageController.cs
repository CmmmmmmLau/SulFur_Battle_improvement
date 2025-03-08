
using System.Collections;
using BattleImprove;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour{
    // Banner 
    public GameObject Banner;
    public TMP_Text tmpEnemyName;
    public TMP_Text tmpWeaponName;
    public TMP_Text tmpExp;
    // Banner Animation
    public Animator BannerAnim;
    // Banner State
    private bool isShowing;
    
    // Kill Audio
    public AudioClip killSound;
    public AudioClip headShotKillSound;
    public AudioSource audioSource;
    
    // -----------DamageInfo-----------

    // Total Damage Counter
    public TMP_Text tmpTotalDamage;
    public GameObject TDGameObject;
    
    private float timer;
    private int totalDamage;
    private int currentDamage;
    
    // DamageSource
    public GameObject DamageInfoPlaceholder;
    public GameObject DamageInfoContainer;
    
    private GameObject firstMessage;
    
    // Prefab
    public GameObject damageSourcePrefab;
    public GameObject placeholderPreafab;
    
    private PluginData.AttackFeedback data;
    
    protected virtual void Start() {
        data = PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedback;
        isShowing = false;
        InitDamageInfo();
    }

    protected void Update() {
        UpdateTotalDamage();

        if (isShowing && (timer += Time.deltaTime) > 5f) {
            // Hide the damage info after 5 seconds
            TDGameObject.SetActive(false);
            timer = 0;
            totalDamage = 0;
            currentDamage = 0;
            
            // And hide the message
            HideMessage();
        }
#if DEBUG
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            OnEnemyKill("Enemy1", "Weapon", "Exp", false);
            OnEnemyHit("Bullet Damage Type#" + Random.RandomRangeInt(0, 10), Random.RandomRangeInt(0, 100));
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            OnEnemyHit("Bullet Damage Type#" + Random.RandomRangeInt(0, 10), Random.RandomRangeInt(0, 100));
        }
#endif
    }

    private void InitDamageInfo() {
        TDGameObject.SetActive(false);
        totalDamage = 0;
        currentDamage = 0;
        timer = 0;
    }
    
    public void OnEnemyKill(string enemyName, string weaponName, string exp, bool isHeadShot) {
        tmpEnemyName.text = enemyName;
        tmpWeaponName.text = weaponName;
        tmpExp.text = exp;
        
        StartCoroutine(ShowMessage());
        
        audioSource.PlayOneShot(isHeadShot ? headShotKillSound : killSound, 0.5f);
    }

    public void OnEnemyHit(string type, int damage) {
        TDGameObject.SetActive(true);
        totalDamage += damage;
        
        if (firstMessage != null && DamageInfoContainer != null) {
            var components = DamageInfoContainer.GetComponentsInChildren<DamageSource>();
            foreach (var damageSource in components) {
                if (damageSource.damageType == type) {
                    damageSource.damage += damage;
                    damageSource.Reset();
                    return;
                }
            }
        }

        var placeholder = Instantiate(placeholderPreafab, DamageInfoPlaceholder.transform);
        placeholder.transform.SetAsFirstSibling();
        
        firstMessage = Instantiate(damageSourcePrefab, DamageInfoContainer.transform);
        firstMessage.transform.position = placeholder.transform.position;
        firstMessage.GetComponent<DamageSource>().InitMessage(type, damage, placeholder);
    }

    private IEnumerator ShowMessage() {
        if (!isShowing) {
            isShowing = true;
            BannerAnim.SetTrigger("Pop");
        }

        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(Banner.transform as RectTransform);
    }

    public void HideMessage() {
        isShowing = false;
        BannerAnim.SetTrigger("Fade");
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
}