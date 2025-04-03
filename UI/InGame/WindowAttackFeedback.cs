using System.Collections.Generic;
using System.Reflection;
using BattleImprove.Utils;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowAttackFeedback : WindowBase{
    PluginData.AttackFeedback data;
    private UWindowControls.WLabel label;
    
    private string resourcePack =>
        Plugin.IndicatorGameObject == null 
            ? i18n.GetText("AttackFeedback.resource.missed") : i18n.GetText("AttackFeedback.resource.loaded");

    protected override void Init() {
        data = DataManager.AttackFeedbackData;
        
        window = UWindow.Begin("Attack Feedback");
        window.Width += 50;
        StartPosition(310, 100);

        label = window.Label(i18n.GetText("AttackFeedback.resource") + ":" + resourcePack);
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
        window.DropDown(i18n.GetText("AttackFeedback.style"), (SetKillMessageStyle), data.messageStyle, DataManager.KillMessageStyle);
        window.Slider(i18n.GetText("AttackFeedback.volume"), (SetKillMessageVolume), data.messageVolume, 0, 1, true);
        window.Button(i18n.GetText("AttackFeedback.test"), (TestKillMessage));
        window.Space();
        
        base.Init();
    }
    
    private void ReloadPrefab() {
        if (Plugin.IndicatorGameObject == null) {
            PrefabManager.LoadAttackFeedbackPrefab();
        }
        
        var info = typeof(UWindowControls.WLabel).GetField("DisplayedString", BindingFlags.NonPublic | BindingFlags.Instance);
        if (info != null) {
            info.SetValue(label, i18n.GetText("AttackFeedback.resource") + ":" + resourcePack);
        }
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
        PrefabManager.LoadKillMessageStyle(DataManager.KillMessageStyle[value]);
    }
    
    private void SetKillMessageVolume(float value) {
        data.messageVolume = value;
    }

    private void TestKillMessage() {
        PluginInstance<MessageController>.Instance.OnEnemyKill("Enemy Name#" + Random.RandomRangeInt(0, 10)
            , "Weapon Name#" + Random.RandomRangeInt(0, 10)
            , Random.RandomRangeInt(0, 10).ToString()
            , Random.RandomRangeInt(0, 10) < 5
            , Random.RandomRangeInt(0, 10) < 5);
        PluginInstance<MessageController>.Instance.OnEnemyHit("Bullet Damage Type#" + Random.RandomRangeInt(0, 10), Random.RandomRangeInt(0, 100));
    }
}