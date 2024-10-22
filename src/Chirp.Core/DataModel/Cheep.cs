using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.DataModel;
/// <summary>
/// Represents a Cheep, a short message posted by an Author.
/// </summary>
public class Cheep
{
    public const int MaxLength = 160;
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    [MaxLength(MaxLength)] public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
}