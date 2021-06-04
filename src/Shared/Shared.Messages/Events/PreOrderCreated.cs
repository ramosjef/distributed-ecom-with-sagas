using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class PreOrderCreated : CorrelatedBy<Guid>
{
    public long OrderId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public Guid CorrelationId { get; set; }
}
