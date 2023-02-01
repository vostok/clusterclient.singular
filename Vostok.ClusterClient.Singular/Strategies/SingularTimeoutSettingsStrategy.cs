using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Sending;
using Vostok.Clusterclient.Core.Strategies;
using Vostok.Clusterclient.Singular.Helpers;
using Vostok.Singular.Core.PathPatterns.Extensions;
using Vostok.Singular.Core.PathPatterns.Timeout;

namespace Vostok.Clusterclient.Singular.Strategies
{
    internal class SingularTimeoutSettingsStrategy : IRequestStrategy
    {
        private readonly IRequestStrategy requestStrategy;
        private readonly TimeoutSettingsProvider timeoutSettingsResolver;

        public SingularTimeoutSettingsStrategy(IRequestStrategy requestStrategy, TimeoutSettingsProvider timeoutSettingsResolver)
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
            var timeout = await timeoutSettingsResolver.Get(request.Method, request.Url.GetRequestPath()).ConfigureAwait(false);

            var newBudget = RequestTimeBudget.StartNew(timeout);

            await requestStrategy.SendAsync(request, parameters, sender, newBudget, replicas, replicasCount, cancellationToken).ConfigureAwait(false);
        }
    }
}