using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DataModel;

/// <summary>
/// Represents a user of the system.
/// </summary>
public class Author : IdentityUser<int>
{
    public int AuthorId
    {
        get => Id;
        set => Id = value;
    }

    public required string Name { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}