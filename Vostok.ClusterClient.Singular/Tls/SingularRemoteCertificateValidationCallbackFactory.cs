using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;

namespace Vostok.Clusterclient.Singular.Tls;

/// <summary>
/// A factory that provides <see cref="RemoteCertificateValidationCallback"/> that trusts to all Singular replicas.
/// </summary>
[PublicAPI]
public static class SingularRemoteCertificateValidationCallbackFactory
{
    public static RemoteCertificateValidationCallback Create()
    {
        return TlsHandshakeValidatorProvider.Get().Verify;
    }

    public static Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> CreateAsFunc()
    {
        return TlsHandshakeValidatorProvider.Get().Verify;
    }
}