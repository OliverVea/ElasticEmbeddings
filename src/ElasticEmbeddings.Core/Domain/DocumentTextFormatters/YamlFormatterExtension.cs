using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Domain.DocumentTextFormatters;

public partial class YamlFormatter
{
    public required Document Document { get; init; }

    private bool HasText => !string.IsNullOrWhiteSpace(Document.Text);
}