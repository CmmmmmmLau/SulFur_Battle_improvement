using System;

namespace BattleImprove.PluginData;

[Serializable]
public class MiscData: ES3Data<MiscData> {
    public bool enable = true;
    public float expShareProportion = 0.5f;
}