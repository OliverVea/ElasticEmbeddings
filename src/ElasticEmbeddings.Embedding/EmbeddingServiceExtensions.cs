using Azure;
using Azure.AI.OpenAI;
using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Interfaces.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticEmbeddings.Embedding;

public static class EmbeddingServiceExtensions
{
    public static void AddEmbedding(this IServiceCollection services, OpenAIConfiguration configuration)
    {
        AddOpenAi(services, configuration);
        
        services.AddSingleton<ITextEmbeddingProvider, TextEmbeddingProvider>();
    }

    private static void AddOpenAi(IServiceCollection services, OpenAIConfiguration configuration)
    {
        var endpoint = new Uri(configuration.Endpoint);
        var credential = new AzureKeyCredential(configuration.ApiKey);
        
        var client = new OpenAIClient(endpoint, credential);
        
        services.AddSingleton(configuration);
        services.AddSingleton(client);
    }
}