using System;
using BattleImprove.Components;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowMenu : WindowBase {

    protected override void Init() {
        window = UWindow.Begin("Cm Plugin Menu");
        StartPosition(100, 100);
        
        window.Button(Plugin.i18n.GetText("AttackFeedback"), () => OpenSubMenu("AttackFeedback"));
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