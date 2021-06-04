using System;

using MassTransit;

namespace Shared.Messages.Commands;

public sealed class ConfirmPayment : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
    public long OrderId { get; set; }
}
