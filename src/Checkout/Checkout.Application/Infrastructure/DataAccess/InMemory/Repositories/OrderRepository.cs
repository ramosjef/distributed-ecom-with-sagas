using System;
using System.Collections.Generic;
using System.Linq;

using Checkout.Application.Domain.Orders;

namespace Checkout.Application.Infrastructure.DataAccess.InMemory
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> Orders;

        public OrderRepository()
        {
            Orders = new List<Order>();
        }

        public Order GetId(Guid id)
        {
            return Orders.FirstOrDefault(o => o.OrderId == id);
        }

        public void Save(Order order)
        {
            if (order.OrderId.Equals(Guid.Empty))
                Orders.Remove(order);
            Orders.Add(order);
        }
    }
}
