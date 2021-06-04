using Checkout.Orchestrator.Sagas.CreateOrder.Activities;

namespace Checkout.Orchestrator.Sagas.CreateOrder;

public sealed class CreateOrderStateMachine : MassTransitStateMachine<CreateOrderState>
{
    public CreateOrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => PreOrderCreated, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => PaymentAuthorized, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => VendorProcessed, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => PaymentConfirmed, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => AuthorizePaymentFailed, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => ProcessOnVendorFailed, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => AuthorizationCanceled, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => ConfirmPaymentFailed, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => VendorOrderCanceled, context => context.CorrelateById(x => x.Message.CorrelationId));

        Initially
        (
            When(PreOrderCreated)
                .Activity(x => x.OfType<PreOrderCreatedActivity>())
                .PublishAsync(AuthorizePaymentWhenPreOrderCreated)
                .TransitionTo(AuthorizingPayment),

            When(PaymentAuthorized)
                .Activity(x => x.OfType<PaymentAuthorizedActivity>())
                .PublishAsync(ProcessOnVendorWhenPaymentAuthorized)
                .TransitionTo(ProcessingOnVendor)
        );

        During(AuthorizingPayment,
            When(PaymentAuthorized)
                .Activity(x => x.OfType<PaymentAuthorizedActivity>())
                .PublishAsync(ProcessOnVendorWhenPaymentAuthorized)
                .TransitionTo(ProcessingOnVendor),
            When(AuthorizePaymentFailed)
                .Activity(x => x.OfType<AuthorizePaymentFailedActivity>())
                .Finalize(),
            Ignore(PreOrderCreated));

        During(ProcessingOnVendor,
            When(VendorProcessed)
                .Activity(x => x.OfType<VendorProcessedActivity>())
                .PublishAsync(ConfirmPaymentWhenVendorProcessed)
                .TransitionTo(ConfirmingPayment),
            When(ProcessOnVendorFailed)
                .PublishAsync(CancelAuthorizationWhenProcessVendorFailed)
                .TransitionTo(CancellingAuthorization),
            Ignore(PaymentAuthorized),
            Ignore(PreOrderCreated));

        During(ConfirmingPayment,
            When(PaymentConfirmed)
                .Activity(x => x.OfType<PaymentConfirmedActivity>())
                .Finalize(),
            When(ConfirmPaymentFailed)
                .PublishAsync(context => context.Init<CancelVendorOrder>(new { context.Saga.OrderId, context.Message.CorrelationId }))
                .TransitionTo(CancellingVendorOrder),
            Ignore(PaymentAuthorized),
            Ignore(PreOrderCreated),
            Ignore(VendorProcessed));

        During(CancellingAuthorization,
            When(AuthorizationCanceled)
                .Activity(x => x.OfType<AuthorizationCanceledActivity>())
                .Finalize());

        During(CancellingVendorOrder,
            When(VendorOrderCanceled)
                .PublishAsync(context => context.Init<CancelAuthorization>(new { context.Saga.OrderId, context.Message.CorrelationId }))
                .TransitionTo(CancellingAuthorization));
    }

    private static Task<SendTuple<AuthorizePayment>> AuthorizePaymentWhenPreOrderCreated(BehaviorContext<CreateOrderState, PreOrderCreated> ctx)
        => ctx.Init<AuthorizePayment>(new { ctx.Saga.CorrelationId, ctx.Message.OrderId });

    private static Task<SendTuple<ProcessOnVendor>> ProcessOnVendorWhenPaymentAuthorized(BehaviorContext<CreateOrderState, PaymentAuthorized> ctx)
        => ctx.Init<ProcessOnVendor>(new { ctx.Saga.CorrelationId, ctx.Message.OrderId });

    private static Task<SendTuple<ConfirmPayment>> ConfirmPaymentWhenVendorProcessed(BehaviorContext<CreateOrderState, VendorProcessed> ctx)
        => ctx.Init<ConfirmPayment>(new { ctx.Saga.OrderId, ctx.Message.CorrelationId });

    private static Task<SendTuple<CancelAuthorization>> CancelAuthorizationWhenProcessVendorFailed(BehaviorContext<CreateOrderState, ProcessOnVendorFailed> ctx)
        => ctx.Init<CancelAuthorization>(new { ctx.Saga.OrderId, ctx.Message.CorrelationId });

    public Event<PreOrderCreated> PreOrderCreated { get; set; }
    public Event<PaymentAuthorized> PaymentAuthorized { get; set; }
    public Event<AuthorizePaymentFailed> AuthorizePaymentFailed { get; set; }
    public Event<VendorProcessed> VendorProcessed { get; set; }
    public Event<ProcessOnVendorFailed> ProcessOnVendorFailed { get; set; }
    public Event<PaymentConfirmed> PaymentConfirmed { get; set; }
    public Event<AuthorizationCanceled> AuthorizationCanceled { get; set; }
    public Event<ConfirmPaymentFailed> ConfirmPaymentFailed { get; set; }
    public Event<VendorOrderCanceled> VendorOrderCanceled { get; set; }

    public State AuthorizingPayment { get; set; }
    public State ProcessingOnVendor { get; set; }
    public State ConfirmingPayment { get; set; }
    public State CancellingAuthorization { get; set; }
    public State CancellingVendorOrder { get; set; }
}
