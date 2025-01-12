using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ExpShare
{
    public class DamageSource : MonoBehaviour {
        private float timer;
        public string damageType;
        public int damage;
        public TMP_Text message;
        
        private void Start() {
            timer = 0;
            message = this.GetComponent<TMP_Text>();
        }
        
        private void Update() {
            timer += Time.deltaTime;
            if (timer > 5f) {
                Destroy(gameObject);
            }
        }
        
        public void InitMessage(string type, int damage) {
            this.damageType = type;
            this.damage = damage;
            message.text = damageType + " " + damage;
            Reset();
        }

        public void Reset() {
            timer = 0;
            message.text = damageType + " " + damage;
        }
    }
}
