using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Sending;
using Vostok.Clusterclient.Core.Strategies;
using Vostok.Clusterclient.Singular.Helpers;
using Vostok.Singular.Core.PathPatterns.Extensions;
using Vostok.Singular.Core.PathPatterns.Timeout;

namespace Vostok.Clusterclient.Singular.Strategies
{
    internal class TimeoutStrategy : IRequestStrategy
    {
        private readonly IRequestStrategy requestStrategy;
        private readonly TimeoutSettingsResolver timeoutSettingsResolver;

        public TimeoutStrategy(IRequestStrategy requestStrategy, TimeoutSettingsResolver timeoutSettingsResolver)
        {
            this.requestStrategy = requestStrategy;
            this.timeoutSettingsResolver = timeoutSettingsResolver;
        }

        public async Task SendAsync(
            Request request,
            RequestParameters parameters,
            IRequestSender sender,
            IRequestTimeBudget budget,
            IEnumerable<Uri> replicas,
            int replicasCount,
            CancellationToken cancellationToken)
        {
            if (budget.Total != ClusterClientDefaults.Timeout)
            {
                await requestStrategy.SendAsync(request, parameters, sender, budget, replicas, replicasCount, cancellationToken).ConfigureAwait(false);
                return;
            }

            var timeout = await timeoutSettingsResolver.Get(request.Method, request.Url.GetRequestPath()).ConfigureAwait(false);

            if (!timeout.HasValue)
            {
                await requestStrategy.SendAsync(request, parameters, sender, budget, replicas, replicasCount, cancellationToken).ConfigureAwait(false);
                return;
            }

            var newBudget = RequestTimeBudget.StartNew(timeout.Value);

            await requestStrategy.SendAsync(request, parameters, sender, newBudget, replicas, replicasCount, cancellationToken).ConfigureAwait(false);
        }
    }
}