using System;

namespace Checkout.Application.Domain.Orders
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; private set; }
    }
}
