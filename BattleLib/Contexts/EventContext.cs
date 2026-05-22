using System;
using System.Collections.Generic;

namespace BattleLib.Contexts;

public abstract class EventContext {
    private Dictionary<Type, object?> _data = new Dictionary<Type, object?>();

    public void SetData<T>(T value) {
        _data[typeof(T)] = value;
    }

    public T? GetData<T>() {
        return _data.TryGetValue(typeof(T), out var value) ? (T)value! : default;
    }
}