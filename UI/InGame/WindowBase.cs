﻿using System;
using System.Reflection;
using BattleImprove.Components;
using BattleImprove.Utils;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowBase : MonoBehaviour {
    public UWindow window;
    internal MenuController controller;
    protected LocalizationManager i18n = Plugin.i18n;
    
    protected virtual void Start() {
        this.Init();
    }

    public void Destroy() {
        Destroy(this);
    }
    
    public virtual void Toggle() {
        window.IsDrawing = !window.IsDrawing;
    }
    
    internal WindowBase StartPosition(int x, int y) {
        window.X = x;
        window.Y = y;
        return this;
    }
    
    internal WindowBase SetController(MenuController controller) {
        this.controller = controller;
        return this;
    }

    protected virtual void Init() {
        window.Space();
        window.Button("Close", Close);
        window.IsDrawing = false;
    }

    protected virtual void Close() {
        controller.SaveData();
        window.IsDrawing = false;
    }
}