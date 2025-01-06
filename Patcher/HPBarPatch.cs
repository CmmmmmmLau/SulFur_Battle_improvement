using System;
using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.DevTools;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ExpShare;

public class HPBarPatch {
    [HarmonyPostfix, HarmonyPatch(typeof(InputReader), "LoadingContinue")]
    private static void AddFrame() {
        var enemies = StaticInstance<UnitManager>.Instance.GetAllEnemies();
        foreach (var enemy in enemies) {
            if (enemy.IsProtectedNpc || enemy.isPlayer) continue;
            StaticInstance<DevToolsManager>.Instance.AddDebugFrameToUnit(enemy);
            enemy.gameObject.AddComponent<VisibleChecker>();
            enemy.debugFrame.gameObject.SetActive(false);
        }
    }
    
    [HarmonyPrefix, HarmonyPatch(typeof(UnitDebugFrame), "Update")]
    private static bool UpdateHPBar(UnitDebugFrame __instance) {
        __instance.transform.LookAt(StaticInstance<GameManager>.Instance.currentCamera.transform);
        var timer = __instance.owner.GetComponent<VisibleChecker>();
        if (timer.UpdateValue()) Traverse.Create(__instance).Method("UpdateValues").GetValue();

        if (__instance.owner.IsAlive) return false;
        timer.enabled = false;
        __instance.gameObject.SetActive(false);

        return false;
    }
    
    private class VisibleChecker : MonoBehaviour {
        private float timer = 0;
        private Npc npc;
        private Camera camera;


        private void Start() {
            this.npc = this.gameObject.GetComponent<Npc>();
            this.camera = StaticInstance<GameManager>.Instance.currentCamera;
        }

        private void Update() {
            if (!npc.IsAlive) return;
            var position = npc.EyesPosition;
            this.timer += Time.unscaledTime;

            if (CheckVisible(position)) {
                this.npc.debugFrame.gameObject.SetActive(true);
            } else {
                this.npc.debugFrame.gameObject.SetActive(false);
            }
        }
        
        private bool CheckVisible(Vector3 position) {
            try {
                Vector3 screenPoint = this.camera.WorldToViewportPoint(position);
                return screenPoint is {z: > 0, x: > 0 and < 1, y: > 0 and < 1} && Vector3.Distance(position, this.camera.transform.position) < 15;
            } catch {
                Plugin.Logger.LogInfo("Player camera lost, trying to find it again");
                this.camera = StaticInstance<GameManager>.Instance.currentCamera;
                StaticInstance<DevToolsManager>.Instance.AddDebugFrameToUnit(this.npc);
            }

            return false;
        }
        
        public bool UpdateValue() {
            if (!(timer > 0.25f)) return false;
            this.timer = 0;
            return true;
        }
    }
}