namespace ChainOfResponsibility;

internal class GenericChainPart<TContext> : IChain<TContext>
{
    public IChain<TContext> Start { get; set; }
    
    private IChain<TContext>.HandleRequestDelegate? _next;

    private readonly Action<TContext> _action;

    public GenericChainPart(Action<TContext> action)
    {
        _action = action;
    }
    
    public void Add(IChain<TContext>.HandleRequestDelegate? next)
    {
        _next = next;
    }

    public void Execute(TContext context)
    {
        _action.Invoke(context);
        _next?.Invoke(context);
    }

    public void Execute(object context)
    {
        if (context is not TContext typedContext)
        {
            return;
        }
        
        Execute(typedContext);
    }
}

internal class AsyncGenericChainPart<TContext> : IAsyncChain<TContext>
{
    public IAsyncChain<TContext> Start { get; set; }
    
    private IAsyncChain<TContext>.HandleRequestDelegate? _next;

    private readonly Func<TContext, Task> _action;

    public AsyncGenericChainPart(Func<TContext, Task> action)
    {
        _action = action;
    }
    
    public void Add(IAsyncChain<TContext>.HandleRequestDelegate? next)
    {
        _next = next;
    }

    public async Task Execute(TContext context)
    {
        await _action.Invoke(context);
        if (_next is not null)
        {
            await _next.Invoke(context);
        }
        
    }

    public async Task Execute(object context)
    {
        if (context is not TContext typedContext)
        {
            return;
        }
        
        await Execute(typedContext);
    }
}
