using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DataModel;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a user of the system.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Author : IdentityUser<int>
{
    public int AuthorId
    {
        get => Id;
        set => Id = value;
    }

    public required string Name { get; set; }
    public new required string Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}