#region

using System.Collections;
using BattleImprove.Patcher;
using BattleImprove.Patcher.QOL;
using BattleImprove.Patcher.TakeHitPatcher;
using BattleImprove.Transpiler;
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

    private bool debugMode = false;

    public void Awake() {
        instance = this;
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION} is loading!");
        
#if DEBUG
        debugMode = true;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} debug mode is enable!");
#endif
        
        // Config
        BattleImprove.Config.InitConifg(Config);

        // Harmony patching
        Patching();
        
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
        
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) harmony.PatchAll(typeof(ExpSharePatch));
        if (BattleImprove.Config.EnableHealthBar.Value) harmony.PatchAll(typeof(HealthBarPatch));
        if (BattleImprove.Config.EnableLoopDropVFX.Value) harmony.PatchAll(typeof(LootDropPatch));
        if (BattleImprove.Config.EnableDeadUnitCollision.Value) harmony.PatchAll(typeof(RemoveDeadBodyCollisionTranspiler));
        if (BattleImprove.Config.EnableDeadProtection.Value) harmony.PatchAll(typeof(DeadProtection));

        // Other
        if (BattleImprove.Config.ReverseMouseScroll.Value) harmony.PatchAll(typeof(MouseScrollTranspiler));
        
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) harmony.PatchAll(typeof(SoundPatch));
        if (BattleImprove.Config.EnableDamageMessage.Value) {
            harmony.PatchAll(typeof(DamageInfoPatch));
            harmony.PatchAll(typeof(KillMessagePatch));
        }
        if (BattleImprove.Config.EnableXCrossHair.Value) harmony.PatchAll(typeof(CrossHairPatch));
        
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