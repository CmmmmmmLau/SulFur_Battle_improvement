using System;
using BattleImprove.PluginData;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.DevTools;
using PerfectRandom.Sulfur.Core.Units;

namespace BattleImprove.Patches.QOL;

public class HealthBar {
    private static Hook hook;
    private static Hook hook2;
    
    public static void Load() {
        if (MiscData.Instance.healthBarEnable) {
            if (hook == null) {
                hook = new Hook(AccessTools.Method(typeof(Npc), "Start"), AddDebugFrame);
                hook2 = new Hook(AccessTools.Method(typeof(UnitDebugFrame), "Update"), UpdateFrame);
            } else {
                hook.Apply();
                hook2.Apply();
            }
        }
    }

    public static void Unload() {
        hook?.Undo();
        hook2?.Undo();
    }
    
    private static void AddDebugFrame(Action<Npc> orig, Npc self) {
        orig?.Invoke(self);
        
        if (self.IsProtectedNpc) {
            return;
        }

        self.gameObject.AddComponent<MonoBehavior.Component.HealthBar>();
    }

    private static void UpdateFrame(Action<UnitDebugFrame> orig, UnitDebugFrame self) {
        if (StaticInstance<DevToolsManager>.Instance.shouldShow) {
            orig?.Invoke(self);
            return;
        };

        var bar = self.owner.GetComponent<MonoBehavior.Component.HealthBar>();
        if (self.owner.IsAlive) {
            self.transform.LookAt(StaticInstance<GameManager>.Instance.currentCamera.transform);
            bar = self.owner.GetComponent<MonoBehavior.Component.HealthBar>();
            if (bar.UpdateValue()) {
                AccessTools.Method(typeof(UnitDebugFrame), "UpdateValues").Invoke(self, null);
            }
        } else {
            bar.enabled = false;
            self.gameObject.SetActive(false);
        }
    }
}