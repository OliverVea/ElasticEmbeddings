using System.ComponentModel.DataAnnotations;

namespace ElasticEmbeddings.Persistence.Entities;

internal class DocumentEmbeddingEntity
{
    [Required]
    [Key]
    public long Id { get; set; }
    
    [Required]
    [MaxLength(9999)]
    public required float[] Embeddings { get; init; }
    
    [Required]
    public long DocumentId { get; init; }
    public DocumentEntity Document { get; init; } = null!;
}