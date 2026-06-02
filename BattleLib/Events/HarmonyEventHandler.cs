using System;

namespace BattleLib.Events;

internal class HarmonyEventHandler<TContext, TState>: IEventHandler {
    private readonly Func<TContext, TState>? _prefix;
    private readonly Action<TContext, TState>? _postfix;
    
    public string Id { get; }
    
    public HarmonyEventHandler(Func<TContext, TState>? prefix, Action<TContext, TState>? postfix) {
        _prefix = prefix;
        _postfix = postfix;
        
        var prefixName = prefix?.Method.Name ?? "<No Prefix>";
        var postfixName = postfix?.Method.Name ?? "<No Postfix>";
        
        Id = $"{prefixName} / {postfixName}";
    }

    public object? InvokePrefix(object context) {
        return _prefix == null ? default(TState) : _prefix((TContext)context);
    }

    public void InvokePostfix(object context, object? prefixState) {
        _postfix?.Invoke((TContext)context, (TState)prefixState!);
    }
}