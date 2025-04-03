#region

using System.Collections;
using BattleImprove.Components;
using BattleImprove.Components.QOL;
using BattleImprove.Patcher.QOL;
using BattleImprove.Patcher.TakeHitPatcher;
using BattleImprove.Transpiler;
using BattleImprove.Utils;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace BattleImprove;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;
    internal static bool NeedUpdate => UpdateChecker.CheckForUpdate();
    internal static Harmony Harmony;
    internal static LocalizationManager i18n;

    private static bool debugMode = false;
    
    internal static GameObject PluginGameObject;
    internal static GameObject IndicatorGameObject;

    public void Awake() {
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION} is loading!");
        
#if DEBUG
        debugMode = true;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} debug mode is enable!");
#endif
        // Config
        BattleImprove.Config.InitConfig(Config);
        // Harmony patching
        Patching();
        // AssetBundle
        StartCoroutine(PrefabManager.LoadAssetBundle());
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        StartCoroutine(InitPluginGameobject());
    }

    private IEnumerator InitPluginGameobject() {
        while (SulfurSave.saveSettings == null && !PrefabManager.IsLoaded) {
            yield return new WaitForSeconds(1f);
        }
        LoggingInfo("Starting plugin initialization...");
        
        // Localization
        LoggingInfo("Loading localization...");
        i18n = new LocalizationManager();
        i18n.LoadLocalization(Application.systemLanguage);
        
        LoggingInfo("Initializing plugin gameobject...");
        // Plugin GameObject
        var plugin = GameObject.Find("CmPlugin");
        if (plugin == null) {
            PluginGameObject = new GameObject("CmPlugin");
            Object.DontDestroyOnLoad(PluginGameObject);
        } else {
            PluginGameObject = plugin;
        }
        
        // load plugin save data
        LoggingInfo("Loading plugin data...");
        DataManager.SetUpData();
        // load indicator prefab
        
        LoggingInfo("Loading plugin prefab...");
        PrefabManager.LoadAttackFeedbackPrefab();
        
        // load other gameobject
        LoggingInfo("Loading other gameobject...");
        PluginGameObject.AddComponent<LootSpawnHelper>();
        var menu = new GameObject("Menu") {
            transform = {
                parent = PluginGameObject.transform
            }
        };
        menu.AddComponent<MenuController>();
    }

    private void OnDestroy() {
        Harmony.UnpatchSelf();
    }
    

    private void Patching() {
        LoggingInfo("Patching...", true);
        Harmony = Harmony.CreateAndPatchAll(typeof(AttackFeedbackPatch));
        Harmony.PatchAll(typeof(DataManager));
        
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) Harmony.PatchAll(typeof(ExpSharePatch));
        if (BattleImprove.Config.EnableHealthBar.Value) Harmony.PatchAll(typeof(HealthBarPatch));
        if (BattleImprove.Config.EnableLoopDropVFX.Value) Harmony.PatchAll(typeof(LootDropPatch));
        if (BattleImprove.Config.EnableDeadUnitCollision.Value) Harmony.PatchAll(typeof(RemoveDeadBodyCollisionTranspiler));
        if (BattleImprove.Config.EnableDeadProtection.Value) Harmony.PatchAll(typeof(DeadProtection));

        // Other
        if (BattleImprove.Config.ReverseMouseScroll.Value) Harmony.PatchAll(typeof(MouseScrollTranspiler));
        
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) Harmony.PatchAll(typeof(SoundPatch));
        if (BattleImprove.Config.EnableDamageMessage.Value) {
            Harmony.PatchAll(typeof(DamageInfoPatch));
            Harmony.PatchAll(typeof(KillMessagePatch));
        }
        if (BattleImprove.Config.EnableXCrossHair.Value) Harmony.PatchAll(typeof(CrossHairPatch));
        
        LoggingInfo("Patching complete!");
    }
    
    public static void LoggingInfo(string info, bool needDebug = false) {
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