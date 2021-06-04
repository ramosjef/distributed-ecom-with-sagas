namespace Checkout.Orchestrator.Sagas.CreateOrder;

public sealed class CreateOrderState :
    SagaStateMachineInstance,
    ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public long OrderId { get; set; }
    public string CurrentState { get; internal set; }
    public int Version { get; set; }

    public List<CorrelatedBy<Guid>> Events { get; set; } = new List<CorrelatedBy<Guid>>();

    public void Apply(PreOrderCreated message)
    {
        OrderId = message.OrderId;
        Events.Add(message);
    }

    public void Apply(PaymentAuthorized message)
    {
        OrderId = OrderId is 0 ? message.OrderId : OrderId;
        Events.Add(message);
    }

    public void Apply(VendorProcessed message)
    {
        OrderId = OrderId is 0 ? message.OrderId : OrderId;
        Events.Add(message);
    }

    public void Apply(PaymentConfirmed message)
    {
        OrderId = OrderId is 0 ? message.OrderId : OrderId;
        Events.Add(message);
    }
}
