using ElasticEmbeddings;
using ElasticEmbeddings.Embedding;
using ElasticEmbeddings.Persistence;
using ElasticEmbeddings.Search;
using ElasticEmbeddings.Search.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace EndToEnd;

public abstract class BaseTest
{
    protected readonly DockerTestContainer DockerTestContainer = new();
    protected readonly DataBuilder DataBuilder = new();
    protected IServiceProvider Services { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task CreateServiceProvider()
    {
        await DockerTestContainer.StartAsync();
        
        var services = new ServiceCollection();
        
        services.AddCore();

        var deployment = Environment.GetEnvironmentVariable("OpenAI__Deployment");
        var endpoint = Environment.GetEnvironmentVariable("OpenAI__Endpoint");
        var apiKey = Environment.GetEnvironmentVariable("OpenAI__ApiKey");
        
        services.AddEmbedding(new OpenAIConfiguration
        {
             Deployment = deployment ?? throw new InvalidOperationException(),
             Endpoint = endpoint ?? throw new InvalidOperationException(),
             ApiKey = apiKey ?? throw new InvalidOperationException()
        });

        var sqliteConnection = new SqliteConnection("DataSource=Search;Filename=:memory:");
        services.AddPersistence(sqliteConnection);


        var elasticPort = DockerTestContainer.GetMappedPort();
        services.AddSearch(new ElasticsearchConfiguration
        {
            Endpoint = $"http://localhost:{elasticPort}",
            Username = "elastic",
            Password = "ELASTIC_PASSWORD"
        });

        services.AddLogging();

        Services = services.BuildServiceProvider();

        await Services.EnsureCreatedAsync(sqliteConnection);
    }
    
    [OneTimeTearDown]
    public async Task RemoveTestContainers()
    {
        await DockerTestContainer.StopAsync();
    }
    
    protected T GetService<T>() where T : notnull
    {
        return Services.GetRequiredService<T>();
    }
}