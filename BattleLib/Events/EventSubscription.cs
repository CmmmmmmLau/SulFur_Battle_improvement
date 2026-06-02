using System;

namespace BattleLib.Events;

public class EventSubscription: IDisposable {
    private readonly Action? _unsubscribe;
    private bool _disposed;

    public EventSubscription(Action action) {
        _unsubscribe = action;
    }
    
    public void Dispose() {
        if (_disposed) return;

        _disposed = true;
        _unsubscribe?.Invoke();
    }
}