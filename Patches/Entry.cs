using BattleImprove.Patches.QOL;

namespace BattleImprove.Patches;

public class Entry {
    public static void Load() {
        ExpShare.Load();
        DeadProtection.Load();
        HealthBar.Load();
    }
}