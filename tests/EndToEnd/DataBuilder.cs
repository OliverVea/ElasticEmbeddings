using AutoFixture;
using AutoFixture.Dsl;
using ElasticEmbeddings.Models;

namespace EndToEnd;

public class DataBuilder
{
    private Fixture Fixture { get; } = new();
    
    public IPostprocessComposer<Document> Document()
    {
        return Fixture.Build<Document>()
            .With(x => x.DocumentId, () => new DocumentId(Guid.NewGuid()))
            .With(x => x.Title, string.Empty)
            .With(x => x.Text, string.Empty);
    }
}