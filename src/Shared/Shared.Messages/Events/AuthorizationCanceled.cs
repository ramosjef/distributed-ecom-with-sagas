using System;

using MassTransit;

namespace Shared.Messages.Events;

public sealed class AuthorizationCanceled : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
    public long OrderId { get; set; }
}
