using System;
using MassTransit;

using Microsoft.Extensions.Logging;

namespace Checkout.Orchestrator.Extensions
{
    public static class MassTransitExtensions
    {
        public static void LogInfo<TType>(this BehaviorContext<TType> context, string message, params object[] args) where TType : class, ISaga
            => context.Log(LogLevel.Information, message, args);

        private static void Log<TType>(this BehaviorContext<TType> context,
                                       LogLevel logLevel,
                                       string message,
                                       params object[] args) where TType : class, ISaga
        {
            if (context.TryGetPayload(out IServiceProvider serviceProvider))
            {
                var logger = (ILogger<TType>)serviceProvider.GetService(typeof(ILogger<TType>));
                logger.Log(logLevel, message, args);
            }
        }
    }
}