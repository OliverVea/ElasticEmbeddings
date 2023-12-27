using AutoFixture;
using ElasticEmbeddings.Interfaces;
using ElasticEmbeddings.Models;
using NUnit.Framework;

namespace EndToEnd;

public class SearchTest : BaseTest
{
    private IDocumentService DocumentService => GetService<IDocumentService>();
    private IDocumentSearchService DocumentSearchService => GetService<IDocumentSearchService>();
    private IDocumentProcessingService DocumentProcessingService => GetService<IDocumentProcessingService>();
    private ElasticEmbeddings.Search.Domain.IDocumentIndexingService DocumentIndexingService => GetService<ElasticEmbeddings.Search.Domain.IDocumentIndexingService>();
    
    [Test]
    public async Task SearchAsync_WithThreeBooks_ReturnsBooksInOrderOfRelevance()
    {
        // Arrange
        const string searchQuery = "Harry Potter";
        
        const string mostRelevantTitle = "Harry Potter and the Philosopher's Stone";
        const string lessRelevantDocument = "The Lord of the Rings";
        const string leastRelevantDocument = "Origins of the Species";

        await CreateBooksWithTitles(mostRelevantTitle, lessRelevantDocument, leastRelevantDocument);
        
        // Act
        var result = await DocumentSearchService.SearchAsync(new DocumentSearchRequest
        {
            Query = searchQuery
        });

        // Assert
        var actualDocumentTitles = result.Documents.Select(x => x.Document.Title);
        var expectedDocumentTitles = new[] { mostRelevantTitle, lessRelevantDocument, leastRelevantDocument };
        Assert.That(actualDocumentTitles, Is.EquivalentTo(expectedDocumentTitles));
        
        var actualDocumentScores = result.Documents.Select(x => x.Score);
        Assert.That(actualDocumentScores, Is.Ordered.Descending);
    }
    
    private async Task CreateBooksWithTitles(params string[] titles)
    {
        foreach (var title in titles)
        {
            var document = DataBuilder.Document()
                .With(x => x.Title, title)
                .Create();
            
            await DocumentService.SetAsync(document, CancellationToken.None);
        }

        await DocumentProcessingService.ProcessDocumentBatchAsync(CancellationToken.None);
        await DocumentIndexingService.ExecuteAsync(CancellationToken.None);
    }
}