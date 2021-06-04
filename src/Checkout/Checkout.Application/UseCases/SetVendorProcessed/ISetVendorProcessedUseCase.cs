namespace Checkout.Application.UseCases.SetVendorProcessed;

public interface ISetVendorProcessedUseCase
{
    Task ExecuteAsync(VendorProcessed vendorProcessed, CancellationToken cancellationToken = default);
}
