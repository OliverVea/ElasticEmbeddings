using System.Collections.Concurrent;
using ElasticEmbeddings.Search.Interfaces;
using ElasticEmbeddings.Search.Models;

namespace ElasticEmbeddings.Search.Application;

public class ElasticIndexState : IElasticIndexState
{
    public ConcurrentQueue<Document> Documents { get; } = new();
}