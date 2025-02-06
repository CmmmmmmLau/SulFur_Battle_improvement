using System.Collections.Generic;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowAttackFeedback : WindowBase{
    PluginData.AttackFeedbackData data;
    
    private string resourcePack {
        get { return Plugin.StaticInstance.IndicatorGameObject == null ? "Missing" : "Loaded"; }
    }
    protected override void Init() {
        data = PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedbackData;
        
        window = UWindow.Begin("Attack Feedback");
        window.Width += 50;
        StartPosition(310, 100);

        window.Label("资源包检测: " + resourcePack);
        window.Button("Reload", Plugin.StaticInstance.LoadPrefab);
        window.Space();
        
        window.Label("Hit Indicator: Sound");
        window.Slider("Volume", (SetIndicatorVolume), data.indicatorVolume, 0, 1, true);
        window.Slider("Distance", (SetIndicatorDistance), data.indicatorDistance, 0, 1, true);
        window.Slider("Distance-far", (SetIndicatorDistanceFar), data.indicatorDistanceFar, 0, 1, true);
        window.Slider("Distance-headshot", (SetIndicatorDistanceHeadshoot), data.indicatorDistanceHeadShoot, 0, 1, true);
        window.Space();

        window.Label("Hit Indicator: xCrossHair");
        window.ColorPicker("Color when hit", (SetHitColor), data.hitColor);
        window.ColorPicker("Color when kill", (SetChangeKillColor), data.killColor);
        window.Space();
        
        window.Label("Kill Message");
        window.DropDown("Style", (SetKillMessageStyle), data.messageStyle, new Dictionary<int, string>() {
            {0, "Battlefield 1"},
            {1, "Battlefield 5"}
        });
        window.Slider("Volume", (SetKillMessageVolume), data.messageVolume, 0, 1, true);
        window.Space();
        
        base.Init();
    }

    private void SetIndicatorVolume(float value) {
        data.indicatorVolume = value;
    }
    
    private void SetIndicatorDistance(float value) {
        data.indicatorDistance = value;
    }
    
    private void SetIndicatorDistanceFar(float value) {
        data.indicatorDistanceFar = value;
    }
    
    private void SetIndicatorDistanceHeadshoot(float value) {
        data.indicatorDistanceHeadShoot = value;
    }
    
    private void SetHitColor(Color color) {
        data.hitColor = color;
    }
    
    private void SetChangeKillColor(Color color) {
        data.killColor = color;
    }
    
    private void SetKillMessageStyle(int value) {
        data.messageStyle = value;
    }
    
    private void SetKillMessageVolume(float value) {
        data.messageVolume = value;
    }
}