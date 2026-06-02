using BattleLib.Context;
using BattleLib.Events;

namespace BattleLib.Example;

internal class CustomState {
    public float foo { get; }
    public float bar { get; }
    
    public CustomState(float foo, float bar) {
        this.foo = foo;
        this.bar = bar;
    }
}

public static class ExampleEventSubscribe {
    public static void Subscribe() {
        var _subscription = EventBus.Enemy.OnDamaged.Subscribe<CustomState>(ExamplePrefix, ExamplePostfix);
    }

    private static CustomState ExamplePrefix(DamagedContext damaged) {
        Plugin.Logger.LogInfo($"Enemy is about to take damage: {damaged.Damage}");
        return new CustomState(damaged.Damage, damaged.Damage * 2);
    }
    
    private static void ExamplePostfix(DamagedContext damaged, CustomState state) {
        Plugin.Logger.LogInfo($"Enemy took damage: {damaged.Damage}, CustomState foo: {state.foo}, bar: {state.bar}");
    }
}

