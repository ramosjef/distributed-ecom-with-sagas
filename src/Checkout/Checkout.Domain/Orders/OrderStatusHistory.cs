namespace Checkout.Domain.Orders;

public class OrderStatusHistory
{
    public OrderStatusHistory(OrderId orderId, OrderStatus orderStatus)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        CreatedAt = DateTime.UtcNow;
    }

    public OrderId OrderId { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
}