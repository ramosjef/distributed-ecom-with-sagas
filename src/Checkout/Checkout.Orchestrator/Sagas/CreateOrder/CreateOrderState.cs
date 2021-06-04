using System;

using MassTransit;

namespace Checkout.Orchestrator.Sagas.CreateOrder
{
    public class CreateOrderState :
        SagaStateMachineInstance,
        ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; internal set; }
        public int Version { get; set; }
    }
}
