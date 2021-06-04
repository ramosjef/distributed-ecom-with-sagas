using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class ProcessOnVendorFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}
