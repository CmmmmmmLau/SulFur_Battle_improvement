using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleImprove.Components.QOL;

public class LootSpawnHelper : MonoBehaviour {
    private PluginData.DeadProtection data;

    private void Start() {
        data = PluginData.DataDict["DeadProtection"] as PluginData.DeadProtection;
    }

    public void SpawnItems(PluginData.DeadProtection keptItems, Transform transform) {
        this.StartCoroutine(SpawnWeapon(keptItems.weapons, transform));
    }

    private IEnumerator SpawnWeapon(List<InventoryData> weapons, Transform transform) {
        foreach (var item in weapons) {
            var itemDef = StaticInstance<LootManager>.Instance.GetItem(item.identifier);
            
            item.attachments = RandomDeleteElement(item.attachments, data.attachmentChance);
            item.enchantments = RandomDeleteElement(item.enchantments, data.enchantmentChance);
            
            var pickUp = StaticInstance<LootManager>.Instance.SpawnItem(itemDef, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.08f), LootSpawnBehaviour.None, false);
            Traverse.Create(pickUp).Field("inventoryData").SetValue(item);
            pickUp.SetAliveTimerMinBeforePickups(0.75f);
            yield return new WaitForSeconds(1f);
        }
        weapons.Clear();
        PluginData.SaveData();
    }
    
    private string[] RandomDeleteElement(string[] array, float chance) {
        var list = new List<string>(array);

        foreach (var element in list.ToList()) {
            var num = Random.Range(0f, 1f);
            Plugin.instance.LoggingInfo("Rolling for " + element + " with chance " + chance + " got " + num);
            if (chance < num) {
                list.Remove(element);
            }
        }
        return list.ToArray();
    }
}