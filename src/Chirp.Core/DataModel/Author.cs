namespace Chirp.Core.DataModel;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a user of the system.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}