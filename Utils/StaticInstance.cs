using System.Collections.Generic;
using BattleImprove.Components;
using BattleImprove.Components.QOL;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Utils;

public class StaticInstance {
    internal static GameObject PluginGameObject;
    internal static GameObject IndicatorGameObject;
    internal static HitSoundEffect HitSoundClips;
    internal static xCrossHair CrossHair;
    internal static MessageController KillMessage;
    internal static LootSpawnHelper LootSpawnHelper;
        
    public static void InitGameObject() {
        var plugin = GameObject.Find("CmPlugin");
        if (plugin == null) {
            PluginGameObject = new GameObject("CmPlugin");
            Object.DontDestroyOnLoad(PluginGameObject);
        } else {
            PluginGameObject = plugin;
        }

        Plugin.i18n = new LocalizationManager();
        Plugin.i18n.LoadLocalization(Application.systemLanguage);
        
        Plugin.firstLaunch = PluginData.SetupData();
        
        LoadPrefab();

        LootSpawnHelper = PluginGameObject.AddComponent<LootSpawnHelper>();
            
        var menu = new GameObject("Menu");
        menu.transform.parent = PluginGameObject.transform;
        menu.AddComponent<MenuController>();
    }

    internal static void LoadPrefab() {
        IndicatorGameObject = Prefab.LoadPrefab("AttackFeedback", PluginGameObject);
        HitSoundClips = PluginGameObject.GetComponentInChildren<HitSoundEffect>();
        CrossHair = PluginGameObject.GetComponentInChildren<xCrossHair>();
        
        var temp = PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedback;
        LoadKillMessageStyle(PluginData.KillMessageStyle[temp.messageStyle]);
    }
    
    internal static void LoadKillMessageStyle(string style = "Battlefield 1") {
        if (KillMessage != null) {
            Object.Destroy(KillMessage.gameObject);
        }
        var gameObject = Prefab.LoadPrefab(style, IndicatorGameObject);
        KillMessage = gameObject.GetComponentInChildren<MessageController>();
    }
}