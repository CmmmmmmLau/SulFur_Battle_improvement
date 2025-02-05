using System;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowMenu : WindowBase {
    public void Update() {
        if(Input.GetKeyDown(KeyCode.F1)) {
            window.IsDrawing = !window.IsDrawing;
        }
    }

    protected override void Init() {
        window = UWindow.Begin("Cm Plugin Menu");
        StartPosition(100, 100);
        
        window.Button("Attack Feedback", () => OpenSubMenu("AttackFeedback"));
        base.Init();
    }
    
    private void OpenSubMenu(string name) {
        controller.OpenWindow(name);
    }
}