using System;
using BattleImprove.PluginData;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Units;

namespace BattleImprove.Patches.QOL;

public static class ExpShare {
    private static Hook hook; 
    
    public static void Load() {
        if (MiscData.Instance.enable) {
            if (ExpShare.hook == null) {
                hook = new Hook(AccessTools.Method(typeof(Npc), "GiveExperience"), ShareExperience);
            } else {
                hook.Apply();
            }
        }
    }

    public static void Unload() {
        if (hook != null) {
            hook.Undo();
        }
    }
    

    private static void ShareExperience(Action<Npc> orig, Npc self) {
        orig.Invoke(self);
        
        var lastUsedWeapon = StaticInstance<GameManager>.Instance.PlayerUnit.lastUsedWeapon;
        var secondWeapon = lastUsedWeapon.inventorySlot == InventorySlot.Weapon0
            ? InventorySlot.Weapon1
            : InventorySlot.Weapon0;
        var equippedWeapon = StaticInstance<GameManager>.Instance.PlayerUnit.GetComponent<EquipmentManager>()
            .EquippedHoldables;

        float exp = self.ExperienceOnKill;
        var proportion = MiscData.Instance.expShareProportion;

        if (!equippedWeapon.ContainsKey(secondWeapon)) return;
        equippedWeapon[secondWeapon].AddExperience(exp * proportion);
    }
}