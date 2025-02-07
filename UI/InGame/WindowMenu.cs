using System;
using BattleImprove.Components;
using BattleImprove.Utils;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowMenu : WindowBase {
    protected override void Init() {
        window = UWindow.Begin("Battle Improvement");
        StartPosition(100, 100);
        
        window.Button(i18n.GetText("AttackFeedback"), () => OpenSubMenu("AttackFeedback"));
        window.Button(i18n.GetText("Settings"), () => OpenSubMenu("Setting"));
        base.Init();
    }

    protected override void Close() {
        base.Close();
        this.controller.CloseSubWindow();
    }

    private void OpenSubMenu(string name) {
        controller.OpenSubWindow(name);
    }
}