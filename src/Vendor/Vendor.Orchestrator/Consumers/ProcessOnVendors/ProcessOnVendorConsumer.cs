using System.Threading.Tasks;

using MassTransit;

using Microsoft.Extensions.Logging;

using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Vendor.Orchestrator.Consumers.ProcessOnVendors
{
    public class ProcessOnVendorConsumer : IConsumer<ProcessOnVendor>
    {
        private readonly ILogger<ProcessOnVendorConsumer> _logger;

        public ProcessOnVendorConsumer(ILogger<ProcessOnVendorConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProcessOnVendor> context)
        {
            _logger.LogInformation($"Pedido {context.Message.CorrelationId} integrado no parceiro");

            VendorProcessed @event = new() { CorrelationId = context.Message.CorrelationId };
            await context.Publish(@event);
        }
    }
}
