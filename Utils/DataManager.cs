using System.Collections.Generic;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.UI;

namespace BattleImprove.Utils;

[HarmonyPatch]
public static class DataManager {
    public static PluginData.Version VersionData;
    public static PluginData.AttackFeedback AttackFeedbackData;
    public static PluginData.DeadProtection DeadProtectionData;
    
    public static Dictionary<int, string> KillMessageStyle = new() {
        {0, "Battlefield 1"},
        {1, "Battlefield 5"}
    };

    public static void SetUpData() {
        SaveManager.LoadSaveFile();
    }

    public static void SaveAllData() {
        SaveVersionData();
        SaveAttackMessageData();
        SaveDeadProtectionData();
    }
    
    public static void SaveVersionData(bool reset = false) {
        SaveManager.SaveVersionData();
    }
    
    public static void SaveAttackMessageData(bool reset = false) {
        SaveManager.SaveAttackMessageData();
    }

    public static void SaveDeadProtectionData(bool reset = false) {
        if (reset) DataManager.DeadProtectionData.opened = false;
        
        if (DataManager.DeadProtectionData.opened) {
            DataManager.DeadProtectionData.weapons.Clear();
        }
        SaveManager.SaveDeadProtectionData();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PauseMenu), "ExitAction")]
    [HarmonyPatch(typeof(GameManager), "PlayerDied")]
    private static void ExitActionPostfix() {
        SaveAllData();
    }
}