using BattleImprove.Patches.QOL;
using HarmonyLib;

namespace BattleImprove.Patches;

public class Entry {
    public static void Load() {
        ExpShare.Load();
        DeadProtection.Load();
        HealthBar.Load();
        CrossHair.Load();

        var harmony = Harmony.CreateAndPatchAll(typeof(LoopDrop));
    }
}