using Checkout.Application.UseCases.SetVendorProcessed;

namespace Checkout.Orchestrator.Sagas.CreateOrder.Activities;

public sealed class VendorProcessedActivity
    : IStateMachineActivity<CreateOrderState, VendorProcessed>
{
    private readonly ConsumeContext _context;
    private readonly ISetVendorProcessedUseCase _setVendorProcessedUseCase;

    public VendorProcessedActivity(ConsumeContext context,
                                   ISetVendorProcessedUseCase setVendorProcessedUseCase)
    {
        _context = context;
        _setVendorProcessedUseCase = setVendorProcessedUseCase;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<CreateOrderState, VendorProcessed> context, IBehavior<CreateOrderState, VendorProcessed> next)
    {
        context.Saga.Apply(context.Message);

        await _setVendorProcessedUseCase.ExecuteAsync(context.Message, context.CancellationToken);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<CreateOrderState, VendorProcessed, TException> context,
        IBehavior<CreateOrderState, VendorProcessed> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(VendorProcessedActivity));
    }
}