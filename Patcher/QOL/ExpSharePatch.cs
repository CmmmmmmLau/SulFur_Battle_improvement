using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Units;

namespace BattleImprove.Patcher.QOL;

public class ExpSharePatch {
    [HarmonyWrapSafe]
    [HarmonyPostfix, HarmonyPatch(typeof(Npc), "GiveExperience")]
    private static void GiveExperiencePrePatch(Npc __instance) {
        var lastUsedWeapon = StaticInstance<GameManager>.Instance.PlayerUnit.lastUsedWeapon;
        var secondWeapon = lastUsedWeapon.inventorySlot == InventorySlot.Weapon0
            ? InventorySlot.Weapon1
            : InventorySlot.Weapon0;
        var equippedWeapon = StaticInstance<GameManager>.Instance.PlayerUnit.GetComponent<EquipmentManager>()
            .EquippedHoldables;

        float exp = __instance.ExperienceOnKill;
        var proportion = Config.Proportion.Value;

        if (!equippedWeapon.ContainsKey(secondWeapon)) return;
        equippedWeapon[secondWeapon].AddExperience(exp * proportion);
    }
}