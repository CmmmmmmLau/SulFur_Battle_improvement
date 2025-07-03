using BattleImprove.MonoBehavior;
using BattleImprove.PluginData;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MenuLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove;

[BepInPlugin(MOD_GUID, MOD_Name, MOD_VERSION)]
[BepInDependency(MenuLib.Plugin.MOD_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin {
    internal new static ManualLogSource Logger;
    
    public const string MOD_GUID = "cmmmmmm.battleimprove";
    public const string MOD_Name = "BattleImprove";
    public const string MOD_VERSION = "2.0.0";

    public void Awake() {
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION} is loading!");

        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(MenuLib.Plugin.MOD_GUID)) {
            Logger.LogInfo("MenuLib is found, configuration features will be available.");
            RegisterMenu();
        } else {
            Logger.LogWarning("MenuLib is not found, configuration features features will not be available.");
        }

        this.gameObject.AddComponent<LootSpawnHelper>();
        
        Patches.Entry.Load();
    }

    private static void RegisterMenu() {
        MenuAPI.AddNewCategory("BattleImprove", parent => {
            MenuAPI.CreateBar("Experience Share", parent);
            MenuAPI.CreateCheckBox("Enable", MiscData.Instance.enable, true, parent, value => {
                MiscData.Instance.enable = value;
                if (value) {
                    Patches.QOL.ExpShare.Load();
                } else {
                    Patches.QOL.ExpShare.Unload();
                }
                MiscData.Save();
            });
            MenuAPI.CreateFloatSliderField("Proportion", MiscData.Instance.expShareProportion, 0.5f, 0f, 1f, parent, value => {
                MiscData.Instance.expShareProportion = value;
                MiscData.Save();
            });
        });
    }
}