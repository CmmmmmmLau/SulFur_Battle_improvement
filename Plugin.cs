using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace ExpShare;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;
    internal static AssetBundle AssetBundle;

    internal static ConfigEntry<float> Proportion;
    internal static ConfigEntry<bool> ShowInfo;

    private void Awake() {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading!");

        // Harmony patching
        Harmony.CreateAndPatchAll(typeof(StaticInstance));
        Harmony.CreateAndPatchAll(typeof(ExpSharePatch));
        Harmony.CreateAndPatchAll(typeof(HPBarPatch));
        Harmony.CreateAndPatchAll(typeof(HitSoundPatch));
        Harmony.CreateAndPatchAll(typeof(xCrossHairPatch));

        // Config
        Proportion = Config.Bind("General", "Proportion", 0.1f,
            "The proportion of the exp that another weapon will received");
        ShowInfo = Config.Bind("General", "ShowInfo", true, "Print the experience share info");
        
        // Load asset bundle
        string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "battle_improve"));
        if (AssetBundle == null) {
            Logger.LogError("Failed to load custom assets.");
            return;
        }

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
    
    public class StaticInstance {
        internal static GameObject PluginInstance;
        internal static Npc[] Enemies;
        internal static Unit[] DiedEnemies;
        internal static HitSoundEffect HitSoundClips;
        internal static xCrossHair CrossHair;
        internal static KillMessage KillMessage;
        
        
        [HarmonyPostfix, HarmonyPriority(Priority.First), HarmonyPatch(typeof(GameManager), "Start")]
        private static void AddBattleImprove() { ;
            PluginInstance = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BattleImprove"));
            HitSoundClips = PluginInstance.GetComponentInChildren<HitSoundEffect>();
            CrossHair = PluginInstance.GetComponentInChildren<xCrossHair>();
            KillMessage = PluginInstance.GetComponentInChildren<KillMessage>();
        }
        
        [HarmonyPrefix, HarmonyPriority(Priority.First), HarmonyPatch(typeof(InputReader), "LoadingContinue")]
        private static void AddFrame() {
            Enemies = StaticInstance<UnitManager>.Instance.GetAllEnemies();
        }
    }
}