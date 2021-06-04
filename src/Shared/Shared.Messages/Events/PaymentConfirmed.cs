using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class PaymentConfirmed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
    public long OrderId { get; set; }
}
