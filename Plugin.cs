#region

using System.Collections;
using BattleImprove.Patcher;
using BattleImprove.Patcher.BattleFeedback;
using BattleImprove.Patcher.QOL;
using BattleImprove.Patcher.TakeHitPatcher;
using BattleImprove.Utils;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

#endregion

namespace BattleImprove;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;
    internal static Plugin instance;
    internal static LocalizationManager i18n;
    internal static bool needUpdate => UpdateChecker.CheckForUpdate();
    internal static bool firstLaunch;
    internal static Harmony harmony;

    private bool debugMode;

    public void Awake() {
        instance = this;
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading!");
        
#if DEBUG
        debugMode = true;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} debug mode is enable!");
#endif
        
        // Config
        LoggingInfo("Loading config...", true);
        BattleImprove.Config.InitConifg(Config);
        LoggingInfo("Config is loaded!", true);

        // Harmony patching
        LoggingInfo("Start patching...", true);
        Patching();
        LoggingInfo("Patching is done!", true);
        
        // AssetBundle
        StartCoroutine(Prefab.LoadAssetBundle());
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Start() {
        LoggingInfo("Plugin is starting...");
        StartCoroutine(Init());
    }

    private void OnDestroy() {
        harmony.UnpatchSelf();
    }

    private IEnumerator Init() {
        LoggingInfo("Waiting for game boost...", true);
        yield return new WaitForSeconds(10);
        LoggingInfo("Start Init!", true);
        StaticInstance.InitGameObject();
    }

    private void Patching() {
        harmony = Harmony.CreateAndPatchAll(typeof(StaticInstance));
        LoggingInfo("StaticInstance is loaded!", true);
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) harmony.PatchAll(typeof(ExpSharePatch));
        LoggingInfo("ExpSharePatch is loaded!", true);
        
        if (BattleImprove.Config.EnableHealthBar.Value) harmony.PatchAll(typeof(HealthBarPatch));
        LoggingInfo("HealthBarPatch is loaded!", true);
        
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) harmony.PatchAll(typeof(SoundPatch));
        LoggingInfo("SoundPatch is loaded!", true);
        
        // if (BattleImprove.Config.EnableDeadUnitCollision.Value) Harmony.CreateAndPatchAll(typeof(DeadUnitCollisionPatch));
        // this.Print("DeadUnitCollisionPatch is loaded!", true);

        if (BattleImprove.Config.EnableDamageMessage.Value) {
            harmony.PatchAll(typeof(DamageInfoPatch));
            LoggingInfo("DamageInfoPatch is loaded!", true);
        
            harmony.PatchAll(typeof(KillMessagePatch));
            LoggingInfo("KillMessagePatch is loaded!", true);
        }

        if (BattleImprove.Config.EnableXCrossHair.Value) {
            harmony.PatchAll(typeof(CrossHairPatch));
            LoggingInfo("CrossHairPatch is loaded!", true);
        }
        
        harmony.PatchAll(typeof(LootParticlePatch));
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