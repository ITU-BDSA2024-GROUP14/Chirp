namespace Chirp.Core.DataModel;

public class OriginalCheep : Cheep
{
    public required string Text { get; init; }

    public override string GetText()
    {
        return Text;
    }
}