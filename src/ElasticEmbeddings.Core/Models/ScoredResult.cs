namespace ElasticEmbeddings.Models;

public class ScoredResult
{
    public required Guid Id { get; init; }
    public required float Score { get; init; }
}