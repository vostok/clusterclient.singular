using JetBrains.Annotations;
using Vostok.Clusterclient.Singular.Tls;
using Vostok.Clusterclient.Transport;
using Vostok.ClusterConfig.Client;
using Vostok.Logging.Abstractions;
using Vostok.Singular.Core;
using Vostok.Singular.Core.Tls;

namespace Vostok.Clusterclient.Singular.Helpers;

[PublicAPI]
public static class TransportSettingsExtensions
{
    /// <summary>
    /// Appends <see cref="UniversalTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static UniversalTransportSettings WithSingularTlsHandshakeValidator(this UniversalTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    /// <summary>
    /// Appends <see cref="SocketsTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static SocketsTransportSettings WithSingularTlsHandshakeValidator(this SocketsTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    /// <summary>
    /// Appends <see cref="WebRequestTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static WebRequestTransportSettings WithSingularTlsHandshakeValidator(this WebRequestTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    /// <summary>
    /// Appends <see cref="NativeTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static NativeTransportSettings WithSingularTlsHandshakeValidator(this NativeTransportSettings settings, ILog log)
    {
        settings.RemoteCertificateValidationCallback = GetValidator(log).Verify;
        return settings;
    }

    private static ITlsHandshakeValidator GetValidator(ILog log)
    {
        var thumbprintsProvider = new ClusterConfigThumbprintVerificationSettingsProvider(
            ClusterConfigClient.Default,
            SingularConstants.CCTlsSettingsName
        );
        var certificateVerifier = new ThumbprintCertificateChainVerifier(thumbprintsProvider);
        return new SingularHandshakeValidator(
            certificateVerifier,
            log
        );
    }
}