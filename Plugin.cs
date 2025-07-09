using System;
using System.Linq;
using BattleImprove.MonoBehavior;
using BattleImprove.PluginData;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MenuLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;
using UnityEngine.UI;

namespace BattleImprove;

[BepInPlugin(MOD_GUID, MOD_Name, MOD_VERSION)]
[BepInDependency(MenuLib.Plugin.MOD_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin {
    public static GameObject PluginObject;
    
    internal new static ManualLogSource Logger;
    
    public const string MOD_GUID = "cmmmmmm.battleimprove";
    public const string MOD_Name = "BattleImprove";
    public const string MOD_VERSION = "2.0.0";

    public void Awake() {
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION} is loading!");
        
        PluginObject = new GameObject("BattleImprove");
        PluginObject.hideFlags = HideFlags.HideAndDontSave;
        PluginObject.AddComponent<LootSpawnHelper>();
        PluginObject.AddComponent<CrossHairController>();
        PluginObject.AddComponent<ImpactSoundController>();
        
        InitCanvas();
        
        Patches.Entry.Load();
        PrefabReference.Load();
    }

    private void Start() {
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(MenuLib.Plugin.MOD_GUID)) {
            Logger.LogInfo("MenuLib is found, configuration features will be available.");
            RegisterMenu();
        } else {
            Logger.LogWarning("MenuLib is not found, configuration features features will not be available.");
        }
    }

    private void InitCanvas() {
        var canvas = new GameObject("CrossHairCanvas").AddComponent<Canvas>();
        canvas.transform.SetParent(PluginObject.transform, false);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        
        CrossHairController.canvas = canvas.gameObject;
    }

    private static void RegisterMenu() {
        MenuAPI.AddNewCategory("BattleImprove", parent => {
            MenuAPI.CreateBar("Crosshair", parent);
            MenuAPI.CreateCheckBox("Enable", CrossHairData.Instance.enable, true, parent, value => {
                CrossHairData.Instance.enable = value;
                if (value) {
                    Patches.CrossHair.Load();
                } else {
                    Patches.CrossHair.Unload();
                }
                CrossHairData.Save();
            });
            MenuAPI.CreateDropDown("Style", PrefabReference.crossHair.Keys.ToList(), CrossHairData.Instance.style, "BF1", parent, value => {
                CrossHairData.Instance.style = value;
                CrossHairData.Save();
                CrossHairController.Instance.ChangeStyle(value);
            });
            MenuAPI.CreateColorPicker("CrossHair color on hit", CrossHairData.Instance.hitColor, Color.white, parent, value => {
                CrossHairData.Instance.hitColor = value;
                CrossHairData.Save();
            });
            
            MenuAPI.CreateColorPicker("CrossHair color on kill", CrossHairData.Instance.killColor, Color.red, parent, value => {
                CrossHairData.Instance.killColor = value;
                CrossHairData.Save();
            });
            
            
            
            MenuAPI.CreateBar("Impact Sound", parent);
            MenuAPI.CreateCheckBox("Enable", ImpactSoundData.Instance.enable, true, parent, value => {
                ImpactSoundData.Instance.enable = value;
                if (value) {
                    Patches.ImpactSound.Load();
                } else {
                    Patches.ImpactSound.Unload();
                }
                ImpactSoundData.Save();
            });
            MenuAPI.CreateDropDown("Style", PrefabReference.impactSound.Keys.ToList(), ImpactSoundData.Instance.style, "BF1", parent, value => {
                ImpactSoundData.Instance.style = value;
                ImpactSoundData.Save();
                ImpactSoundController.Instance.ChangeStyle(value);
            });
            MenuAPI.CreateFloatSliderField("Distance", ImpactSoundData.Instance.volume, 1f, 0f, 1f, parent, value => {
                ImpactSoundData.Instance.volume = value;
                ImpactSoundData.Save();
            });
            MenuAPI.CreateFloatSliderField("Distance - Far", ImpactSoundData.Instance.volumeFar, 1f, 0f, 1f, parent, value => {
                ImpactSoundData.Instance.volumeFar = value;
                ImpactSoundData.Save();
            });
            MenuAPI.CreateFloatSliderField("Distance - Critical", ImpactSoundData.Instance.volumeCrit, 1f, 0f, 1f, parent, value => {
                ImpactSoundData.Instance.volumeCrit = value;
                ImpactSoundData.Save();
            });
                        
            
            
            MenuAPI.CreateBar("Dead Protection", parent);
            MenuAPI.CreateCheckBox("Enable", DeadProtectionData.Instance.enable, true, parent, value => {
                DeadProtectionData.Instance.enable = value;
                if (value) {
                    Patches.QOL.DeadProtection.Load();
                } else {
                    Patches.QOL.DeadProtection.Unload();
                }
                DeadProtectionData.Save();
            });
            MenuAPI.CreateFloatSliderField("Weapon Durability", DeadProtectionData.Instance.weaponDurability, 0.3f, 0f, 1f, parent, value => {
                DeadProtectionData.Instance.weaponDurability = value;
                DeadProtectionData.Save();
            });
            MenuAPI.CreateFloatSliderField("Attachment Chance", DeadProtectionData.Instance.attachmentChance, 0.3f, 0f, 1f, parent, value => {
                DeadProtectionData.Instance.attachmentChance = value;
                DeadProtectionData.Save();
            });
            MenuAPI.CreateFloatSliderField("Enchantment Chance", DeadProtectionData.Instance.enchantmentChance, 0.3f, 0f, 1f, parent, value => {
                DeadProtectionData.Instance.enchantmentChance = value;
                DeadProtectionData.Save();
            });
            MenuAPI.CreateFloatSliderField("Barrel Chance", DeadProtectionData.Instance.barrelChance, 0.3f, 0f, 1f, parent, value => {
                DeadProtectionData.Instance.barrelChance = value;
                DeadProtectionData.Save();
            });
            MenuAPI.CreateBar("Loop Drop Effect", parent);
            MenuAPI.CreateCheckBox("Enable", MiscData.Instance.loopDropEnable, true, parent, value => {
                MiscData.Instance.loopDropEnable = value;
                MiscData.Save();
            });
            
            
            
            MenuAPI.CreateBar("Health Bar", parent);
            MenuAPI.CreateCheckBox("Enable", MiscData.Instance.healthBarEnable, true, parent, value => {
                MiscData.Instance.healthBarEnable = value;
                if (value) {
                    Patches.QOL.HealthBar.Load();
                } else {
                    Patches.QOL.HealthBar.Unload();
                }
                MiscData.Save();
            });
            
            
            
            MenuAPI.CreateBar("Experience Share", parent);
            MenuAPI.CreateCheckBox("Enable", MiscData.Instance.expShareEnable, true, parent, value => {
                MiscData.Instance.expShareEnable = value;
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