using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BattleImprove.Utils;

public class Prefab {
    internal static AssetBundle AssetBundle;
    public static Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();

    public static IEnumerator LoadAssetBundle() {
        Plugin.instance.LoggingInfo("Loading Asset Bundle...", true);
        // Load asset bundle

#if DEBUG
        var sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "battle_improve"));
#else
        AssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("BattleImprove.Assets.battle_improve"));
#endif
        if (AssetBundle == null) {
            Plugin.instance.LoggingInfo("Failed to load custom assets.");
        } else {
            var request = AssetBundle.LoadAssetAsync<GameObject>("AttackFeedback");
            yield return request;
            Prefabs.Add("AttackFeedback", request.asset as GameObject);

            foreach (var style in PluginData.KillMessageStyle.Values) {
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
    }
    
    public static GameObject LoadPrefab(string name, GameObject parent) {
        Plugin.instance.LoggingInfo($"Loading {name} Prefab...", true);
        return Object.Instantiate(Prefabs[name], parent.transform, true);
    }
    
    // public static async Task<GameObject> LoadAttackFeedbackPrefab(GameObject parent) {
    //     Plugin.instance.LoggingInfo("Loading Attack Feedback Prefab...", true);
    //     return Object.Instantiate(AssetBundle.LoadAsset<GameObject>("AttackFeedback"), parent.transform, true);
    // }
    //
    // public static GameObject LoadLootParticlePrefab(GameObject parent, string tier) {
    //     Plugin.instance.LoggingInfo("Loading Loot Particle Prefab...", true);
    //     return Object.Instantiate(AssetBundle.LoadAsset<GameObject>("LoopDrop" + tier), parent.transform, true);
    // }
}