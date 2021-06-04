using System;

using Shared.Core.Extensions;

namespace Shared.Core.Utils
{
    public static class QueueName
    {
        public static Uri ToUri(string queueName) => new($"queue:{queueName.ToKebabCase()}");
    }
}
