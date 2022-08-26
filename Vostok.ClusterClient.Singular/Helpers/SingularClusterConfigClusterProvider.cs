using System;
using System.Collections.Generic;
using System.Linq;
using Vostok.Clusterclient.Core.Topology;
using Vostok.Clusterclient.Topology.CC;
using Vostok.Commons.Collections;

namespace Vostok.Clusterclient.Singular.Helpers;

internal class SingularClusterConfigClusterProvider : IClusterProvider
{
    private readonly ClusterConfigClusterProvider clusterConfigClusterProvider;
    private readonly CachingTransform<IList<Uri>, Uri[]> transform;

    public SingularClusterConfigClusterProvider(ClusterConfigClusterProvider clusterConfigClusterProvider)
    {
        this.clusterConfigClusterProvider = clusterConfigClusterProvider;
        transform = new CachingTransform<IList<Uri>, Uri[]>(cluster => cluster.Select(CutSchemeAndPort).ToArray());
    }

    public IList<Uri> GetCluster() => transform.Get(clusterConfigClusterProvider.GetCluster());

    private static Uri CutSchemeAndPort(Uri uri)
    {
        var cleanUri = uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port & ~UriComponents.Scheme, UriFormat.UriEscaped);
        return new Uri(cleanUri);
    }
}