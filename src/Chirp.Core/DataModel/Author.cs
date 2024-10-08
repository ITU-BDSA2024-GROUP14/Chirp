namespace Chirp.Core.DataModel;
/// <summary>
/// Represents a user of the system.
/// </summary>
public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}