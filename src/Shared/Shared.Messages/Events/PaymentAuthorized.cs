using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class PaymentAuthorized : CorrelatedBy<Guid>
{
    public long OrderId { get; set; }
    public DateTime CreatedDateTime { get; private set; }
    public Guid CorrelationId { get; set; }
}
