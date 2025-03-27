namespace ChainOfResponsibility;

public interface IChain<TContext> : IChain
{
    internal IChain<TContext> Start { get; set; }
    
    delegate void HandleRequestDelegate(TContext context);

    void Add(HandleRequestDelegate? next);
    void Execute(TContext context);
}

public interface IChain
{
    void Execute(object context);
}