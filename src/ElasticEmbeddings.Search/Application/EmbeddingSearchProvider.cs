using Elastic.Clients.Elasticsearch;
using ElasticEmbeddings.Interfaces.Providers;
using ElasticEmbeddings.Models;
using ElasticEmbeddings.Search.Interfaces;
using Document = ElasticEmbeddings.Search.Models.Document;

namespace ElasticEmbeddings.Search.Application;

public class EmbeddingSearchProvider(
    ElasticsearchClient client,
    IDocumentMapper documentMapper,
    IElasticIndexState elasticIndexState) : IEmbeddingSearchProvider
{
    public Task IndexAsync(DocumentEmbedding documentEmbedding)
    {
        var document = documentMapper.Map(documentEmbedding);
        elasticIndexState.Documents.Enqueue(document);
        
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<DocumentResult>> SearchAsync(Embedding embedding)
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
}