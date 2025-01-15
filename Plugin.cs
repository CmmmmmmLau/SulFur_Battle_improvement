using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BattleImprove.Patcher.BattleFeedback;
using BattleImprove.Patcher.QOL;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;
    internal static AssetBundle AssetBundle;

    private void Awake() {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading!");
        
        // Config
        BattleImprove.Config.InitConifg(this.Config);

        // Harmony patching
        Patching();
        
        // Load asset bundle
        // var sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        // AssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "battle_improve"));
        // if (AssetBundle == null) {
        //     Logger.LogError("Failed to load custom assets.");
        //     return;
        // }
        AssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("BattleImprove.Assets.battle_improve"));


        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Patching() {
        Harmony.CreateAndPatchAll(typeof(StaticInstance));
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) Harmony.CreateAndPatchAll(typeof(ExpSharePatch));
        if (BattleImprove.Config.EnableHealthBar.Value) Harmony.CreateAndPatchAll(typeof(HealthBarPatch));
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) Harmony.CreateAndPatchAll(typeof(SoundPatch));
        if (BattleImprove.Config.EnableDeadUnitCollision.Value) Harmony.CreateAndPatchAll(typeof(DeadUnitCollisionPatch));
        Harmony.CreateAndPatchAll(typeof(AttackFeedbackPatch));
    }

    public class StaticInstance {
        internal static GameObject PluginInstance;
        internal static Npc[] Enemies;
        internal static List<Unit> KilledEnemies;
        internal static HitSoundEffect HitSoundClips;
        internal static xCrossHair CrossHair;
        internal static KillMessage KillMessage;
        internal static DamageInfo DamageInfo;


        [HarmonyPostfix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(GameManager), "Start")]
        private static void AddBattleImprove() {
            PluginInstance = Instantiate(AssetBundle.LoadAsset<GameObject>("BattleImprove"));
            HitSoundClips = PluginInstance.GetComponentInChildren<HitSoundEffect>();
            CrossHair = PluginInstance.GetComponentInChildren<xCrossHair>();
            KillMessage = PluginInstance.GetComponentInChildren<KillMessage>();
            DamageInfo = PluginInstance.GetComponentInChildren<DamageInfo>();
        }

        [HarmonyPrefix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(InputReader), "LoadingContinue")]
        private static void AddFrame() {
            Enemies = StaticInstance<UnitManager>.Instance.GetAllEnemies();
            KilledEnemies = new List<Unit>();
        }
    }
}