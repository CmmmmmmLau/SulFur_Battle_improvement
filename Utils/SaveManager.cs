using System;
using System.Collections.Generic;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;

namespace BattleImprove.Utils;

public static class SaveManager {
    public const string SaveFileName = "BattleImproveSaveData";

    public static void LoadSaveFile() {
        try {
            
            if (ES3.KeyExists("CmPlugin", SulfurSave.Imp.saveSettings)) {
                TransferSaveData();
            } 
            
            if (ES3.FileExists(SaveFileName)) {
                LoadAll();
            } else {
                Plugin.LoggingInfo("Save data not found, creating new one...");
                LoadDefaults();
                Plugin.LoggingInfo("Save data created!");
            }
        } catch (Exception e) {
            Console.WriteLine(e);
            Plugin.LoggingInfo("Failed to load save data, creating new one...");
            LoadDefaults();
            throw;
        }
    }

    private static void LoadAll() {
        LoadVersionData();
        LoadAttackMessageData();
        LoadDeadProtectionData();
    }

    private static void LoadDeadProtectionData() {
        var data = ES3.Load("DeadProtection", SaveFileName, new PluginData.DeadProtection());
        DataManager.DeadProtectionData = data;
    }

    private static void LoadAttackMessageData() {
        var data = ES3.Load("AttackFeedback", SaveFileName, new PluginData.AttackFeedback());
        DataManager.AttackFeedbackData = data;
    }

    private static void LoadVersionData() {
        var data = ES3.Load("Version", SaveFileName, new PluginData.Version());
        DataManager.VersionData = data;
    }
    
    
    public static void SaveAll() {
        SaveVersionData();
        SaveAttackMessageData();
        SaveDeadProtectionData();
    }
    
    public static void SaveVersionData() {
        ES3.Save("Version", DataManager.VersionData, SaveManager.SaveFileName);
    }
    
    public static void SaveAttackMessageData() {
        ES3.Save("AttackFeedback", DataManager.AttackFeedbackData, SaveManager.SaveFileName);
    }
    
    public static void SaveDeadProtectionData(bool reset = false) {
        ES3.Save("DeadProtection", DataManager.DeadProtectionData, SaveManager.SaveFileName);
    }

    private static void TransferSaveData() {
        Plugin.LoggingInfo("Save data found in the vanilla save file, transfers it to the new file...");
        var dataDict = SulfurSave.Imp.Load("CmPlugin", new Dictionary<string, PluginData>());
        ES3.DeleteKey("CmPlugin", SulfurSave.Imp.saveSettings);

        var data1 = dataDict["BattleImprove"] as PluginData.Version;
        Traverse.IterateFields(data1, RoundUp);
        ES3.Save("BattleImprove", data1, SaveManager.SaveFileName);
        
        var data2 = dataDict["AttackFeedback"] as PluginData.AttackFeedback;
        Traverse.IterateFields(data2, RoundUp);
        ES3.Save("AttackFeedback", data2, SaveManager.SaveFileName);
        
        var data3 = dataDict["DeadProtection"] as PluginData.DeadProtection;
        Traverse.IterateFields(data3, RoundUp);
        ES3.Save("DeadProtection", data3, SaveManager.SaveFileName);
    }

    private static void RoundUp(Traverse traverse) {
        if (traverse.GetValue() is float value) {
            traverse.SetValue(MathF.Round(value, 2));
        }
    }
    
    private static void LoadDefaults() {
        ES3.Save("BattleImprove", new PluginData.Version(), SaveFileName);
        ES3.Save("AttackFeedback", new PluginData.AttackFeedback(), SaveFileName);
        ES3.Save("DeadProtection", new PluginData.DeadProtection(), SaveFileName);
        LoadAll();
    }
}