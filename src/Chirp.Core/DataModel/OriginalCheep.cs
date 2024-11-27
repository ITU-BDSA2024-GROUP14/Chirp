namespace Chirp.Core.DataModel;

public class OriginalCheep (String text) : Cheep
{
    public override string Text { get { return text; } }
}