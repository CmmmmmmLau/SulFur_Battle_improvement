using System;
using System.Collections.Generic;
using PerfectRandom.Sulfur.Core.Items;

namespace BattleImprove.PluginData;

[Serializable]
public class DeadProtectionData: ES3Data<DeadProtectionData> {
    public bool enable = true;
    public float weaponDurability = 0.3f;
    public float attachmentChance = 0.3f;
    public float enchantmentChance = 0.3f;
    public float barrelChance = 0.3f;
}