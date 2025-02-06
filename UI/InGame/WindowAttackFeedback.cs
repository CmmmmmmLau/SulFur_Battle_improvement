using System.Collections.Generic;
using BattleImprove.Utils;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowAttackFeedback : WindowBase{
    PluginData.AttackFeedbackData data;
    private LocalizationManager i18n = Plugin.i18n;
    
    private string resourcePack =>
        Plugin.StaticInstance.IndicatorGameObject == null 
            ? i18n.GetText("AttackFeedback.resource.missed") : i18n.GetText("AttackFeedback.resource.loaded");

    protected override void Init() {
        data = PluginData.DataDict["AttackFeedback"] as PluginData.AttackFeedbackData;
        
        window = UWindow.Begin("Attack Feedback");
        window.Width += 50;
        StartPosition(310, 100);

        window.Label(i18n.GetText("AttackFeedback.resource") + ":" + resourcePack);
        window.Button("Reload", (ReloadPrefab));
        window.Space();
        
        window.Label(i18n.GetText("AttackFeedback.indicator"));
        window.Slider(i18n.GetText("AttackFeedback.volume"), (SetIndicatorVolume), data.indicatorVolume, 0, 1, true);
        window.Slider(i18n.GetText("AttackFeedback.distance"), (SetIndicatorDistance), data.indicatorDistance, 0, 1, true);
        window.Slider(i18n.GetText("AttackFeedback.distance.far"), (SetIndicatorDistanceFar), data.indicatorDistanceFar, 0, 1, true);
        window.Slider(i18n.GetText("AttackFeedback.distance.headshot"), (SetIndicatorDistanceHeadshoot), data.indicatorDistanceHeadShoot, 0, 1, true);
        window.Space();

        window.Label(i18n.GetText("AttackFeedback.cross"));
        window.ColorPicker(i18n.GetText("AttackFeedback.hitcolor"), (SetHitColor), data.hitColor);
        window.ColorPicker(i18n.GetText("AttackFeedback.killcolor"), (SetChangeKillColor), data.killColor);
        window.Space();
        
        window.Label(i18n.GetText("AttackFeedback.message"));
        window.DropDown(i18n.GetText("AttackFeedback.style"), (SetKillMessageStyle), data.messageStyle, new Dictionary<int, string>() {
            {0, "Battlefield 1"},
            {1, "Battlefield 5"}
        });
        window.Slider(i18n.GetText("AttackFeedback.volume"), (SetKillMessageVolume), data.messageVolume, 0, 1, true);
        window.Space();
        
        base.Init();
    }
    
    private void ReloadPrefab() {
        Plugin.StaticInstance.LoadPrefab();
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