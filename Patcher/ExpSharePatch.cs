using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Units;
using PerfectRandom.Sulfur.Core.Weapons;
using UnityEngine;

namespace ExpShare;

public class ExpSharePatch {
    [HarmonyPostfix, HarmonyPatch(typeof(Player), "Start")]
    private static void PlayerStartPatch(Player __instance) {
        __instance.playerUnit.gameObject.AddComponent<InfoPrinter>();
    }

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
        
        if (!Plugin.ShowInfo.Value) return;
        StaticInstance<GameManager>.Instance.GetPlayerUnit().GetComponent<InfoPrinter>().AddMessage($"Shared experience between weapons: {exp}({exp * proportion})", 10);
    }
}