using JetBrains.Annotations;
using Vostok.Clusterclient.Singular.Tls;
using Vostok.Clusterclient.Transport;

namespace Vostok.Clusterclient.Singular.Helpers;

[PublicAPI]
public static class TransportSettingsExtensions
{
    /// <summary>
    /// Appends <see cref="UniversalTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static UniversalTransportSettings WithSingularTlsHandshakeValidator(this UniversalTransportSettings settings)
    {
        settings.RemoteCertificateValidationCallback = SingularRemoteCertificateValidationCallbackFactory.Create();
        return settings;
    }

    /// <summary>
    /// Appends <see cref="SocketsTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static SocketsTransportSettings WithSingularTlsHandshakeValidator(this SocketsTransportSettings settings)
    {
        settings.RemoteCertificateValidationCallback = SingularRemoteCertificateValidationCallbackFactory.Create();
        return settings;
    }

    /// <summary>
    /// Appends <see cref="WebRequestTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static WebRequestTransportSettings WithSingularTlsHandshakeValidator(this WebRequestTransportSettings settings)
    {
        settings.RemoteCertificateValidationCallback = SingularRemoteCertificateValidationCallbackFactory.Create();
        return settings;
    }

    /// <summary>
    /// Appends <see cref="NativeTransportSettings.RemoteCertificateValidationCallback"/> to correctly identify Singular replicas.
    /// </summary>
    public static NativeTransportSettings WithSingularTlsHandshakeValidator(this NativeTransportSettings settings)
    {
        settings.RemoteCertificateValidationCallback = SingularRemoteCertificateValidationCallbackFactory.CreateAsFunc();
        return settings;
    }
}