using System;
using System.Collections.Generic;
using System.Linq;
using BattleImprove.Components.QOL;
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
    [HarmonyWrapSafe]
    [HarmonyPrefix, HarmonyPatch(typeof(LootManager), "AddToChurchCollection")]
    private static void SavePlayerEquipment() {
        Plugin.LoggingInfo("Player died, saving weapon! Quick!!!");
        var itemData = DataManager.DeadProtectionData;
        var weapons = itemData.weapons;
        weapons.Clear();
        
        var equipment = StaticInstance<GameManager>.Instance.GetPlayerUnit().GetComponent<EquipmentManager>().EquippedHoldables;
        foreach (var inventoryItem in equipment.Select(item => item.Value)) {
            if (inventoryItem.SlotType != SlotType.Weapon) {
                continue;
            }
            var itemdef = inventoryItem.itemDefinition as WeaponSO;
            Plugin.LoggingInfo("Saved item: " + itemdef.LocalizedDisplayName);
            
            Plugin.LoggingInfo("Durability Current: " + inventoryItem.DurabilityCurrent);
            inventoryItem.ModifyDurability(-inventoryItem.DurabilityCurrent * (1 - itemData.weaponDurability));
            Plugin.LoggingInfo("Durability Modified: " + inventoryItem.DurabilityCurrent);
            
            Plugin.LoggingInfo("Grabbing item data...");
            var InventoryData = new InventoryData(itemdef.identifier, inventoryItem.gridPosition.x, inventoryItem.gridPosition.y, inventoryItem.quantity, 
                inventoryItem.currentAmmo, itemdef.caliber.identifier, inventoryItem.stats.SerializedAttributeData(),   
                inventoryItem.GetSerializedAttachments(), inventoryItem.GetSerializedEnchantments(), 
                inventoryItem.InventorySize.x, inventoryItem.InventorySize.y, false, false);
            
            Plugin.LoggingInfo("Saving item data...");
            weapons.Add(InventoryData);
        }
        
        DataManager.SaveDeadProtectionData(true);
    }
    
    [HarmonyWrapSafe]
    [HarmonyPostfix, HarmonyPatch(typeof(ChurchCollectionLootable), "Loot")]
    private static void ReturnPlayerEquipment(ChurchCollectionLootable __instance) {
        Plugin.LoggingInfo("Donation box opened, popping equipment back!");
        var transform = Traverse.Create(__instance).Field("lootSpawnTransform").GetValue<Transform>();
        if (transform == null) {
            transform = __instance.transform;
        }
        
        var ItemData = DataManager.DeadProtectionData;
        PluginInstance<LootSpawnHelper>.Instance.SpawnItems(ItemData, transform);
    }
}