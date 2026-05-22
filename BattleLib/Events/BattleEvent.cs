using System;
using BattleLib.Contexts;

namespace BattleLib.Events;

public class BattleEvent<T> where T : EventContext {
    public event Action<T>? OnPre;
    public event Action<T>? OnPost;

    public void FirePreEvent(T ctx) {
        OnPre?.Invoke(ctx);
    }

    public void FirePostEvent(T ctx) {
        OnPost?.Invoke(ctx);
    }
}