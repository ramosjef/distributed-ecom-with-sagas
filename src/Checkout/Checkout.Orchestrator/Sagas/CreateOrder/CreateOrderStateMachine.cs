using MassTransit;

using Shared.Messages.Commands;
using Shared.Messages.Events;

using Checkout.Orchestrator.Extensions;

namespace Checkout.Orchestrator.Sagas.CreateOrder
{
    public class CreateOrderStateMachine : MassTransitStateMachine<CreateOrderState>
    {
        public CreateOrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => PreOrderCreated, context => context.CorrelateById(x => x.Message.CorrelationId));
            Event(() => AuthorizePaymentProcessed, context => context.CorrelateById(x => x.Message.CorrelationId));
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
                    .Then(context => context.LogInfo("Evendo recebido PreOrderCreated {0}", context.Message.CorrelationId))
                    .PublishAsync(context => context.Init<AuthorizePayment>(new { context.Message.CorrelationId }))
                    .TransitionTo(AuthorizePaymentRequested),

                When(AuthorizePaymentProcessed)
                    .Then(context => context.LogInfo("Evendo recebido AuthorizePaymentProcessed => {0}", context.Message.CorrelationId))
                    .PublishAsync(context => context.Init<ProcessOnVendor>(new { context.Message.CorrelationId }))
                    .TransitionTo(ProcessOnVendorRequested)
            );

            During(AuthorizePaymentRequested,
                When(AuthorizePaymentProcessed)
                    .Then(context => context.LogInfo("Evendo recebido AuthorizePaymentProcessed => {0}", context.Message.CorrelationId))
                    .PublishAsync(context => context.Init<ProcessOnVendor>(new { context.Message.CorrelationId }))
                    .TransitionTo(ProcessOnVendorRequested),
                When(AuthorizePaymentFailed)
                    .Then(context => context.LogInfo("Erro ao autorizar o pedido => {0}", context.Message.CorrelationId))
                    .Finalize(),
                Ignore(PreOrderCreated));

            During(ProcessOnVendorRequested,
                When(VendorProcessed)
                    .Then(context => context.LogInfo("Evendo recebido VendorProcessed => {0}", context.Message.CorrelationId))
                    .PublishAsync(context => context.Init<ConfirmPayment>(new { context.Message.CorrelationId }))
                    .TransitionTo(ConfirmPaymentRequested),
                When(ProcessOnVendorFailed)
                    .PublishAsync(context => context.Init<CancelAuthorization>(new { context.Message.CorrelationId }))
                    .Then(context => context.LogInfo("Erro ao integrar o pedido no parceiro, solicitando o cancelamento da autorização no payment => {0}", context.Message.CorrelationId))
                    .TransitionTo(CancelAuthorizationRequested),
                Ignore(AuthorizePaymentProcessed),
                Ignore(PreOrderCreated));

            During(ConfirmPaymentRequested,
                When(PaymentConfirmed)
                    .Then(context => context.LogInfo("Evendo recebido PaymentConfirmed => {0}", context.Message.CorrelationId))
                    .Finalize(),
                When(ConfirmPaymentFailed)
                    .Then(context => context.LogInfo("Erro ao confirmar o pagamento => {0}", context.Message.CorrelationId))
                    .PublishAsync(context => context.Init<CancelVendorOrder>(new { context.Message.CorrelationId }))
                    .TransitionTo(CancelVendorOrderRequestested),
                Ignore(AuthorizePaymentProcessed),
                Ignore(PreOrderCreated),
                Ignore(VendorProcessed));

            During(CancelAuthorizationRequested,
                When(AuthorizationCanceled)
                    .Then(context => context.LogInfo("Autorização cancelada => {0}", context.Message.CorrelationId))
                    .Finalize());

            During(CancelVendorOrderRequestested,
                When(VendorOrderCanceled)
                    .Then(context => context.LogInfo("Pedido cancelado no parceiro => {0}", context.Message.CorrelationId))
                    .PublishAsync(context => context.Init<CancelAuthorization>(new { context.Message.CorrelationId }))
                    .TransitionTo(CancelAuthorizationRequested));
        }

        public Event<PreOrderCreated> PreOrderCreated { get; set; }
        public Event<PaymentAuthorized> AuthorizePaymentProcessed { get; set; }
        public Event<AuthorizePaymentFailed> AuthorizePaymentFailed { get; set; }
        public Event<VendorProcessed> VendorProcessed { get; set; }
        public Event<ProcessOnVendorFailed> ProcessOnVendorFailed { get; set; }
        public Event<PaymentConfirmed> PaymentConfirmed { get; set; }
        public Event<AuthorizationCanceled> AuthorizationCanceled { get; set; }
        public Event<ConfirmPaymentFailed> ConfirmPaymentFailed { get; set; }
        public Event<VendorOrderCanceled> VendorOrderCanceled { get; set; }

        public State AuthorizePaymentRequested { get; set; }
        public State ProcessOnVendorRequested { get; set; }
        public State ConfirmPaymentRequested { get; set; }
        public State CancelAuthorizationRequested { get; set; }
        public State CancelVendorOrderRequestested { get; set; }
    }
}
