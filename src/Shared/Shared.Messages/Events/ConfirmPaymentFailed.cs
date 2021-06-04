using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class ConfirmPaymentFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}
