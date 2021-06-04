using System;

namespace Checkout.Application.Domain.Orders
{
    public interface IOrderRepository
    {
        public Order GetId(Guid id);
        public void Save(Order order);
    }
}
