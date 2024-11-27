namespace Chirp.Core.DataModel;

/// <summary>
/// Represents a Cheep, a short message posted by an Author.
/// </summary>
public abstract class Cheep
{
    public const int MaxLength = 160;
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    public required DateTime TimeStamp { get; set; }

    public abstract string GetText();
}