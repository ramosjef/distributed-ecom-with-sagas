using Checkout.Application.UseCases.SetPaymentConfirmed;

namespace Checkout.Orchestrator.Sagas.CreateOrder.Activities;

public sealed class PaymentConfirmedActivity :
    IStateMachineActivity<CreateOrderState, PaymentConfirmed>
{
    private readonly ConsumeContext _context;
    private readonly ISetPaymentConfirmedUseCase _setPaymentConfirmedUseCase;

    public PaymentConfirmedActivity(ConsumeContext context,
                                    ISetPaymentConfirmedUseCase setPaymentConfirmedUseCase)
    {
        _context = context;
        _setPaymentConfirmedUseCase = setPaymentConfirmedUseCase;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<CreateOrderState, PaymentConfirmed> context,
        IBehavior<CreateOrderState, PaymentConfirmed> next)
    {
        context.Saga.Apply(context.Message);

        await _setPaymentConfirmedUseCase.ExecuteAsync(context.Message, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<CreateOrderState, PaymentConfirmed, TException> context,
        IBehavior<CreateOrderState, PaymentConfirmed> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(PaymentConfirmedActivity));
    }
}
