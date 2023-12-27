using Elastic.Clients.Elasticsearch;
using ElasticEmbeddings.Interfaces.Repositories;
using ElasticEmbeddings.Models;
using ElasticEmbeddings.Search.Domain;
using Document = ElasticEmbeddings.Search.Models.Document;

namespace ElasticEmbeddings.Search;

public class EmbeddingSearchRepository(
    ElasticsearchClient client,
    IDocumentMapper documentMapper,
    IDocumentIndexingQueue documentIndexingQueue) : IEmbeddingSearchRepository
{
    public Task IndexAsync(DocumentEmbedding documentEmbedding)
    {
        var document = documentMapper.Map(documentEmbedding);
        documentIndexingQueue.Documents.Enqueue(document);
        
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<DocumentResult>> SearchAsync(ElasticEmbeddings.Models.Embedding embedding)
    {
        var queryVector = embedding.Embeddings.ToArray();
        
        var result = await client.SearchAsync<Document>(r => r
                .Index(Constants.IndexName)
                .Knn(knn => knn
                    .QueryVector(queryVector)
                    .k(Constants.K)
                    .NumCandidates(Constants.K)
                    .Similarity(0)
                    .Field(doc => doc.Embedding))
                .Explain());
        
        if (!result.IsValidResponse) throw new Exception(result.DebugInformation);

        return documentMapper.Map(result.Hits);
    }

    public async Task<long?> CountAsync()
    {
        var indexName = Indices.Index(Constants.IndexName);
        
        var result = await client.CountAsync<Document>(x => x.Indices(indexName));

        if (!result.IsValidResponse) return null;

        return result.Count;
    }
}