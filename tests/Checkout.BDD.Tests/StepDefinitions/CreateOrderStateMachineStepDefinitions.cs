using System.Diagnostics.CodeAnalysis;

using Checkout.Application.UseCases;
using Checkout.Domain.Orders;
using Checkout.Infrastructure.DataAccess.InMemory.Repositories;
using Checkout.Orchestrator.Sagas.CreateOrder;

using Microsoft.Extensions.DependencyInjection;

using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Checkout.BDD.Tests.StepDefinitions;

[Binding, ExcludeFromCodeCoverage]
public sealed class CreateOrderStateMachineStepDefinitions
{
    private ServiceProvider? _provider;
    private ITestHarness? _harness;
    private ISagaStateMachineTestHarness<CreateOrderStateMachine, CreateOrderState>? _sagaHarness;
    private Guid _correlationId = Guid.Empty;
    private long _orderId = 0;

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        _provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddSagaStateMachine<CreateOrderStateMachine, CreateOrderState>();
                x.AddUseCases();
                x.AddSingleton<IOrderRepository>(sp => new OrderRepository());
            })
            .BuildServiceProvider(true);

        _harness = _provider.GetTestHarness();

        await _harness.Start();

        _sagaHarness = _harness!.GetSagaStateMachineHarness<CreateOrderStateMachine, CreateOrderState>();
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        await _harness.Stop();
        await _provider!.DisposeAsync();
    }

    [Given(@"CorrelationId ""([^""]*)""")]
    public void GivenCorrelationId(string correlationId) => _correlationId = Guid.Parse(correlationId);

    [Given(@"OrderId (.*)")]
    public async Task GivenOrderId(int orderId)
    {
        _orderId = orderId;

        Order order = Order.InitPreOrder();
        order.Id = orderId;

        var repo = _provider!.GetRequiredService<IOrderRepository>();
        await repo.CreateAsync(order);
    }

    [When(@"PreOrderCreated")]
    public async Task WhenPreOrderCreated() => await _harness!.Bus.Publish(new PreOrderCreated
    {
        CorrelationId = _correlationId,
        OrderId = _orderId
    });

    [When(@"PaymentAuthorized")]
    public async Task WhenPaymentAuthorized() => await _harness!.Bus.Publish(new PaymentAuthorized
    {
        CorrelationId = _correlationId,
        OrderId = _orderId
    });

    [When(@"VendorProcessed")]
    public async Task WhenVendorProcessed()
    {
        // Set CurrentState to ProcessingOnVendor
        await _harness!.Bus.Publish(new PaymentAuthorized
        {
            CorrelationId = _correlationId,
            OrderId = _orderId
        });

        await _harness!.Bus.Publish(new VendorProcessed
        {
            CorrelationId = _correlationId,
            OrderId = _orderId
        });
    }

    [When(@"PaymentConfirmed")]
    public async Task WhenPaymentConfirmed()
    {
        // Set CurrentState to ProcessingOnVendor
        await _harness!.Bus.Publish(new PaymentAuthorized
        {
            CorrelationId = _correlationId,
            OrderId = _orderId
        });

        // Set CurrentState to ConfirmingPayment
        await _harness!.Bus.Publish(new VendorProcessed
        {
            CorrelationId = _correlationId,
            OrderId = _orderId
        });

        await _harness!.Bus.Publish(new PaymentConfirmed
        {
            CorrelationId = _correlationId,
            OrderId = _orderId
        });
    }

    [Then(@"OrderRepository Has Order With Id (.*) And With Status ""([^""]*)""")]
    public async Task ThenOrderRepositoryHasOrderWithIdAndWithStatus(long orderId, string orderStatus)
    {
        Order order = await _provider!.GetRequiredService<IOrderRepository>().GetAsync(orderId);

        Assert.NotNull(order);
        Assert.Equal(orderStatus, $"{order.Status}");
    }

    [Then(@"Consume PreOrderCreated")]
    public async Task ThenConsumePreOrderCreated()
    {
        await _harness.Stop();
        Assert.True(await _harness!.Consumed.Any<PreOrderCreated>());
    }

    [Then(@"Consume PaymentAuthorized")]
    public async Task ThenConsumePaymentAuthorized()
    {
        await _harness.Stop();
        Assert.True(await _sagaHarness!.Consumed.Any<PaymentAuthorized>());
    }

    [Then(@"Consume VendorProcessed")]
    public async Task ThenConsumeVendorProcessed()
    {
        await _harness.Stop();
        Assert.True(await _sagaHarness!.Consumed.Any<VendorProcessed>());
    }

    [Then(@"Consume PaymentConfirmed")]
    public async Task ThenConsumePaymentConfirmed()
    {
        await _harness.Stop();
        Assert.True(await _sagaHarness!.Consumed.Any<PaymentConfirmed>());
    }

    [Then(@"Publish AuthorizePayment")]
    public async Task ThenPublishAuthorizePayment() => Assert.True(await _harness!.Published.Any<AuthorizePayment>());

    [Then(@"Publish ProcessOnVendor")]
    public async Task ThenPublishProcessOnVendor() => Assert.True(await _harness!.Published.Any<ProcessOnVendor>());

    [Then(@"Publish ConfirmPayment")]
    public async Task ThenPublishConfirmPayment() => Assert.True(await _harness!.Published.Any<ConfirmPayment>());

    [Then(@"Saga current state is ""([^""]*)""")]
    public void ThenSagaCurrentStateIs(string sagaState)
    {
        CreateOrderState instance = _sagaHarness!.Created.ContainsInState(
            _correlationId,
            _sagaHarness.StateMachine,
            (machine) => sagaState switch
            {
                "ProcessingOnVendor" => machine.ProcessingOnVendor,
                "AuthorizingPayment" => machine.AuthorizingPayment,
                "ConfirmingPayment" => machine.ConfirmingPayment,
                "Final" => machine.Final,
                _ => throw new NotImplementedException(),
            });

        Assert.NotNull(instance);
        Assert.Equal(instance.CurrentState, sagaState);
    }

    [Then(@"Saga has same correlationId")]
    public async Task ThenSagaHasSameCorrelationId()
        => Assert.True(await _sagaHarness!.Created.Any(x => x.CorrelationId == _correlationId));
}
