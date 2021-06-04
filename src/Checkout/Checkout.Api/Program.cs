using Checkout.Api.Common;
using Checkout.Application.UseCases;
using Checkout.Infrastructure.DataAccess.Mongo;

using CorrelationId;
using CorrelationId.DependencyInjection;

using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();

builder.Services
    .AddSwagger()
    .AddUseCases()
    .AddMassTransit(builder.Configuration)
    .AddMongoRepositories(builder.Configuration)
    .AddLogging(cfg => cfg.AddConsole())
    .AddDefaultCorrelationId(x => x.AddToLoggingScope = true);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout.Api v1"));
}

app.UseCorrelationId();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
