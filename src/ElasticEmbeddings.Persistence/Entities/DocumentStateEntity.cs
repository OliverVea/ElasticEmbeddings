using System.ComponentModel.DataAnnotations;
using ElasticEmbeddings.Models;

namespace ElasticEmbeddings.Persistence.Entities;

internal class DocumentStateEntity
{
    [Required]
    [Key]
    public long Id { get; set; }
    
    [Required]
    public required DocumentState State { get; set; }
    
    
    [Required]
    public long DocumentId { get; init; }
    public DocumentEntity Document { get; init; } = null!;
}