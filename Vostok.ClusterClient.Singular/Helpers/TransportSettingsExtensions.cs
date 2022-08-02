using Vostok.Clusterclient.Singular.Tls;
using Vostok.Clusterclient.Transport;
using Vostok.ClusterConfig.Client;
using Vostok.Logging.Abstractions;
using Vostok.Singular.Core;
using Vostok.Singular.Core.Tls;

namespace Vostok.Clusterclient.Singular.Helpers;

public static class TransportSettingsExtensions
{
    // todo: we should probably substitute default CCC with provided
    public static UniversalTransportSettings WithSingularTlsHandshakeValidator(this UniversalTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    public static SocketsTransportSettings WithSingularTlsHandshakeValidator(this SocketsTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    public static WebRequestTransportSettings WithSingularTlsHandshakeValidator(this WebRequestTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    public static NativeTransportSettings WithSingularTlsHandshakeValidator(this NativeTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    private static ITlsHandshakeValidator GetValidator(ILog log)
    {
        var thumbprintsProvider = new ClusterConfigThumbprintsProvider(
            ClusterConfigClient.Default,
            SingularConstants.CloudEnvironment
        );
        var certificateVerifier = new ThumbprintCertificateVerifier(thumbprintsProvider);
        return new SingularHandshakeValidator(
            certificateVerifier,
            log
        );
    }
}