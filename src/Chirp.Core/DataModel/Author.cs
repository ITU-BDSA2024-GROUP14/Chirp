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
    
    [PersonalData]
    public required string Beak { get; set; }
    
    public new required string Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }

    public HashSet<Author> Following { get; set; } = [];
}