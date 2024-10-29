using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DataModel;

/// <summary>
/// Represents a user of the system.
/// </summary>
public class Author : IdentityUser<int>
{
    [NotMapped]
    public int AuthorId
    {
        get => Id;
        set => Id = value;
    }

    public string? Name { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}