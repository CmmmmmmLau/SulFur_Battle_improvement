using System;
using System.Net;

namespace BattleImprove.Utils;

public class UpdateChecker {
    private const string UPDATE_URL = "https://api.github.com/repos/CmmmmmmLau/SulFur_Battle_improvement/releases/latest";
    
    public static bool CheckForUpdate() {
        using WebClient client = new WebClient();
        client.Headers.Add("User-Agent", "UnityModUpdater");
            
        try {
            var json = client.DownloadString(UPDATE_URL);
            var currentVersion = MyPluginInfo.PLUGIN_VERSION;
            var latestVersion = ExtractVersionFromJson(json);
                
            if (CheckUpdate(currentVersion, latestVersion)) {
                Plugin.Logger.LogInfo($"New version available: {latestVersion}");
                return true;
            } else {
                Plugin.Logger.LogInfo("No new version available.");
                return false;
            }
        } catch (WebException e) {
            Plugin.Logger.LogError($"Failed to check for update: {e.Message}");
            return false;
        }
    }
    
    private static bool CheckUpdate(string current, string latest)
    {
        Version vCurrent = new Version(current);
        Version vLatest = new Version(latest);
        return vLatest > vCurrent;
    }
    
    private static string ExtractVersionFromJson(string json)
    {
        int start = json.IndexOf("\"tag_name\":\"") + 12;
        int end = json.IndexOf("\",", start);
        return json.Substring(start, end - start).Replace("v", "");
    }
}