using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.DataModel;

/// <summary>
///     Represents a user of the system.
/// </summary>
public class Author : IdentityUser<int>
{
    [NotMapped]
    public int AuthorId
    {
        get => Id;
        set => Id = value;
    }

    [PersonalData] public required string DisplayName { get; set; }

    [PersonalData] public new required string Email { get; set; }
    [PersonalData] public List<Cheep> Cheeps { get; set; } = [];

    [PersonalData] public List<Author> Following { get; set; } = [];
}