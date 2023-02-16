#nullable enable
using System;
using System.Threading.Tasks;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Modules;
using Vostok.Singular.Core.PathPatterns.Extensions;
using Vostok.Singular.Core.PathPatterns.Timeout;

namespace Vostok.Clusterclient.Singular
{
    internal class SingularTimeBudgetModule : IRequestModule
    {
        private readonly TimeoutSettingsProvider timeoutSettingsProvider;

        public SingularTimeBudgetModule(TimeoutSettingsProvider timeoutSettingsProvider)
        {
            this.timeoutSettingsProvider = timeoutSettingsProvider;
        }
        
        public async Task<ClusterResult> ExecuteAsync(IRequestContext context, Func<IRequestContext, Task<ClusterResult>> next)
        {
            var timeout = await timeoutSettingsProvider.Get(context.Request.Method, context.Request.Url.GetRequestPath()).ConfigureAwait(false);

            context.Budget = RequestTimeBudget.StartNew(timeout);

            return await next(context).ConfigureAwait(false);
        }
    }
}