using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExpShare.Components;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.DevTools;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace ExpShare.Patcher;

public class HealthBarPatch {
    
    [HarmonyPrefix, HarmonyPatch(typeof(UnitDebugFrame), "Update")]
    private static bool UpdateHPBar(UnitDebugFrame __instance) {
        if (StaticInstance<DevToolsManager>.Instance.shouldShow) return true;
    
        __instance.transform.LookAt(StaticInstance<GameManager>.Instance.currentCamera.transform);
        var bar = __instance.owner.GetComponent<HealthBar>();
        if (bar.UpdateValue()) Traverse.Create(__instance).Method("UpdateValues").GetValue();
    
        if (__instance.owner.IsAlive) return false;
        
        bar.enabled = false;
        __instance.gameObject.SetActive(false);
    
        return false;
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(Npc), "Start")]
    private static void AddDebugFrame(Player __instance) {
        __instance.gameObject.AddComponent<HealthBar>();
    }
}