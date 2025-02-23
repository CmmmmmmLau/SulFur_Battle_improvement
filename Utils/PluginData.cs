using System;
using System.Collections.Generic;
using System.Linq;
using BattleImprove.Utils;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using UnityEngine;
using Application = UnityEngine.Device.Application;

namespace BattleImprove;

[Serializable]
public class PluginData {
    [NonSerialized]
    internal static Dictionary<string, PluginData> DataDict;
    
    public static bool SetupData() {
        try {
            if (SulfurSave.KeyExists("CmPlugin")) {
                DataDict = SulfurSave.Load("CmPlugin", new Dictionary<string, PluginData>());
                VerifyData();
                return false;
            } else {
                Plugin.instance.LoggingInfo("Save data not found, creating new one...", true);
                LoadDefaults();
                Plugin.instance.LoggingInfo("Save data created!", true);
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Plugin.instance.LoggingInfo("Failed to load save data, creating new one...", true);
            LoadDefaults();
            throw;
        }
        return true;
    }
    
    private static void LoadDefaults() {
        DataDict = new Dictionary<string, PluginData> {
            {"BattleImprove", new Version()},
            {"AttackFeedback", new AttackFeedback()},
            {"DeadProtection", new DeadProtection()}
        };
        SaveData();
    }
    
    private static void VerifyData() {
        if (!DataDict.ContainsKey("BattleImprove")) {
            DataDict.Add("BattleImprove", new Version());
        }
        if (!DataDict.ContainsKey("AttackFeedback")) {
            DataDict.Add("AttackFeedback", new AttackFeedback());
        }
        if (!DataDict.ContainsKey("DeadProtection")) {
            DataDict.Add("DeadProtection", new DeadProtection());
        }
    }
    
    private static void RemoveData() {
        DataDict = new Dictionary<string, PluginData>();
        SaveData();
    }
    
    public static void SaveData() {
        SulfurSave.Save("CmPlugin", DataDict);
    }
    
    public class Version : PluginData{
        public string version => MyPluginInfo.PLUGIN_VERSION;
        public KeyCode menuKey = KeyCode.F1;
    }
    
    public class AttackFeedback : PluginData {
        public float indicatorVolume = 0.5f;
        public float indicatorDistance = 0.5f;
        public float indicatorDistanceFar = 0.5f;
        public float indicatorDistanceHeadShoot = 0.5f;
        public Color hitColor = Color.white;
        public Color killColor = Color.red;
        public int messageStyle = 0;
        public float messageVolume = 0.5f;
    }
    
    public class DeadProtection : PluginData {
        public List<InventoryData> weapons = new ();
        public List<string> weaponModify = new ();
    }
}