using System;
using Vostok.ClusterConfig.Client;
using Vostok.Logging.Abstractions;
using Vostok.Singular.Core;
using Vostok.Singular.Core.Tls;

namespace Vostok.Clusterclient.Singular.Tls;

internal class TlsHandshakeValidatorProvider
{
    private static readonly Lazy<ITlsHandshakeValidator> Validator = new(CreateValidator);

    public static ITlsHandshakeValidator GetValidator()
    {
        return Validator.Value;
    }

    private static ITlsHandshakeValidator CreateValidator()
    {
        var log = LogProvider.Get();
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