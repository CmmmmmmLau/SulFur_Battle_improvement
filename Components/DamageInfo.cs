using System;
using System.ComponentModel;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Stats;
using TMPro;
using UnityEngine;

namespace ExpShare.Components
{
    public class DamageInfo : MonoBehaviour {
        private GameObject damageParent;
        public TMP_Text tmpTotalDamage;
        private int totalDamage;
        private int currentDamage;
        
        public GameObject damageSourcePrefab;
        private GameObject messageContainer;
        private GameObject firstMessage;
        
        private float timer;
        
        void Start() {
            this.damageParent = this.gameObject.transform.Find("Total").gameObject;
            this.damageParent.SetActive(false);
            totalDamage = 0;
            currentDamage = 0;

            this.messageContainer = this.gameObject.transform.Find("Detail").gameObject;
        }
        
        void Update() {
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
            this.damageParent.SetActive(true);
            totalDamage += damage;
        }
        
        private void AddDamageSource(string type, int damage) {
            if (firstMessage != null) {
                DamageSource component = firstMessage.GetComponent<DamageSource>();
                if (component.damageType == type) {
                    component.damage += damage;
                    component.Reset();
                    return;
                } 
            } 
            firstMessage = Instantiate(damageSourcePrefab, this.messageContainer.transform);
            firstMessage.transform.SetAsFirstSibling();
            firstMessage.GetComponent<DamageSource>().InitMessage(type, damage);
        }

        private void UpdateDamage() {
            if (currentDamage >= totalDamage) return;
            this.timer = 0;
            
            var step = Mathf.Max(1, (totalDamage - currentDamage) / 10);
            currentDamage += step;
            tmpTotalDamage.text = currentDamage.ToString();
        }
    }
}
