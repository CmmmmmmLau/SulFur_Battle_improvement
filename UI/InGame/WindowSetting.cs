using System;
using System.Collections.Generic;
using System.Linq;
using BattleImprove.Utils;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowSetting : WindowBase {
    protected override void Init() {
        window = UWindow.Begin(i18n.GetText("Settings"));
        window.Width += 50;
        StartPosition(310, 100);
        window.Button(i18n.GetText("Reset"), (Reset));
        window.Button(i18n.GetText("Hotkey"), (ChangeMenuKey));
        
        base.Init();
    }

    private void Reset() {
        this.controller.ResetWindow();
    }

    private void ChangeMenuKey() {
        controller.OpenSubWindow("Hotkey", false);
    }
}