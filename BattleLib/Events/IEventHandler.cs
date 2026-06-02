namespace BattleLib.Events;

internal interface IEventHandler {
    string Id { get; }
    
    object? InvokePrefix(object context);
    void InvokePostfix(object context, object? prefixState);
}