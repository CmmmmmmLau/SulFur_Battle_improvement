using BattleLib.Context;

namespace BattleLib.Events;

public static class EventBus {
    public static class Enemy  {
        public static HarmonyEvent<DamagedContext> OnDamaged { get; } = new HarmonyEvent<DamagedContext>();
    }
}