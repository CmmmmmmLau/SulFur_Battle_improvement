using System;
using System.Collections.Generic;
using BattleImprove.UI.InGame;
using PerfectRandom.Sulfur.Core;
using UnityEngine;

namespace BattleImprove.Components;

public class MenuController : MonoBehaviour{
    protected Dictionary<string, WindowBase> windos = new Dictionary<string, WindowBase>();
    protected WindowBase currentWindow;
    protected WindowBase menu;
    
    private void Start() {
        this.gameObject.AddComponent<WindowUpdateCheck>();
        
        menu = this.gameObject.AddComponent<WindowMenu>().SetController(this);
        windos.Add("Menu", menu);
        
        var attackFeedback = this.gameObject.AddComponent<WindowAttackFeedback>().SetController(this);
        windos.Add("AttackFeedback", attackFeedback);
    }
    
    public void Update() {
        if(Input.GetKeyDown(KeyCode.F1)) {
            ToggleMenu();
        }
    }

    public void ToggleMenu() {
        menu.Toggle();
        if (menu.window.IsDrawing) {
            if (currentWindow != null) {
                currentWindow.Toggle();
            }
            StaticInstance<GameManager>.Instance.ModifyCursorState(GameManager.ControllerLockState.Paused, true);
            StaticInstance<GameManager>.Instance.ModifyGamePauseState(GameManager.ControllerLockState.Paused, true);
            StaticInstance<GameManager>.Instance.ModifyControllerLock(GameManager.ControllerLockState.Paused, true);
        } else {
            CloseSubWindow();
            StaticInstance<GameManager>.Instance.ModifyCursorState(GameManager.ControllerLockState.Paused, false);
            StaticInstance<GameManager>.Instance.ModifyGamePauseState(GameManager.ControllerLockState.Paused, false);
            StaticInstance<GameManager>.Instance.ModifyControllerLock(GameManager.ControllerLockState.Paused, false);
            PluginData.SaveData();
        }
    }
    
    public void OpenSubWindow(string name) {
        if (currentWindow != null 
            && currentWindow != windos[name] 
            && currentWindow.window.IsDrawing) {
            currentWindow.Toggle();
        }
        currentWindow = windos[name];
        currentWindow.Toggle();
    }
    
    public void CloseSubWindow() {
        if (currentWindow != null && currentWindow.window.IsDrawing) {
            currentWindow.Toggle();
        }
    }
}