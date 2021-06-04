namespace Checkout.Orchestrator.Sagas.CreateOrder.Activities;

public sealed class AuthorizePaymentFailedActivity
    : IStateMachineActivity<CreateOrderState, AuthorizePaymentFailed>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<CreateOrderState, AuthorizePaymentFailed> context,
        IBehavior<CreateOrderState, AuthorizePaymentFailed> next)
    {

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<CreateOrderState, AuthorizePaymentFailed, TException> context,
        IBehavior<CreateOrderState, AuthorizePaymentFailed> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(AuthorizePaymentFailedActivity));
    }
}