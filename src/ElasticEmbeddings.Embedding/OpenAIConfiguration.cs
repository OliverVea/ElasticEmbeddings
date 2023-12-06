namespace ElasticEmbeddings.Embedding;

public class OpenAIConfiguration
{
    public required string Deployment { get; init; }
    public required string ApiKey { get; init; }
    public required string Endpoint { get; init; }
}