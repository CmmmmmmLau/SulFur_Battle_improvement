using System.Collections;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.DevTools;
using PerfectRandom.Sulfur.Core.Units;
using UnityEngine;

namespace BattleImprove.Components;

public class HealthBar : MonoBehaviour {
    private Camera camera;
    private bool isInitialized;
    private Npc npc;
    private float timer;

    private void Start() {
        npc = gameObject.GetComponent<Npc>();
        if (npc.IsProtectedNpc) {
            Destroy(this);
            return;
        }

        isInitialized = false;
        StartCoroutine(Initialize());
    }

    private void Update() {
        if (!isInitialized || !npc.IsAlive) return;
        var position = npc.EyesPosition;
        timer += Time.deltaTime;

        npc.debugFrame.gameObject.SetActive(CheckVisible(position));
    }

    private IEnumerator Initialize() {
        yield return new WaitForSeconds(3);
        StaticInstance<DevToolsManager>.Instance.AddDebugFrameToUnit(npc);
        camera = StaticInstance<GameManager>.Instance.currentCamera;
        isInitialized = true;
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