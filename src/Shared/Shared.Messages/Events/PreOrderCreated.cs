using System;

using MassTransit;

namespace Shared.Messages.Events
{
    public sealed class PreOrderCreated : CorrelatedBy<Guid>
    {
        public DateTime CreatedDateTime { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
