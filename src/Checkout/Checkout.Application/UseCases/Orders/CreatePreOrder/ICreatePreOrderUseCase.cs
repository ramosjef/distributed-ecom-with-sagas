using System.Threading.Tasks;

namespace Checkout.Application.UseCases.Orders.CreatePreOrder
{
    public interface ICreatePreOrderUseCase
    {
        Task ExecuteAsync(CreatePreOrderRequest request);
    }
}
