using Checkout.Domain.Core;

using Shared.Messages.Events;

namespace Checkout.Domain.Orders;

public sealed class Order : Aggregate<OrderId>
{
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ModifiedAt { get; private set; }
    public List<OrderStatusHistory> OrderStatusHistories { get; private set; } = new List<OrderStatusHistory>();

    public void SetPaymentAuthorized(PaymentAuthorized paymentAuthorized)
    {
        Status = OrderStatus.PaymentAuthorized;
        ModifiedAt = DateTime.UtcNow;

        AddStatusHistory(Status);
    }

    public void SetVendorProcessed(VendorProcessed vendorProcessed)
    {
        Status = OrderStatus.CreatedOnVendor;
        ModifiedAt = DateTime.UtcNow;

        AddStatusHistory(Status);
    }

    public void SetPaymentConfirmed(PaymentConfirmed paymentConfirmed)
    {
        Status = OrderStatus.PaymentConfirmed;
        ModifiedAt = DateTime.UtcNow;

        AddStatusHistory(Status);
    }

    public void AddStatusHistory(OrderStatus orderStatus) =>
        OrderStatusHistories.Add(new OrderStatusHistory(Id, orderStatus));

    public static Order InitPreOrder()
    {
        Order order = new()
        {
            Status = OrderStatus.PreOrder,
            CreatedAt = DateTime.UtcNow,
        };

        order.AddStatusHistory(order.Status);

        return order;
    }
}
