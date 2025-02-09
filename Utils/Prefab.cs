using System.IO;
using System.Reflection;
using UnityEngine;

namespace BattleImprove.Utils;

public class Prefab {
    internal static AssetBundle AssetBundle;

    public static void LoadAssetBundle() {
        Plugin.instance.LoggingInfo("Loading Asset Bundle...", true);
        // Load asset bundle

#if DEBUG
        var sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "battle_improve"));
        if (AssetBundle == null) {
            Plugin.instance.LoggingInfo("Failed to load custom assets.");
            return;
        }
#else
        AssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("BattleImprove.Assets.battle_improve"));
#endif
    }
    
    public static GameObject LoadAttackFeedbackPrefab(GameObject parent) {
        Plugin.instance.LoggingInfo("Loading Attack Feedback Prefab...", true);
        return Object.Instantiate(AssetBundle.LoadAsset<GameObject>("AttackFeedback"), parent.transform, true);
    }
    
    public static GameObject LoadLootParticlePrefab(GameObject parent) {
        Plugin.instance.LoggingInfo("Loading Loot Particle Prefab...", true);
        return Object.Instantiate(AssetBundle.LoadAsset<GameObject>("LootParticle"), parent.transform, true);
    }
}