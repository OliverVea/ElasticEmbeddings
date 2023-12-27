using System.Text.Json;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace EndToEnd;

public class ElasticSearchTestContainer
{
    private const string Image = "elasticsearch:8.11.1";
    private const int Port = 9200;
    private const string Username = "elastic";
    private const string PasswordKey = "ELASTIC_PASSWORD";
    private const string Password = "ELASTIC_PASSWORD";
    private const string HealthEndpoint = "/_cluster/health";
    private const bool AssignRandomHostPort = true;
    

    private readonly IContainer _container;
    
    public ElasticSearchTestContainer()
    {
        _container = new ContainerBuilder()
            .WithImage(Image)
            .WithPortBinding(Port, AssignRandomHostPort)
            .WithEnvironment(PasswordKey,Password)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r
                        .ForPort(Port)
                        .ForPath(HealthEndpoint)
                        .UsingTls()
                        .IgnoreServerCertificateValidation()
                        .WithMethod(HttpMethod.Get)
                        .ForResponseMessageMatching(async message =>
                        {
                            var content = await message.Content.ReadAsStringAsync();

                            try
                            {
                                var statusResponse = JsonSerializer.Deserialize<StatusResponse>(content, new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });
                                return statusResponse is { Status: "green" };
                            }
                            catch (Exception)
                            {
                                // ignored
                            }

                            return false;
                        })
                        .WithBasicAuthentication(Username, Password)))
            .Build();
    }

    private class StatusResponse
    {
        public required string Status { get; init; }
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