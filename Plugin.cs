using System;
using System.Collections;
using System.Reflection;
using BattleImprove.Patcher;
using BattleImprove.Patcher.BattleFeedback;
using BattleImprove.Patcher.QOL;
using BattleImprove.Patcher.TakeHitPatcher;
using BattleImprove.Utils;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace BattleImprove;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;
    internal static Plugin instance;
    internal static LocalizationManager i18n;
    internal static bool needUpdate => UpdateChecker.CheckForUpdate();
    internal static bool firstLaunch;

    private bool debugMode = false;

    public void Awake() {
        instance = this;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading!");
        
#if DEBUG
        debugMode = true;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} debug mode is enable!");
#endif
        
        // Config
        this.LoggingInfo("Loading config...", true);
        BattleImprove.Config.InitConifg(this.Config);
        this.LoggingInfo("Config is loaded!", true);

        // Harmony patching
        this.LoggingInfo("Start patching...", true);
        Patching();
        this.LoggingInfo("Patching is done!", true);
        
        // AssetBundle
        Prefab.LoadAssetBundle();
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Start() {
        this.LoggingInfo("Plugin is starting...");
        StartCoroutine(Init());
    }

    private void OnDestroy() {
        Harmony.UnpatchAll();
    }

    private IEnumerator Init() {
        this.LoggingInfo("Waiting for game boost...", true);
        yield return new WaitForSeconds(10);
        this.LoggingInfo("Start Init!", true);
        StaticInstance.InitGameObject();
    }

    private void Patching() {
        Harmony.CreateAndPatchAll(typeof(StaticInstance));
        this.LoggingInfo("StaticInstance is loaded!", true);
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) Harmony.CreateAndPatchAll(typeof(ExpSharePatch));
        this.LoggingInfo("ExpSharePatch is loaded!", true);
        
        if (BattleImprove.Config.EnableHealthBar.Value) Harmony.CreateAndPatchAll(typeof(HealthBarPatch));
        this.LoggingInfo("HealthBarPatch is loaded!", true);
        
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) Harmony.CreateAndPatchAll(typeof(SoundPatch));
        this.LoggingInfo("SoundPatch is loaded!", true);
        
        // if (BattleImprove.Config.EnableDeadUnitCollision.Value) Harmony.CreateAndPatchAll(typeof(DeadUnitCollisionPatch));
        // this.Print("DeadUnitCollisionPatch is loaded!", true);

        if (BattleImprove.Config.EnableDamageMessage.Value) {
            Harmony.CreateAndPatchAll(typeof(DamageInfoPatch));
            this.LoggingInfo("DamageInfoPatch is loaded!", true);
        
            Harmony.CreateAndPatchAll(typeof(KillMessagePatch));
            this.LoggingInfo("KillMessagePatch is loaded!", true);
        }

        if (BattleImprove.Config.EnableXCrossHair.Value) {
            Harmony.CreateAndPatchAll(typeof(CrossHairPatch));
            this.LoggingInfo("CrossHairPatch is loaded!", true);
        }
        
        Harmony.CreateAndPatchAll(typeof(LootParticlePatch));
    }
    
    public void LoggingInfo(string info, bool needDebug = false) {
        switch (needDebug) {
            case true when debugMode:
                Logger.LogInfo("Debug info: " + info);
                break;
            case false:
                Logger.LogInfo(info);
                break;
        }
    }
}