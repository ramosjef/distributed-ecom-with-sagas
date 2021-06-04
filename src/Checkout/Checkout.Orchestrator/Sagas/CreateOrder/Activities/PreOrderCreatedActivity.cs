namespace Checkout.Orchestrator.Sagas.CreateOrder.Activities;

public sealed class PreOrderCreatedActivity :
    IStateMachineActivity<CreateOrderState, PreOrderCreated>
{
    private readonly ConsumeContext _context;

    public PreOrderCreatedActivity(ConsumeContext context)
    {
        _context = context;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<CreateOrderState, PreOrderCreated> context,
        IBehavior<CreateOrderState, PreOrderCreated> next)
    {
        context.Saga.Apply(context.Message);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<CreateOrderState, PreOrderCreated, TException> context,
        IBehavior<CreateOrderState, PreOrderCreated> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(PreOrderCreatedActivity));
    }
}
