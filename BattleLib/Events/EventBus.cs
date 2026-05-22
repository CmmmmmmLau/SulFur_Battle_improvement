using BattleLib.Events;

namespace BattleLib.Contexts;

public static class EventBus {
    public static class Enemy {
        public static BattleEvent<DamageContext> OnDamage = new BattleEvent<DamageContext>();
    }
    
    public static class Player {
        public static BattleEvent<DamageContext> OnDamage = new BattleEvent<DamageContext>();
    }
}