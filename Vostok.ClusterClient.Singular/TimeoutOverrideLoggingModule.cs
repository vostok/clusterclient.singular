#nullable enable
using System;
using System.Threading.Tasks;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Modules;
using Vostok.Logging.Abstractions;

namespace Vostok.Clusterclient.Singular
{
    internal class TimeoutOverrideLoggingModule : IRequestModule
    {
        private readonly ILog log;

        public TimeoutOverrideLoggingModule(ILog log)
        {
            this.log = log;
        }
        
        public async Task<ClusterResult> ExecuteAsync(IRequestContext context, Func<IRequestContext, Task<ClusterResult>> next)
        {
            log.Info("Client timeout has been overridden by Singular settings");

            return await next(context).ConfigureAwait(false);
        }
    }
}