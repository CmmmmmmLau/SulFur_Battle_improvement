using HarmonyLib;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.DevTools;
using PerfectRandom.Sulfur.Core.Input;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace ExpShare.Patcher;

public class HPBarPatch {
    [HarmonyPostfix, HarmonyPatch(typeof(InputReader), "LoadingContinue")]
    private static void AddFrame() {
        foreach (var enemy in Plugin.StaticInstance.Enemies) {
            if (enemy.IsProtectedNpc || enemy.isPlayer) continue;
            StaticInstance<DevToolsManager>.Instance.AddDebugFrameToUnit(enemy);
            enemy.gameObject.AddComponent<VisibleChecker>();
            enemy.debugFrame.gameObject.SetActive(false);
        }
    }
    
    // [HarmonyPostfix, HarmonyPatch(typeof(UnitManager), "AddUnit")]
    // private static void AddDebugFrame(Unit unit) {
    //     StaticInstance<DevToolsManager>.Instance.AddDebugFrameToUnit(unit);
    //     unit.gameObject.AddComponent<VisibleChecker>();
    //     unit.debugFrame.gameObject.SetActive(false);
    // }
    
    [HarmonyPrefix, HarmonyPatch(typeof(UnitDebugFrame), "Update")]
    private static bool UpdateHPBar(UnitDebugFrame __instance) {
        if (StaticInstance<DevToolsManager>.Instance.shouldShow) return true;

        __instance.transform.LookAt(StaticInstance<GameManager>.Instance.currentCamera.transform);
        var timer = __instance.owner.GetComponent<VisibleChecker>();
        if (timer.UpdateValue()) Traverse.Create(__instance).Method("UpdateValues").GetValue();

        if (__instance.owner.IsAlive) return false;
        timer.enabled = false;
        __instance.gameObject.SetActive(false);

        return false;
    }

    private class VisibleChecker : MonoBehaviour {
        private Camera camera;
        private Npc npc;
        private float timer;


        private void Start() {
            npc = gameObject.GetComponent<Npc>();
            camera = StaticInstance<GameManager>.Instance.currentCamera;
        }

        private void Update() {
            if (!npc.IsAlive) return;
            var position = npc.EyesPosition;
            timer += Time.unscaledTime;

            if (CheckVisible(position))
                npc.debugFrame.gameObject.SetActive(true);
            else
                npc.debugFrame.gameObject.SetActive(false);
        }

        private bool CheckVisible(Vector3 position) {
            try {
                var screenPoint = camera.WorldToViewportPoint(position);
                return screenPoint is {z: > 0, x: > 0 and < 1, y: > 0 and < 1} &&
                       Vector3.Distance(position, camera.transform.position) < 15;
            }
            catch {
                Plugin.Logger.LogInfo("Player camera lost, trying to find it again");
                camera = StaticInstance<GameManager>.Instance.currentCamera;
                StaticInstance<DevToolsManager>.Instance.AddDebugFrameToUnit(npc);
            }

            return false;
        }

        public bool UpdateValue() {
            if (!(timer > 0.25f)) return false;
            timer = 0;
            return true;
        }
    }
}