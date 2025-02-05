using System;
using System.Collections.Generic;
using BattleImprove.UI.InGame;
using UnityEngine;

namespace BattleImprove.Components;

public class MenuController : MonoBehaviour{
    Dictionary<string, WindowBase> windos = new Dictionary<string, WindowBase>();
    WindowBase currentWindow;
    
    private void Start() {
        var menu = this.gameObject.AddComponent<WindowMenu>().SetController(this);
        windos.Add("Menu", menu);
        
        var attackFeedback = this.gameObject.AddComponent<WindowAttackFeedback>().SetController(this);
        windos.Add("AttackFeedback", attackFeedback);
    }
    
    public void OpenWindow(string name) {
        if (currentWindow != null 
            && currentWindow != windos[name] 
            && currentWindow.window.IsDrawing) {
            currentWindow.Toggle();
        }
        currentWindow = windos[name];
        currentWindow.Toggle();
    }
}