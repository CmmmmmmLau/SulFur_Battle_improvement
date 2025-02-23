using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.World;
using UnityEngine;

namespace BattleImprove.Components.QOL;

public class LootSpawnHelper : MonoBehaviour {
    public void SpawnItems(PluginData.DeadProtection keptItems, Transform transform) {
        this.StartCoroutine(SpawnWeapon(keptItems.weapons, transform));
        this.StartCoroutine(SpawnModify(keptItems.weaponModify, transform));
    }

    private IEnumerator SpawnWeapon(List<InventoryData> weapons, Transform transform) {
        foreach (var item in weapons) {
            var itemDef = StaticInstance<LootManager>.Instance.GetItem(item.identifier);
            var pickUp = StaticInstance<LootManager>.Instance.SpawnItem(itemDef, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.08f), LootSpawnBehaviour.None, false);
            Traverse.Create(pickUp).Field("inventoryData").SetValue(item);
            pickUp.SetAliveTimerMinBeforePickups(0.75f);
            yield return new WaitForSeconds(1f);
        }
        weapons.Clear();
        PluginData.SaveData();
    }
    
    private IEnumerator SpawnModify(List<string> modfiles, Transform transform) {
        foreach (var item in modfiles) {
            var itemDef = StaticInstance<LootManager>.Instance.GetItem(item);
            StaticInstance<LootManager>.Instance.SpawnItem(itemDef, transform.position, LootSpawnBehaviour.None, false).SetAliveTimerMinBeforePickups(0.75f);
            yield return new WaitForSeconds(0.1f);
        }
        modfiles.Clear();
        PluginData.SaveData();
    }
}