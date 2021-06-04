using System.Threading.Tasks;

namespace Checkout.Application.UseCases.Orders.CreatePreOrder
{
    public sealed class CreatePreOrderUseCase : ICreatePreOrderUseCase
    {
        public CreatePreOrderUseCase() { }

        public Task ExecuteAsync(CreatePreOrderRequest request) => Task.CompletedTask;
    }
}
