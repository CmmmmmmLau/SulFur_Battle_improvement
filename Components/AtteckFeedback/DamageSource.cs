using TMPro;
using UnityEngine;

public class DamageSource : MonoBehaviour {
    public string damageType;
    public int damage;
    public TMP_Text message;
    private float timer;

    public void Reset() {
        timer = 0;
        message.text = damageType + " " + damage;
    }

    private void Start() {
        timer = 0;
        message = GetComponent<TMP_Text>();
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > 5f) Destroy(gameObject);
    }

    public void InitMessage(string type, int damage) {
        damageType = type;
        this.damage = damage;
        message.text = damageType + " " + damage;
        Reset();
    }
}