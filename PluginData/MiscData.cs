using System;

namespace BattleImprove.PluginData;

[Serializable]
public class MiscData: ES3Data<MiscData> {
    public bool expShareEnable = true;
    public float expShareProportion = 0.5f;
    
    public bool healthBarEnable = true;
}