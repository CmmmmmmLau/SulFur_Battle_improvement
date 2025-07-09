namespace BattleImprove.PluginData;

public class ImpactSoundData: ES3Data<ImpactSoundData> {
    public bool enable = true;
    public string style = "BF1";
    public float volume = 0.5f;
    public float volumeFar = 0.5f;
    public float volumeCrit = 0.5f;
}