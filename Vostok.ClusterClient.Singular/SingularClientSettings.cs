﻿using System;
using JetBrains.Annotations;
using Vostok.Clusterclient.Core.Topology;
using Vostok.ClusterConfig.Client;
using Vostok.Context;
using Vostok.Metrics;
using Vostok.Singular.Core;

#nullable enable

namespace Vostok.Clusterclient.Singular
{
    [PublicAPI]
    public class SingularClientSettings
    {
        private string? explicitTargetEnvironment;

        /// <param name="environmentName">See <see cref="TargetEnvironment" />.</param>
        /// <param name="serviceName">See <see cref="TargetService" />.</param>
        public SingularClientSettings(string environmentName, string serviceName)
        {
            explicitTargetEnvironment = environmentName ?? throw new ArgumentNullException(nameof(environmentName));
            TargetService = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
        }

        /// <param name="serviceName">See <see cref="TargetService" />.</param>
        public SingularClientSettings(string serviceName)
        {
            TargetService = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
        }

        /// <summary>
        /// Target environment in service discovery system to send requests to.
        /// </summary>
        public string TargetEnvironment => explicitTargetEnvironment
                                           ?? FlowingContext.Properties.Get<string>(SingularConstants.DistributedProperties.ForcedEnvironment)
                                           ?? ClusterConfigClient.Default.Zone
                                           ?? SingularConstants.DefaultZone;

        /// <summary>
        /// Target service to send requests to. This value must exactly match the name that service uses to register in service discovery system.
        /// </summary>
        public string TargetService { get; }

        /// <summary>
        /// Explicitly setting a custom <see cref="IClusterProvider"/> prevents the client from using Singular topology specified in ClusterConfig.
        /// </summary>
        public IClusterProvider? AlternativeClusterProvider { get; set; }

        /// <summary>
        /// Explicitly setting a <see cref="IMetricContext"/> allows the client from writing metrics about quality.
        /// </summary>
        public IMetricContext? MetricContext { get; set; }
    }
}