using System;
using System.Linq;
using BattleImprove.MonoBehavior;
using BattleImprove.PluginData;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Gameplay;
using UnityEngine;

namespace BattleImprove.Patches.QOL;

public class DeadProtection {
    private static Hook hook;
    private static Hook hook2;
    
    public static void Load() {
        if (DeadProtectionData.Instance.enable) {
            if (hook == null) {
                hook = new Hook(AccessTools.Method(typeof(LootManager), nameof(LootManager.AddToChurchCollection)), SavePlayerEquipment);
                hook2 = new Hook(AccessTools.Method(typeof(ChurchCollectionLootable), "Loot"), ReturnPlayerEquipment);
            } else {
                hook.Apply();
                hook2.Apply();
            }
        }
    }

    public static void Unload() {
        if (hook != null) {
            hook.Undo();
            hook2.Undo();
        }
    }

    private static void SavePlayerEquipment(Action<LootManager> orig, LootManager self) {
        Plugin.Logger.LogInfo("Saving player equipment...");
        var weapons = KeptWeaponData.Instance.weapons;
        weapons.Clear();
        
        var equipment = StaticInstance<GameManager>.Instance.PlayerUnit.GetComponent<EquipmentManager>().EquippedHoldables;
        foreach (var inventoryItem in equipment.Select(item => item.Value)) {
            if (inventoryItem.SlotType != SlotType.Weapon) {
                continue;
            }
            var itemdef = inventoryItem.itemDefinition as WeaponSO;
            Plugin.Logger.LogInfo("Saved item: " + itemdef.LocalizedDisplayName);
            
            Plugin.Logger.LogInfo("Durability Current: " + inventoryItem.DurabilityCurrent);
            inventoryItem.ModifyDurability(-inventoryItem.DurabilityCurrent * (1 - DeadProtectionData.Instance.weaponDurability));
            Plugin.Logger.LogInfo("Durability Modified: " + inventoryItem.DurabilityCurrent);
            
            Plugin.Logger.LogInfo("Grabbing item data...");
            var InventoryData = new InventoryData(itemdef.identifier, inventoryItem.gridPosition.x, inventoryItem.gridPosition.y, inventoryItem.quantity, 
                inventoryItem.currentAmmo, inventoryItem.CurrentCaliber.identifier, inventoryItem.stats.SerializedAttributeData(),   
                inventoryItem.GetSerializedAttachments(), inventoryItem.GetSerializedEnchantments(), 
                inventoryItem.InventorySize.x, inventoryItem.InventorySize.y, false, false);
            
            Plugin.Logger.LogInfo("Saving item data...");
            weapons.Add(InventoryData);
        }
        KeptWeaponData.Save();
        KeptWeaponData.Instance.opened = false;
        
        orig?.Invoke(self);
    }
    
    private static void ReturnPlayerEquipment(Action<ChurchCollectionLootable> orig, ChurchCollectionLootable self) {
        orig?.Invoke(self);
        
        Plugin.Logger.LogInfo("Donation box opened, popping equipment back!");
        var transform = Traverse.Create(self).Field("lootSpawnTransform").GetValue<Transform>();
        if (transform == null) {
            transform = self.transform;
        }
        
        PluginInstance<LootSpawnHelper>.Instance.SpawnItems(KeptWeaponData.Instance, transform);
    }
}