using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleImprove.PluginData;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using UnityEngine;

namespace BattleImprove.MonoBehavior;

public class LootSpawnHelper: PluginInstance<LootSpawnHelper> {
    public void SpawnItems(KeptWeaponData keptItems, Transform transform) {
        this.StartCoroutine(SpawnWeapon(keptItems.weapons, transform));
        keptItems.opened = true;
    }

    private IEnumerator SpawnWeapon(List<InventoryData> weapons, Transform transform) {
        var data = DeadProtectionData.Instance;
        
        foreach (var item in weapons) {
            var itemDef = StaticInstance<LootManager>.Instance.GetItem(item.identifier) as WeaponSO;
            item.attachments = RandomDeleteElement(item.attachments, data.attachmentChance);
            item.enchantments = RandomDeleteElement(item.enchantments, data.enchantmentChance);
            
            var num = Random.Range(0f, 1f);
            Plugin.Logger.LogInfo("Rolling for " + item.caliber + " with chance " + data.barrelChance + " got " + num);
            if (num < data.barrelChance) {
                item.caliber = itemDef.caliber.identifier;
            } 

            var room = StaticInstance<GameManager>.Instance.PlayerUnit.currentRoom;
            
            // var pickUp = StaticInstance<LootManager>.Instance.SpawnItem(itemDef, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.08f), LootSpawnBehaviour.None, false);
            var pickUp = StaticInstance<InteractionManager>.Instance.SpawnPickup(
                new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.08f),
                false,
                itemDef,
                room,
                item,
                null,
                0.75f);
            yield return new WaitForSeconds(1f);
        }
    }
    
    private string[] RandomDeleteElement(string[] array, float chance) {
        var list = new List<string>(array);

        foreach (var element in list.ToList()) {
            var num = Random.Range(0f, 1f);
            Plugin.Logger.LogInfo("Rolling for " + element + " with chance " + chance + " got " + num);
            if (num < chance) {
                list.Remove(element);
            }
        }
        return list.ToArray();
    }
}