using System;

using MassTransit;

namespace Shared.Messages.Commands
{
    public sealed class ProcessOnVendor : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}
