using System;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Ordering;
using Vostok.Clusterclient.Core.Ordering.Weighed;
using Vostok.Clusterclient.Core.Retry;
using Vostok.Clusterclient.Core.Strategies;
using Vostok.Clusterclient.Core.Topology;
using Vostok.Clusterclient.Core.Transforms;
using Vostok.ClusterClient.Datacenters;
using Vostok.Clusterclient.Topology.CC;
using Vostok.ClusterConfig.Client;
using Vostok.Datacenters;

namespace Vostok.Clusterclient.Singular
{
    [PublicAPI]
    public static class IClusterClientConfigurationExtensions
    {
        /// <summary>
        /// Sets up given ClusterClient configuration to send requests via Singular API gateway according to given <paramref name="settings"/>.
        /// </summary>
        public static void SetupSingular([NotNull] this IClusterClientConfiguration self, [NotNull] SingularClientSettings settings)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (settings.AlternativeClusterProvider != null)
            {
                self.ClusterProvider = settings.AlternativeClusterProvider;
            }
            else
            {
                self.SetupClusterConfigTopology(settings.ClusterConfigClient ?? ClusterConfigClient.Default, settings.ClusterConfigTopologyPath);
            }

            self.RequestTransforms.Add(
                new AdHocRequestTransform(
                    request => request
                        .WithHeader(SingularConstants.EnvironmentHeader, settings.TargetEnvironment)
                        .WithHeader(SingularConstants.ServiceHeader, settings.TargetService)));

            self.SetupWeighedReplicaOrdering(
                builder =>
                {
                    builder.AddAdaptiveHealthModifierWithLinearDecay(TimeSpan.FromMinutes(5));
                    builder.SetupAvoidInactiveDatacentersWeightModifier(DatacentersProvider.Get());
                    builder.SetupBoostLocalDatacentersWeightModifier(DatacentersProvider.Get());
                });

            self.DefaultRequestStrategy = Strategy.Sequential1;

            self.MaxReplicasUsedPerRequest = 3;

            self.TargetServiceName = $"{settings.TargetService} via {SingularConstants.SingularServiceName}";
            self.TargetEnvironment = settings.TargetEnvironment;
        }

        /// <summary>
        /// Sets up given ClusterClient configuration to send requests via Nginx -> Singular on given URL <paramref name="baseUrl"/>.
        /// </summary>
        public static void SetupSingularExternal([NotNull] this IClusterClientConfiguration self, [NotNull] string baseUrl)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            self.ClusterProvider = new FixedClusterProvider(baseUrl);
            self.ReplicaOrdering = new AsIsReplicaOrdering();

            self.RetryPolicy = new AdHocRetryPolicy((request, requestParams, results)
                => results.Count==0 || results.All(x => x.Verdict != ResponseVerdict.Accept));
            self.RetryStrategy = new ImmediateRetryStrategy(3);

            self.DefaultRequestStrategy = Strategy.Sequential1;
            self.TargetServiceName = $"{baseUrl} via {SingularConstants.SingularServiceName}";
        }
    }
}