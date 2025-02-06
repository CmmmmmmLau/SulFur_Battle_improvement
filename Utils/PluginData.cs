using System;
using System.Collections.Generic;
using PerfectRandom.Sulfur.Core;
using UnityEngine;

namespace BattleImprove;

[Serializable]
public class PluginData {
    [NonSerialized]
    internal static Dictionary<string, PluginData> DataDict;
    
    public static bool SetupData() {
        if (SulfurSave.KeyExists("CmPlugin")) {
            DataDict = SulfurSave.Load("CmPlugin", new Dictionary<string, PluginData>());
            return false;
        } else {
            Plugin.instance.Print("Save data not found, creating new one...", true);
            LoadDefaults();
            Plugin.instance.Print("Save data created!", true);
        }
        return false;
    }
    
    private static void LoadDefaults() {
        DataDict = new Dictionary<string, PluginData> {
            {"BattleImprove", new VersionData()},
            {"AttackFeedback", new AttackFeedbackData()}
        };
        SaveData();
    }
    
    private static void VerifyData() {
        if (!DataDict.ContainsKey("BattleImprove")) {
            DataDict.Add("BattleImprove", new VersionData());
        }
        if (!DataDict.ContainsKey("AttackFeedback")) {
            DataDict.Add("AttackFeedback", new AttackFeedbackData());
        }
    }
    
    public static void SaveData() {
        SulfurSave.Save("CmPlugin", DataDict);
    }
    
    public class VersionData : PluginData{
        public string version;
        
        public VersionData() {
            this.version = MyPluginInfo.PLUGIN_VERSION;
        }
    }
    
    public class AttackFeedbackData : PluginData {
        public float indicatorVolume = 0.5f;
        public float indicatorDistance = 0.5f;
        public float indicatorDistanceFar = 0.5f;
        public float indicatorDistanceHeadShoot = 0.5f;
        public Color hitColor = Color.white;
        public Color killColor = Color.red;
        public int messageStyle = 0;
        public float messageVolume = 0.5f;
    }
}