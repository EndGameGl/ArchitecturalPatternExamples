namespace ChainOfResponsibility;

public interface IAsyncChain<TContext> : IAsyncChain
{
    internal IAsyncChain<TContext> Start { get; set; }
    
    delegate Task HandleRequestDelegate(TContext context);

    void Add(HandleRequestDelegate? next);
    Task Execute(TContext context);
}

public interface IAsyncChain
{
    Task Execute(object context);
}