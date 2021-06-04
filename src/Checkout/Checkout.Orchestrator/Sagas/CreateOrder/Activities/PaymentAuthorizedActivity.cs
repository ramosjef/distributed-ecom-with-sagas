using Checkout.Application.UseCases.SetPaymentAuthorized;

namespace Checkout.Orchestrator.Sagas.CreateOrder.Activities;

public sealed class PaymentAuthorizedActivity :
    IStateMachineActivity<CreateOrderState, PaymentAuthorized>
{
    private readonly ConsumeContext _context;
    private readonly ISetPaymentAuthorizedUseCase _setPaymentAuthorizedUseCase;

    public PaymentAuthorizedActivity(ConsumeContext context,
                                     ISetPaymentAuthorizedUseCase setPaymentAuthorizedUseCase)
    {
        _context = context;
        _setPaymentAuthorizedUseCase = setPaymentAuthorizedUseCase;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<CreateOrderState, PaymentAuthorized> context,
        IBehavior<CreateOrderState, PaymentAuthorized> next)
    {
        context.Saga.Apply(context.Message);

        await _setPaymentAuthorizedUseCase.ExecuteAsync(context.Message, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<CreateOrderState, PaymentAuthorized, TException> context,
        IBehavior<CreateOrderState, PaymentAuthorized> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(PaymentAuthorizedActivity));
    }
}
