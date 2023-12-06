namespace ElasticEmbeddings.Search;

public static class Constants
{
    public const string IndexName = "documents";
    public const int IngestionBatchSize = 1000;
    public const long K = 20;
    public const float MinRelevanceScore = 0.8f;
    public const string Similarity = "cosine";
    public const int EmbeddingDimensions = 1536;
}