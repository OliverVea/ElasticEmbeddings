using Azure.AI.OpenAI;
using ElasticEmbeddings.Interfaces.Providers;

namespace ElasticEmbeddings.Embedding;

internal class TextEmbeddingProvider(OpenAIClient client, OpenAIConfiguration configuration) : ITextEmbeddingProvider
{
    public async Task<Models.Embedding> GetEmbeddingAsync(string text)
    {
        var embeddingsOptions = new EmbeddingsOptions
        {
            DeploymentName = configuration.Deployment,
            Input = [ text ]
        };
        
        var response = await client.GetEmbeddingsAsync(embeddingsOptions);

        return Map(response.Value.Data.Single());
    }

    public async Task<IReadOnlyList<Models.Embedding>> GetEmbeddingsAsync(IReadOnlyList<string> texts)
    {
        if (!texts.Any()) return Array.Empty<Models.Embedding>();
            
        var embeddingsOptions = new EmbeddingsOptions
        {
            DeploymentName = configuration.Deployment,
            Input = texts.ToArray()
        };
        
        var response = await client.GetEmbeddingsAsync(embeddingsOptions);

        return response.Value.Data.Select(Map).ToArray();
    }

    private static Models.Embedding Map(EmbeddingItem embeddingItem)
    {
        return new Models.Embedding
        {
            Embeddings = embeddingItem.Embedding
        };
    }
}