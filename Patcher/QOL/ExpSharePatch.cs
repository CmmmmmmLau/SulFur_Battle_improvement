﻿using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Units;

namespace ExpShare.Patcher;

public class ExpSharePatch {
    [HarmonyPostfix, HarmonyPatch(typeof(Npc), "GiveExperience")]
    private static void GiveExperiencePrePatch(Npc __instance) {
        var lastUsedWeapon = StaticInstance<GameManager>.Instance.GetPlayerUnit().lastUsedWeapon;
        var secondWeapon = lastUsedWeapon.inventorySlot == InventorySlot.Weapon0
            ? InventorySlot.Weapon1
            : InventorySlot.Weapon0;
        var equippedWeapon = StaticInstance<GameManager>.Instance.GetPlayerUnit().GetComponent<EquipmentManager>()
            .EquippedHoldables;

        float exp = __instance.ExperienceOnKill;
        var proportion = Plugin.Proportion.Value;
        
        if (!equippedWeapon.ContainsKey(secondWeapon)) return;
        equippedWeapon[secondWeapon].AddExperience(exp * proportion);
    }
}