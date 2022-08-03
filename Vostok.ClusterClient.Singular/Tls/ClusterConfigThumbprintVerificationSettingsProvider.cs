using System;
using System.Collections.Generic;
using Vostok.ClusterConfig.Client.Abstractions;
using Vostok.Singular.Core.Tls;

namespace Vostok.Clusterclient.Singular.Tls
{
    internal class ClusterConfigThumbprintVerificationSettingsProvider : IThumbprintVerificationSettingsProvider
    {
        private readonly IClusterConfigClient clusterConfigClient;
        private readonly ClusterConfigPath thumbprintsPath;

        public ClusterConfigThumbprintVerificationSettingsProvider(IClusterConfigClient clusterConfigClient, ClusterConfigPath thumbprintsPath)
        {
            this.clusterConfigClient = clusterConfigClient;
            this.thumbprintsPath = thumbprintsPath;
        }

        public IEnumerable<string> GetWhitelist()
        {
            throw new NotImplementedException();
        }

        public bool AllowAnyThumbprintExceptBlacklisted => throw new NotImplementedException();

        public IEnumerable<string> GetBlacklist()
        {
            throw new NotImplementedException();
        }
    }
}