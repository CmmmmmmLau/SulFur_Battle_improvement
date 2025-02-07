using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowHotkey : WindowBase {
    UWindowControls.WLabel label;
    private bool needInput = false;
    private KeyCode key;
    
    protected override void Init() {
        window = UWindow.Begin(i18n.GetText("Hotkey.title"));
        var data = PluginData.DataDict["BattleImprove"] as PluginData.Version;
        label = window.Label(i18n.GetText("Hotkey.current") + " " + data.menuKey.ToString());
        StartPosition(350, 350);
        
        base.Init();
    }

    public override void Toggle() {
        base.Toggle();
        needInput = window.IsDrawing;
    }

    private void Update() {
        if (needInput) {
            Event e = Event.current;
            if (e.isKey) {
                key = e.keyCode;
                var info = typeof(UWindowControls.WLabel).GetField("DisplayedString", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null) {
                    info.SetValue(label,i18n.GetText("Hotkey.current") + " " + key.ToString());
                }
            }
        }
    }

    protected override void Close() {
        var data = PluginData.DataDict["BattleImprove"] as PluginData.Version;
        data.menuKey = key;
        PluginData.SaveData();
        base.Close();
    }
}