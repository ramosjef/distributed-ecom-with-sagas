using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class AuthorizePaymentFailed : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}
