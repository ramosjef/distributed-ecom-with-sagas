using System.Linq;

namespace Vendor.Worker.Consumers.ProcessOnVendors;

public class ProcessOnVendorConsumerDefinition : ConsumerDefinition<ProcessOnVendorConsumer>
{
    public ProcessOnVendorConsumerDefinition()
    {
        // override the default endpoint name
        //EndpointName = "process-on-vendor";

        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<ProcessOnVendorConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(Enumerable.Range(0, 1).Select(_ => TimeSpan.FromMinutes(1)).ToArray()));
        endpointConfigurator.UseInMemoryOutbox();
    }
}
