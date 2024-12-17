using System.Diagnostics.CodeAnalysis;
using Chirp.Core.DataModel;

namespace Chirp.Infrastructure.Data.DataTransferObjects;
/// <summary>
/// This DataTransferObject represents an author without the fields we don't need in
/// the rest of the program from the Author class.
/// </summary>
public class AuthorDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }

    public required List<string> Following { get; set; }

    /// <summary>
    /// This constructor creates an AuthorDTO from an Author object.
    /// </summary>
    /// <param name="author">The author object to instantiate a AuthorDTO from</param>
    [SetsRequiredMembers]
    public AuthorDTO(Author author)
    {
        Name = author.DisplayName;
        Email = author.Email;
        Cheeps = [];

        foreach (var cheep in author.Cheeps)
        {
            Cheeps.Add(new CheepDTO(cheep));
        }


        Following = [];
        foreach (var a in author.Following)
        {
            Following.Add(a.DisplayName);
        }
    }
}