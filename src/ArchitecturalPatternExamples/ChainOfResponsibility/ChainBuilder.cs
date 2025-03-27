namespace ChainOfResponsibility;

public static class ChainBuilder
{
    public static IChain<TContext> Start<TContext>(IChain<TContext> chainStart)
    {
        chainStart.Start = chainStart;
        return chainStart;
    }

    public static IChain<TContext> Start<TContext>(Action<TContext> chainStartAction)
    {
        var chainStart = new GenericChainPart<TContext>(chainStartAction);
        chainStart.Start = chainStart;
        return chainStart;
    }

    public static IChain<TContext> Start<TContext>(Func<TContext, bool> chainStartAction)
    {
        var chainStart = new GenericConditionChainPart<TContext>(chainStartAction);
        chainStart.Start = chainStart;
        return chainStart;
    }

    public static IChain<TContext> Then<TContext>(
        this IChain<TContext> current,
        IChain<TContext> nextPart
    )
    {
        current.Add(nextPart.Execute);
        nextPart.Start = current.Start;
        return nextPart;
    }

    public static IChain<TContext> Then<TContext>(
        this IChain<TContext> current,
        Action<TContext> nextPartAction
    )
    {
        var nextPart = new GenericChainPart<TContext>(nextPartAction);
        current.Add(nextPart.Execute);
        nextPart.Start = current.Start;
        return nextPart;
    }

    public static IChain<TContext> Then<TContext>(
        this IChain<TContext> current,
        Func<TContext, bool> nextPartAction
    )
    {
        var nextPart = new GenericConditionChainPart<TContext>(nextPartAction);
        current.Add(nextPart.Execute);
        nextPart.Start = current.Start;
        return nextPart;
    }

    public static IChain<TContext> Build<TContext>(this IChain<TContext> chain)
    {
        return chain.Start;
    }
}
