using DotNet.Testcontainers.Configurations;

namespace EndToEnd;

public static class HttpWaitStrategyExtensions
{
    public static HttpWaitStrategy IgnoreServerCertificateValidation(this HttpWaitStrategy waitStrategy)
    {
        return waitStrategy.UsingHttpMessageHandler(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    }
    
}