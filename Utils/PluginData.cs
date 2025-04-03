using System;
using System.Collections.Generic;
using PerfectRandom.Sulfur.Core;
using PerfectRandom.Sulfur.Core.Items;
using UnityEngine;

namespace BattleImprove.Utils;

[Serializable]
public class PluginData {
    public class Version : PluginData{
        public string version => MyPluginInfo.PLUGIN_VERSION;
        public KeyCode menuKey = KeyCode.F1;
    }
    
    public class AttackFeedback : PluginData {
        public float indicatorVolume = 0.5f;
        public float indicatorDistance = 0.5f;
        public float indicatorDistanceFar = 0.5f;
        public float indicatorDistanceHeadShoot = 0.5f;
        public Color hitColor = Color.white;
        public Color killColor = Color.red;
        public int messageStyle = 0;
        public float messageVolume = 0.5f;
    }
    
    public class DeadProtection : PluginData {
        public bool opened = false;
        public List<InventoryData> weapons = new ();
        public float weaponDurability = 0.3f;
        public float attachmentChance = 0.3f;
        public float enchantmentChance = 0.3f;
    }
}