using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace ExpShare;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    
    internal static ConfigEntry<float> Proportion;
    internal static ConfigEntry<bool> ShowInfo;

    private void Awake() {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loading!");

        // Harmony patching
        Harmony.CreateAndPatchAll(typeof(ExpSharePatch));
        Harmony.CreateAndPatchAll(typeof(HPBarPatch));
        // Harmony.CreateAndPatchAll(typeof(DeadUnitCollisionPatch));
        
        // Config
        Proportion = this.Config.Bind("General", "Proportion", 0.1f, "The proportion of the exp that another weapon will received");
        ShowInfo = this.Config.Bind("General", "ShowInfo", true, "Print the experience share info");
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}