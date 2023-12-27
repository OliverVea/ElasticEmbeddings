using System.Collections.Concurrent;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Domain;

public class DocumentIndexingQueue : IDocumentIndexingQueue
{
    public ConcurrentQueue<Document> Documents { get; } = new();
}