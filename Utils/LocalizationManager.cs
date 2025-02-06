using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BattleImprove.Utils;

public class LocalizationManager {
    private static Dictionary<string, string> localizationText = new();
    private static SystemLanguage currentLanguage;
    
    private enum SupportedLanguages {
        English = SystemLanguage.English,
        ChineseSimplified = SystemLanguage.ChineseSimplified
    }

    public void LoadLocaliztion(SystemLanguage language) {
        if (Enum.IsDefined(typeof(SupportedLanguages), (int)language)) {
            currentLanguage = language;
        } else {
            currentLanguage = SystemLanguage.English;
        }
        
        localizationText.Clear();
        Assembly assembly = Assembly.GetExecutingAssembly();
        string langPatch = $"BattleImprove.Lang.{currentLanguage}.lang";

        using (Stream stream = assembly.GetManifestResourceStream(langPatch)) {
            if (stream == null) {
                Debug.LogError($"Failed to load localization file: {langPatch}");
                return;
            }

            using (StreamReader reader = new(stream)) {
                var lines = reader.ReadToEnd().Split('\n');
                foreach (var line in lines) {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                    var token = line.Split('=', 2);
                    if (token.Length == 2) {
                        localizationText[token[0].Trim()] = token[1].Trim();
                    }
                }
            }
        }
    }
    
    public string GetText(string key) {
        return localizationText.ContainsKey(key) ? localizationText[key] : key;
    }
}