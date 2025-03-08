using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DamageSource : MonoBehaviour {
    public string damageType;
    public int damage;
    public TMP_Text message;
    
    private float timer;
    private GameObject placeholder;

    public void Reset() {
        timer = 0;
        message.text = damageType + " " + damage;
    }

    private void Start() {
        timer = 0;
        message = GetComponent<TMP_Text>();
    }

    private void Update() {
        this.transform.position = Vector3.MoveTowards(this.transform.position, placeholder.transform.position, 200f * Time.deltaTime);
        
        timer += Time.deltaTime;
        if (timer > 5f) Destroy(gameObject);
    }

    public void InitMessage(string type, int damage, GameObject placeholder) {
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.damageType = type;
        this.damage = damage;
        this.message.text = damageType + " " + damage;
        this.placeholder = placeholder;
        Reset();
    }
}