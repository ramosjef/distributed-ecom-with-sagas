namespace Payment.Worker.Consumers.ConfirmPayments;

public class ConfirmPaymentConsumerDefinition : ConsumerDefinition<ConfirmPaymentConsumer>
{
    public ConfirmPaymentConsumerDefinition()
    {
        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<ConfirmPaymentConsumer> consumerConfigurator)
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox();
    }
}
