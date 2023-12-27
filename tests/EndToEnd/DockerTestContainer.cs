using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace EndToEnd;

public class DockerTestContainer
{
    private const string Image = "elasticsearch:8.11.1";
    private const int Port = 9200;

    private readonly IContainer _container;
    
    public DockerTestContainer()
    {
        _container = new ContainerBuilder()
            .WithImage(Image)
            .WithPortBinding(Port, assignRandomHostPort: true)
            .WithEnvironment("ELASTIC_PASSWORD","ELASTIC_PASSWORD")
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r
                        .ForPort(Port)
                        .ForPath("/_cat/health")
                        .UsingTls()
                        .UsingHttpMessageHandler(new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        })
                        .WithBasicAuthentication("elastic", "ELASTIC_PASSWORD")))
            .Build();
    }
    
    public Task StartAsync()
    {
        return _container.StartAsync();
    }
    
    public int GetMappedPort()
    {
        return _container.GetMappedPublicPort(Port);
    }
    
    public Task StopAsync()
    {
        return _container.StopAsync();
    }
}