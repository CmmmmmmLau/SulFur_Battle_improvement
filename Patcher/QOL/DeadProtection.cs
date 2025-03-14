using System;
using System.Collections.Generic;
using System.Linq;
using BattleImprove.Utils;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Stats;
using PerfectRandom.Sulfur.Core.World;
using PerfectRandom.Sulfur.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleImprove.Patcher.QOL;

public class DeadProtection {
    
    [HarmonyPrefix, HarmonyPatch(typeof(LootManager), "AddToChurchCollection")]
    private static void SavePlayerEquipment() {
        Plugin.instance.LoggingInfo("Player died, saving weapon! Quick!!!");
        var itemData = PluginData.DataDict["DeadProtection"] as PluginData.DeadProtection;
        var weapons = itemData.weapons;
        weapons.Clear();
        
        var equipment = StaticInstance<GameManager>.Instance.GetPlayerUnit().GetComponent<EquipmentManager>().EquippedHoldables;
        foreach (var inventoryItem in equipment.Select(item => item.Value)) {
            if (inventoryItem.SlotType != SlotType.Weapon) {
                continue;
            }
            var itemdef = inventoryItem.itemDefinition as WeaponSO;
            Plugin.instance.LoggingInfo("Saved item: " + itemdef.LocalizedDisplayName);
            
            Plugin.instance.LoggingInfo("Durability Current: " + inventoryItem.DurabilityCurrent);
            inventoryItem.ModifyDurability(-inventoryItem.DurabilityCurrent * (1 - itemData.weaponDurability));
            Plugin.instance.LoggingInfo("Durability Modified: " + inventoryItem.DurabilityCurrent);
            
            Plugin.instance.LoggingInfo("Grabbing item data...");
            var InventoryData = new InventoryData(itemdef.identifier, inventoryItem.gridPosition.x, inventoryItem.gridPosition.y, inventoryItem.quantity, 
                inventoryItem.currentAmmo, itemdef.caliber.identifier, inventoryItem.stats.SerializedAttributeData(),   
                inventoryItem.GetSerializedAttachments(), inventoryItem.GetSerializedEnchantments(), 
                inventoryItem.InventorySize.x, inventoryItem.InventorySize.y, false, false);
            
            Plugin.instance.LoggingInfo("Saving item data...");
            weapons.Add(InventoryData);
        }
        PluginData.SaveData();
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(ChurchCollectionLootable), "Loot")]
    private static void ReturnPlayerEquipment(ChurchCollectionLootable __instance) {
        Plugin.instance.LoggingInfo("Donation box opened, popping equipment back!");
        var transform = Traverse.Create(__instance).Field("lootSpawnTransform").GetValue<Transform>();
        if (transform == null) {
            transform = __instance.transform;
        }
        
        var ItemData = PluginData.DataDict["DeadProtection"] as PluginData.DeadProtection;
        StaticInstance.LootSpawnHelper.SpawnItems(ItemData, transform);
    }
}