using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BattleImprove.Utils;

public static class PrefabManager {
    internal static AssetBundle AssetBundle;
    public static Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();
    public static bool IsLoaded;

    public static IEnumerator LoadAssetBundle() {
        IsLoaded = false;
        Plugin.LoggingInfo("Loading Asset Bundle...", true);
        // Load asset bundle

#if DEBUG
        var sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "battle_improve"));
#else
        AssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("BattleImprove.Assets.battle_improve"));
#endif
        if (AssetBundle == null) {
            Plugin.LoggingInfo("Failed to load custom assets.");
        } else {
            var request = AssetBundle.LoadAssetAsync<GameObject>("AttackFeedback");
            yield return request;
            Prefabs.Add("AttackFeedback", request.asset as GameObject);

            foreach (var style in DataManager.KillMessageStyle.Values) {
                request = AssetBundle.LoadAssetAsync<GameObject>(style);
                yield return request;
                Prefabs.Add(style, request.asset as GameObject);
            }

            for (var i = 1; i <= 5; i++) {
                request = AssetBundle.LoadAssetAsync<GameObject>("LoopDropTier" + i);
                yield return request;
                Prefabs.Add("LoopDropTier" + i, request.asset as GameObject);
            }
        }
        
        Plugin.LoggingInfo("Asset Bundle loaded!", true);
        IsLoaded = true;
    }
    
    public static GameObject LoadPrefab(string name, GameObject parent) {
        Plugin.LoggingInfo($"Loading {name} Prefab...", true);
        return Object.Instantiate(Prefabs[name], parent.transform, true);
    }
    
    internal static void LoadAttackFeedbackPrefab() {
        Plugin.IndicatorGameObject = LoadPrefab("AttackFeedback", Plugin.PluginGameObject);
        LoadKillMessageStyle(DataManager.KillMessageStyle[DataManager.AttackFeedbackData.messageStyle]);
    }
    
    internal static void LoadKillMessageStyle(string style = "Battlefield 1") {
        if (PluginInstance<MessageController>.Instance != null) {
            Object.Destroy(PluginInstance<MessageController>.Instance.gameObject);
        }
        LoadPrefab(style, Plugin.IndicatorGameObject);
    }
}