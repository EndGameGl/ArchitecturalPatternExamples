namespace ChainOfResponsibility;

public class GenericConditionChainPart<TContext> : IChain<TContext>
{
    public IChain<TContext> Start { get; set; }

    private IChain<TContext>.HandleRequestDelegate? _next;

    private readonly Func<TContext, bool> _condition;

    public GenericConditionChainPart(Func<TContext, bool> condition)
    {
        _condition = condition;
    }

    public void Add(IChain<TContext>.HandleRequestDelegate? next)
    {
        _next = next;
    }

    public void Execute(TContext context)
    {
        if (!_condition(context))
        {
            return;
        }
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

public class AsyncGenericConditionChainPart<TContext> : IAsyncChain<TContext>
{
    public IAsyncChain<TContext> Start { get; set; }

    private IAsyncChain<TContext>.HandleRequestDelegate? _next;

    private readonly Func<TContext, Task<bool>> _condition;

    public AsyncGenericConditionChainPart(Func<TContext, Task<bool>> condition)
    {
        _condition = condition;
    }

    public void Add(IAsyncChain<TContext>.HandleRequestDelegate? next)
    {
        _next = next;
    }

    public async Task Execute(TContext context)
    {
        if (!await _condition(context))
        {
            return;
        }

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
