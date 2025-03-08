using System.Collections;
using UnityEngine;
using UrGUI.UWindow;

namespace BattleImprove.UI.InGame;

public class WindowUpdateCheck : WindowBase{
    protected override void Start() {
        if (Plugin.needUpdate) {
            this.Init();
        } else {
            Destroy(this);
        }
    }
    
    protected override void Init() {
        window = UWindow.Begin("Update Check");
        StartPosition(0, 0);

        window.Space();
        window.Label(Plugin.i18n.GetText("NeedUpdate"));
        window.Label(Plugin.i18n.GetText("NeedUpdate.Current") + MyPluginInfo.PLUGIN_VERSION);
        window.Label(Plugin.i18n.GetText("NeedUpdate.Text"));
        
        base.Init();
        window.IsDrawing = true;
        StartCoroutine(AutoClose());
    }
    
    private IEnumerator AutoClose() {
        yield return new WaitForSeconds(5);
        Close();
        Destroy();
    }
}