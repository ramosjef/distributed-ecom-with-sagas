using System;

using MassTransit;

namespace Shared.Messages.Events
{
    public sealed class PaymentAuthorized : CorrelatedBy<Guid>
    {
        public DateTime CreatedDateTime { get; private set; }
        public Guid CorrelationId { get; set; }
    }
}
