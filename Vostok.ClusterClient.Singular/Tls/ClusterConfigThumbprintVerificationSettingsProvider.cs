using System.Collections.Generic;
using Vostok.ClusterConfig.Client.Abstractions;
using Vostok.Configuration;
using Vostok.Configuration.Sources.ClusterConfig;
using Vostok.Configuration.Sources.Json;
using Vostok.Singular.Core.Configuration;
using Vostok.Singular.Core.Tls;

namespace Vostok.Clusterclient.Singular.Tls
{
    internal class ClusterConfigThumbprintVerificationSettingsProvider : IThumbprintVerificationSettingsProvider
    {
        private readonly ConfigurationProvider provider;

        public ClusterConfigThumbprintVerificationSettingsProvider(IClusterConfigClient clusterConfigClient, ClusterConfigPath thumbprintsPath)
        {
            provider = new ConfigurationProvider();
            provider.SetupSourceFor<SingularSettings.TlsClientSettings>(CreateSource(clusterConfigClient, thumbprintsPath));
        }

        public IList<string> GetWhitelist()
        {
            return GetTlsSettings().CertificateThumbprintsWhitelist;
        }

        public IList<string> GetBlacklist()
        {
            return GetTlsSettings().CertificateThumbprintsBlacklist;
        }

        private SingularSettings.TlsClientSettings GetTlsSettings()
        {
            return provider.Get<SingularSettings.TlsClientSettings>();
        }

        private static ClusterConfigSource CreateSource(IClusterConfigClient client, ClusterConfigPath path)
        {
            return new ClusterConfigSource(new ClusterConfigSourceSettings(client, path.ToString())
            {
                ValuesParser = (value, _) => JsonConfigurationParser.Parse(value)
            });
        }
    }
}