namespace Chirp.CSVDBService.Models;

public class CreateCheepRequestModel
{
    public required string Author { get; set; }
    public required string Message { get; set; }
}