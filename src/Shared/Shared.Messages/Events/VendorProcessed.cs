using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class VendorProcessed : CorrelatedBy<Guid>
{
    public long OrderId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public Guid CorrelationId { get; set; }
}
