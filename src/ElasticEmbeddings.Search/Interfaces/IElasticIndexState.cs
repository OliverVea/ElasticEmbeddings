using System.Collections.Concurrent;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Interfaces;

public interface IElasticIndexState
{
    ConcurrentQueue<Document> Documents { get; }
}