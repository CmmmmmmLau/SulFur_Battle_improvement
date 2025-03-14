using System.Collections.Generic;
using BattleImprove.UI.InGame;
using BattleImprove.Utils;
using PerfectRandom.Sulfur.Core;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.Components;

public class MenuController : MonoBehaviour{
    protected Dictionary<string, WindowBase> windos = new Dictionary<string, WindowBase>();
    protected WindowBase currentWindow;
    protected WindowBase menu;
    protected KeyCode menuKey {
        get {
            var data = PluginData.DataDict["BattleImprove"] as PluginData.Version;
            return data.menuKey;
        }
    }

    private void Start() {
        var window = this.gameObject.AddComponent<WindowUpdateCheck>().SetController(this);
        InitWindow();
    }
    
    public void Update() {
        if(Input.GetKeyDown(menuKey)) {
            ToggleMenu();
        }
    }
    
    public void ResetWindow() {
        ToggleMenu();
        foreach (var window in windos) {
            window.Value.Destroy();
        }
        windos.Clear();
        InitWindow();
    }

    private void InitWindow() {
        menu = this.gameObject.AddComponent<WindowMenu>().SetController(this);
        windos.Add("Menu", menu);
        
        var attackFeedback = this.gameObject.AddComponent<WindowAttackFeedback>().SetController(this);
        windos.Add("AttackFeedback", attackFeedback);
        
        var deadProtection = this.gameObject.AddComponent<WindowDeadProtection>().SetController(this);
        windos.Add("DeadProtection", deadProtection);
        
        var setting = this.gameObject.AddComponent<WindowSetting>().SetController(this);
        windos.Add("Setting", setting);
        
        var hotkey = this.gameObject.AddComponent<WindowHotkey>().SetController(this);
        windos.Add("Hotkey", hotkey);
    }

    public void ToggleMenu() {
        if (menu.window.ActiveSkin == null) {
            UWindow.RestoreGlobalDefaultSkin();
        }
        
        menu.Toggle();
        if (menu.window.IsDrawing) {
            if (currentWindow != null) {
                currentWindow.Toggle();
            }
            Pause(true);
        } else {
            CloseSubWindow();
            Pause(false);
        }
        this.SaveData();
    }
    
    public void SaveData() {
        PluginData.SaveData();
    }

    public void Pause(bool state) {
        var manager = StaticInstance<GameManager>.Instance;
        if (manager == null) return;
        manager.ModifyCursorState(GameManager.ControllerLockState.Paused, state);
        manager.ModifyControllerLock(GameManager.ControllerLockState.Paused, state);
    }
    
    public void OpenSubWindow(string name, bool closeCurrent = true) {
        if (currentWindow != null 
            && currentWindow != windos[name] 
            && currentWindow.window.IsDrawing 
            && closeCurrent) {
            currentWindow.Toggle();
        }

        if (closeCurrent) {
            currentWindow = windos[name];
            currentWindow.Toggle();
        } else {
            windos[name].Toggle();  
        }
    }
    
    public void CloseSubWindow() {
        foreach (var windowBase in windos) {
            windowBase.Value.window.IsDrawing = false;
        }
    }
}