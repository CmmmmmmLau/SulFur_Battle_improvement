using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;

namespace BattleImprove;

public class Config {
    internal static ConfigEntry<bool> EnableHealthBar;
    internal static ConfigEntry<bool> EnableExpShare;
    internal static ConfigEntry<bool> EnableSoundFeedback;
    internal static ConfigEntry<bool> EnableDeadUnitCollision;
    internal static ConfigEntry<bool> EnableXCrossHair;
    internal static ConfigEntry<bool> EnableDamageMessage;
    internal static ConfigEntry<bool> EnableLoopDropVFX;
    internal static ConfigEntry<bool> EnableDeadProtection;
    
    
    internal static ConfigEntry<float> Proportion;
        
    public static void InitConifg(ConfigFile cfg) {
        ToggleConfigInit(cfg);
        ExpShareConfigInit(cfg);
    }

    private static void ToggleConfigInit(ConfigFile cfg) {
        EnableExpShare = cfg.Bind("Toggle/开关", "EnableExpShare", true, "Enable experience share/是否开启经验共享");
        EnableHealthBar = cfg.Bind("Toggle/开关", "EnableHealthBar", true, "Enable health bar/是否开启血条");
        EnableXCrossHair = cfg.Bind("Toggle/开关", "EnableHitFeedback", true, "Enable xCrossHair feedback/是否开启击中准心反馈");
        EnableSoundFeedback = cfg.Bind("Toggle/开关", "EnableSoundFeedback", true, "Enable hit sound feedback on enemies/是否开启敌人受击声音反馈");
        EnableDamageMessage = cfg.Bind("Toggle/开关", "EnableDamageMessage", true, "Enable damage and kill message/是否开启伤害与击杀信息");
        EnableDeadUnitCollision = cfg.Bind("Toggle/开关", "EnableDeadUnitCollision", true, "Allowing bullets to pass through deadbody/是否使子弹能穿过尸体");
        EnableLoopDropVFX = cfg.Bind("Toggle/开关", "EnableLoopDropVFX", true, "Enable loot drop VFX/是否开启掉落特效");
        EnableDeadProtection = cfg.Bind("Toggle/开关", "EnableDeadProtection", true, "Enable dead protection/是否开启死亡保护");
    }

    private static void ExpShareConfigInit(ConfigFile cfg) {
        Proportion = cfg.Bind("ExpShare/经验共享", "Proportion", 0.5f, "The proportion of experience shared to second weapon/共享给第二把武器的经验比例");
    }
}