namespace Checkout.Orchestrator.Sagas.CreateOrder.Activities;

public sealed class AuthorizationCanceledActivity
    : IStateMachineActivity<CreateOrderState, AuthorizationCanceled>
{
    public AuthorizationCanceledActivity()
    {
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<CreateOrderState, AuthorizationCanceled> context,
        IBehavior<CreateOrderState, AuthorizationCanceled> next)
    {
        //TODO: cancelar pedido

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<CreateOrderState, AuthorizationCanceled, TException> context,
        IBehavior<CreateOrderState, AuthorizationCanceled> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(AuthorizationCanceledActivity));
    }
}