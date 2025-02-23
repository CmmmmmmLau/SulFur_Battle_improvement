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
    internal static Npc[] Enemies;
    internal static List<Unit> KilledEnemies;
    internal static HitSoundEffect HitSoundClips;
    internal static xCrossHair CrossHair;
    internal static KillMessage KillMessage;
    internal static DamageInfo DamageInfo;
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
        LoadPrefab();
        Plugin.firstLaunch = PluginData.SetupData();

        LootSpawnHelper = PluginGameObject.AddComponent<LootSpawnHelper>();
            
        var menu = new GameObject("Menu");
        menu.transform.parent = PluginGameObject.transform;
        menu.AddComponent<MenuController>();
    }

    internal static void LoadPrefab() {
        IndicatorGameObject = Prefab.LoadPrefab("AttackFeedback", PluginGameObject);
        HitSoundClips = PluginGameObject.GetComponentInChildren<HitSoundEffect>();
        CrossHair = PluginGameObject.GetComponentInChildren<xCrossHair>();
        KillMessage = PluginGameObject.GetComponentInChildren<KillMessage>();
        DamageInfo = PluginGameObject.GetComponentInChildren<DamageInfo>();
    }

    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    [HarmonyPatch(typeof(InputReader), "LoadingContinue")]
    private static void AddFrame() {
        Enemies = StaticInstance<UnitManager>.Instance.GetAllEnemies();
        KilledEnemies = new List<Unit>();
    }
}