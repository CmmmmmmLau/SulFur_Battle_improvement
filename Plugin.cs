using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BattleImprove.Components;
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
    internal static Plugin instance;
    
    private bool isInitialized = false;
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
        this.Print("Loading config...", true);
        BattleImprove.Config.InitConifg(this.Config);
        this.Print("Config is loaded!", true);

        // Harmony patching
        this.Print("Start patching...", true);
        Patching();
        this.Print("Patching is done!", true);
        
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
        this.Print("Plugin is starting...");
        StartCoroutine(Init());
    }

    private void OnDestroy() {
        Harmony.UnpatchAll();
    }

    private IEnumerator Init() {
        while (!isInitialized) {
            this.Print("GameManager not found! Wait for 3 seconds...", true);
            yield return new WaitForSeconds(3);
            if (StaticInstance<GameManager>.Instance != null) {
                isInitialized = true;
                StaticInstance.AddBattleImprove();
                this.Print("GameManager found! Start Init!", true);
            }
        }
    }

    private void Patching() {
        Harmony.CreateAndPatchAll(typeof(StaticInstance));
        this.Print("StaticInstance is loaded!", true);
        // QOL
        if (BattleImprove.Config.EnableExpShare.Value) Harmony.CreateAndPatchAll(typeof(ExpSharePatch));
        this.Print("ExpSharePatch is loaded!", true);
        
        if (BattleImprove.Config.EnableHealthBar.Value) Harmony.CreateAndPatchAll(typeof(HealthBarPatch));
        this.Print("HealthBarPatch is loaded!", true);
        
        // BF
        if (BattleImprove.Config.EnableSoundFeedback.Value) Harmony.CreateAndPatchAll(typeof(SoundPatch));
        this.Print("SoundPatch is loaded!", true);
        
        if (BattleImprove.Config.EnableDeadUnitCollision.Value) Harmony.CreateAndPatchAll(typeof(DeadUnitCollisionPatch));
        this.Print("DeadUnitCollisionPatch is loaded!", true);
        
        Harmony.CreateAndPatchAll(typeof(AttackFeedbackPatch));
    }
    
    public void Print(string info, bool needDebug = false) {
        switch (needDebug) {
            case true when debugMode:
                Logger.LogInfo("Debug info: " + info);
                break;
            case false:
                Logger.LogInfo(info);
                break;
        }
    }

    public class StaticInstance {
        internal static GameObject PluginGameObject;
        internal static Npc[] Enemies;
        internal static List<Unit> KilledEnemies;
        internal static HitSoundEffect HitSoundClips;
        internal static xCrossHair CrossHair;
        internal static KillMessage KillMessage;
        internal static DamageInfo DamageInfo;
        
        public static void AddBattleImprove() {
            PluginGameObject = Instantiate(AssetBundle.LoadAsset<GameObject>("BattleImprove"));
            HitSoundClips = PluginGameObject.GetComponentInChildren<HitSoundEffect>();
            CrossHair = PluginGameObject.GetComponentInChildren<xCrossHair>();
            KillMessage = PluginGameObject.GetComponentInChildren<KillMessage>();
            DamageInfo = PluginGameObject.GetComponentInChildren<DamageInfo>();
            
            // GameObject manager = new GameObject("PluginManager");
            // manager.gameObject.transform.parent = PluginGameObject.transform;
            // manager.AddComponent<PluginManager>();
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