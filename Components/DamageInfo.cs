using TMPro;
using UnityEngine;

public class DamageInfo : MonoBehaviour {
    public TMP_Text tmpTotalDamage;

    public GameObject damageSourcePrefab;
    private int currentDamage;
    private GameObject damageParent;
    private GameObject firstMessage;
    private GameObject messageContainer;

    private float timer;
    private int totalDamage;

    private void Start() {
        damageParent = gameObject.transform.Find("Total").gameObject;
        damageParent.SetActive(false);
        totalDamage = 0;
        currentDamage = 0;
        timer = 10;

        messageContainer = gameObject.transform.Find("Detail").gameObject;
    }

    private void Update() {
        UpdateDamage();
        timer += Time.deltaTime;
        if (!(timer > 5f)) return;
        damageParent.SetActive(false);
        timer = 0;
        totalDamage = 0;
        currentDamage = 0;
    }

    public void ShowDamageInfo(string type, int damage) {
        AddTotalDamage(damage);
        AddDamageSource(type, damage);
    }

    private void AddTotalDamage(int damage) {
        damageParent.SetActive(true);
        totalDamage += damage;
    }

    private void AddDamageSource(string type, int damage) {
        if (firstMessage != null && messageContainer != null) {
            var components = messageContainer.GetComponentsInChildren<DamageSource>();
            foreach (var damageSource in components) {
                if (damageSource.damageType == type) {
                    damageSource.damage += damage;
                    damageSource.Reset();
                    return;
                }
            }
        }

        firstMessage = Instantiate(damageSourcePrefab, messageContainer.transform);
        firstMessage.transform.SetAsFirstSibling();
        firstMessage.GetComponent<DamageSource>().InitMessage(type, damage);
    }

    private void UpdateDamage() {
        if (currentDamage >= totalDamage) return;
        timer = 0;

        var step = Mathf.Max(1, (totalDamage - currentDamage) / 100);
        currentDamage += step;
        
        tmpTotalDamage.text = currentDamage.ToString();
    }
}