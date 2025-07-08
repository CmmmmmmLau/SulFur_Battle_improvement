using UnityEngine;

namespace BattleImprove.PluginData;

public class CrossHairData: ES3Data<CrossHairData> {
    public bool enable = true;
    public string style = "BF1";
    public Color hitColor = Color.white;
    public Color killColor = Color.red;
}