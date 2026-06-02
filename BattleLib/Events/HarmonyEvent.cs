using System;
using System.Collections.Generic;

namespace BattleLib.Events;


public class HarmonyEvent<TContext>{
    private readonly List<IEventHandler> _handlers = new List<IEventHandler>();
    private IEventHandler[] _handlerCache = Array.Empty<IEventHandler>();
    
    public EventSubscription Subscribe<TState>(Func<TContext, TState>? prefix, Action<TContext, TState>? postfix) {
        
        var handler = new HarmonyEventHandler<TContext, TState>(prefix, postfix);
        _handlers.Add(handler);
        UpdateCache();
        
        Plugin.Logger.LogInfo($"Subscribed to event with handler: {handler.Id}");
        
        return new EventSubscription(() => Unsubscribe(handler));
    }

    public EventSubscription Subscribe(Action<TContext>? prefix = null, Action<TContext>? postfix = null) {
        return Subscribe<object?>(
            ctx => {
                prefix?.Invoke(ctx);
                return null;
            },
            (ctx, _) => {
                postfix?.Invoke(ctx);
            }
        );
    }

    internal HandlerStateContainer? InvokePrefix(TContext context) {
        if (_handlerCache.Length == 0) return null;
        
        var handlers = _handlerCache;
        var container = new HandlerStateContainer(handlers.Length);
        
        foreach (var handler in handlers) {
            try {
                var state = handler.InvokePrefix(context!);
                container.Pairs.Add(new HandlerStatePair(handler, state));
            }
            catch (Exception e) {
                LogHandlerException("InvokePrefix", handler, e);
            }

        }
        
        return container.Pairs.Count == 0 ? null: container;
    }

    internal void InvokePostfix(TContext context, HandlerStateContainer? container) {
        if (container == null) return;

        foreach (var pair in container.Pairs) {
            try {
                pair.Handler.InvokePostfix(context!, pair.State);
            }
            catch (Exception e) {
                LogHandlerException("InvokePostfix", pair.Handler, e);
            }
        }
    }
    
    private void UpdateCache() {
        _handlerCache = _handlers.Count == 0? Array.Empty<IEventHandler>() : _handlers.ToArray();
    }
    
    private static void LogHandlerException(string method, IEventHandler handler, Exception e) {
        Plugin.Logger.LogError($"Exception in {method} of handler {handler.Id}: {e}");
    }

    private void Unsubscribe(IEventHandler handler) {
        if (!_handlers.Remove(handler)) return;
        
        UpdateCache();
        Plugin.Logger.LogInfo($"Unsubscribed from event with handler: {handler.Id}");
    }
}

internal class HandlerStateContainer {
    public List<HandlerStatePair> Pairs { get; }

    public HandlerStateContainer(int size) {
        Pairs = new List<HandlerStatePair>(size);
    }
}

internal readonly struct HandlerStatePair {
    public IEventHandler Handler { get; }
    public object? State { get; }
    
    public HandlerStatePair(IEventHandler handler, object? state) {
        Handler = handler;
        State = state;
    }
}