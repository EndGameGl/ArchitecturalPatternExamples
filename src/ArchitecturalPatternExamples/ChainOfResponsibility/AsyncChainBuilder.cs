namespace ChainOfResponsibility;

public static class AsyncChainBuilder
{
    public static IAsyncChain Start<TContext>(IAsyncChain<TContext> chainStart)
    {
        chainStart.Start = chainStart;
        return chainStart;
    }

    public static IAsyncChain<TContext> Start<TContext>(Func<TContext, Task> chainStartAction)
    {
        var chainStart = new AsyncGenericChainPart<TContext>(chainStartAction);
        chainStart.Start = chainStart;
        return chainStart;
    }

    public static IAsyncChain<TContext> Start<TContext>(Func<TContext, Task<bool>> chainStartAction)
    {
        var chainStart = new AsyncGenericConditionChainPart<TContext>(chainStartAction);
        chainStart.Start = chainStart;
        return chainStart;
    }

    public static IAsyncChain<TContext> Then<TContext>(
        this IAsyncChain<TContext> current,
        IAsyncChain<TContext> nextPart
    )
    {
        current.Add(nextPart.Execute);
        nextPart.Start = current.Start;
        return nextPart;
    }

    public static IAsyncChain<TContext> Then<TContext>(
        this IAsyncChain<TContext> current,
        Func<TContext, Task> nextPartAction
    )
    {
        var nextPart = new AsyncGenericChainPart<TContext>(nextPartAction);
        current.Add(nextPart.Execute);
        nextPart.Start = current.Start;
        return nextPart;
    }

    public static IAsyncChain<TContext> Then<TContext>(
        this IAsyncChain<TContext> current,
        Func<TContext, Task<bool>> nextPartAction
    )
    {
        var nextPart = new AsyncGenericConditionChainPart<TContext>(nextPartAction);
        current.Add(nextPart.Execute);
        nextPart.Start = current.Start;
        return nextPart;
    }

    public static IAsyncChain<TContext> Build<TContext>(this IAsyncChain<TContext> chain)
    {
        return chain.Start;
    }
}