using System;
using System.Collections;
using System.Reflection;
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
    internal static AssetBundle AssetBundle;
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
        this.Info("Loading config...", true);
        BattleImprove.Config.InitConifg(this.Config);
        this.Info("Config is loaded!", true);

        // Harmony patching
        this.Info("Start patching...", true);
        Patching();
        this.Info("Patching is done!", true);
        
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

    private void Start() {
        this.Info("Plugin is starting...");
        StartCoroutine(Init());
    }

    private void OnDestroy() {
        Harmony.UnpatchAll();
    }

    private IEnumerator Init() {
        this.Info("Waiting for game boost...", true);
        yield return new WaitForSeconds(10);
        this.Info("Start Init!", true);
        StaticInstance.InitGameObject();
    }

    private void Patching() {
        Harmony.CreateAndPatchAll(typeof(StaticInstance));
        this.Info("StaticInstance is loaded!", true);
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) Harmony.CreateAndPatchAll(typeof(ExpSharePatch));
        this.Info("ExpSharePatch is loaded!", true);
        
        if (BattleImprove.Config.EnableHealthBar.Value) Harmony.CreateAndPatchAll(typeof(HealthBarPatch));
        this.Info("HealthBarPatch is loaded!", true);
        
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) Harmony.CreateAndPatchAll(typeof(SoundPatch));
        this.Info("SoundPatch is loaded!", true);
        
        // if (BattleImprove.Config.EnableDeadUnitCollision.Value) Harmony.CreateAndPatchAll(typeof(DeadUnitCollisionPatch));
        // this.Print("DeadUnitCollisionPatch is loaded!", true);

        if (BattleImprove.Config.EnableDamageMessage.Value) {
            Harmony.CreateAndPatchAll(typeof(DamageInfoPatch));
            this.Info("DamageInfoPatch is loaded!", true);
        
            Harmony.CreateAndPatchAll(typeof(KillMessagePatch));
            this.Info("KillMessagePatch is loaded!", true);
        }

        if (BattleImprove.Config.EnableXCrossHair.Value) {
            Harmony.CreateAndPatchAll(typeof(CrossHairPatch));
            this.Info("CrossHairPatch is loaded!", true);
        }
    }
    
    public void Info(string info, bool needDebug = false) {
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