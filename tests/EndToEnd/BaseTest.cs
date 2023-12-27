using ElasticEmbeddings;
using ElasticEmbeddings.Embedding;
using ElasticEmbeddings.Persistence;
using ElasticEmbeddings.Search;
using ElasticEmbeddings.Search.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace EndToEnd;

public abstract class BaseTest
{
    protected readonly DataBuilder DataBuilder = new();
    
    private IServiceProvider _services = null!;
    private readonly ElasticSearchTestContainer _elasticSearchTestContainer = new();

    [OneTimeSetUp]
    public async Task CreateServiceProvider()
    {
        await _elasticSearchTestContainer.StartAsync();
        
        var services = new ServiceCollection();
        
        var logger = new Logger("console", InternalTraceLevel.Debug, TestContext.Out);
        
        services.AddSingleton<ILogger>(logger);
        
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

        var elasticPort = _elasticSearchTestContainer.GetMappedPort();
        services.AddSearch(new ElasticsearchConfiguration
        {
            Endpoint = $"https://localhost:{elasticPort}",
            Username = "elastic",
            Password = "ELASTIC_PASSWORD"
        });

        services.AddLogging();

        _services = services.BuildServiceProvider();

        await _services.EnsureCreatedAsync();
    }
    
    [OneTimeTearDown]
    public async Task RemoveTestContainers()
    {
        await _elasticSearchTestContainer.StopAsync();
    }
    
    protected T GetService<T>() where T : notnull
    {
        return _services.GetRequiredService<T>();
    }
}