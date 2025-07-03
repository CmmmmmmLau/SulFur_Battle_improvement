using System.Collections.Generic;
using PerfectRandom.Sulfur.Core.Items;

namespace BattleImprove.PluginData;

public class KeptWeaponData: ES3Data<KeptWeaponData> {
    public bool opened = false;
    public List<InventoryData> weapons = new ();
}