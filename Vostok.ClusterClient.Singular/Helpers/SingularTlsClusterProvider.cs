using System;
using System.Collections.Generic;
using System.Linq;
using Vostok.Clusterclient.Core.Topology;
using Vostok.Commons.Collections;

namespace Vostok.Clusterclient.Singular.Helpers;

internal class SingularTlsClusterProvider : IClusterProvider
{
    private readonly IClusterProvider clusterConfigClusterProvider;
    private readonly CachingTransform<IList<Uri>, Uri[]> transform;

    public SingularTlsClusterProvider(IClusterProvider clusterConfigClusterProvider)
    {
        this.clusterConfigClusterProvider = clusterConfigClusterProvider;
        transform = new CachingTransform<IList<Uri>, Uri[]>(cluster => cluster.Select(ReplaceSchemeAndPort).ToArray());
    }

    public IList<Uri> GetCluster() => transform.Get(clusterConfigClusterProvider.GetCluster());

    private static Uri ReplaceSchemeAndPort(Uri uri)
    {
        return new UriBuilder(uri)
        {
            Scheme = Uri.UriSchemeHttps,
            Port = 443
        }.Uri;
    }
}