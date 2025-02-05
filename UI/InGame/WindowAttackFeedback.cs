using System.Collections.Generic;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowAttackFeedback : WindowBase{
    private string resourcePack {
        get { return Plugin.StaticInstance.IndicatorGameObject == null ? "Missing" : "Loaded"; }
    }
    protected override void Init() {
        window = UWindow.Begin("Attack Feedback");
        window.Width += 50;
        StartPosition(310, 100);

        window.Label("资源包检测: " + resourcePack);
        window.Button("Reload", Plugin.StaticInstance.LoadPrefab);
        window.Space();
        
        window.Label("Hit Indicator: Sound");
        window.Slider("Volume", (SetIndicatorVolume), 0.5f, 0, 1, true);
        window.Slider("Volume", (SetIndicatorDistance), 0.5f, 0, 1, true);
        window.Space();

        window.Label("Hit Indicator: xCrossHair");
        window.ColorPicker("Color when hit", (SetHitColor), Color.white);
        window.ColorPicker("Color when kill", (SetChangeKillColor), Color.red);
        window.Space();
        
        window.Label("Kill Message");
        window.DropDown("Style", (SetKillMessageStyle), 0, new Dictionary<int, string>() {
            {0, "Battlefield 1"},
            {1, "Battlefield 5"}
        });
        window.Slider("Volume", (SetIndicatorVolume), 0.5f, 0, 1, true);
        window.Space();
        
        base.Init();
    }

    private void SetIndicatorVolume(float value) {
    }
    
    private void SetIndicatorDistance(float value) {
    }
    
    private void SetHitColor(Color color) {
    }
    
    private void SetChangeKillColor(Color color) {
    }
    
    private void SetKillMessageStyle(int value) {
    }
    
    private void SetKillMessageVolume(float value) {
    }
}