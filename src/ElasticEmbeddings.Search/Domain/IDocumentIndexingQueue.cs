using System.Collections.Concurrent;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Domain;

public interface IDocumentIndexingQueue
{
    ConcurrentQueue<Document> Documents { get; }
}