using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BattleImprove.Utils;

public class LocalizationManager {
    private static Dictionary<string, string> localizationText = new();
    private static SystemLanguage currentLanguage;
    
    public enum SupportedLanguages {
        English,
        ChineseSimplified
    }

    public void LoadLocalization(SystemLanguage language) {
        if (Enum.GetNames(typeof(SupportedLanguages)).Contains(language.ToString())) {
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
    
    public interface II18N {
        protected string GetText(string key) {
            return Plugin.i18n.GetText(key);
        }
    }
}