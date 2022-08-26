using System;
using Vostok.Logging.Abstractions;
using Vostok.Singular.Core.Tls;

namespace Vostok.Clusterclient.Singular.Tls;

internal class TlsHandshakeValidatorProvider
{
    private static readonly Lazy<ITlsHandshakeValidator> Validator = new(Create);

    public static ITlsHandshakeValidator Get()
    {
        return Validator.Value;
    }

    private static ITlsHandshakeValidator Create()
    {
        var log = LogProvider.Get();
        var thumbprintsProvider = ClusterConfigThumbprintVerificationSettingsProvider.Default;
        var certificateVerifier = new ThumbprintCertificateChainVerifier(thumbprintsProvider);
        return new SingularHandshakeValidator(
            certificateVerifier,
            log
        );
    }
}