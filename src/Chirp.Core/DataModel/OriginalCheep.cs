namespace Chirp.Core.DataModel;

public class OriginalCheep : Cheep
{
    private string _text;

    public required string Text
    {
        init
        {
            _text = value;
        }
    }

    public override string GetText()
    {
        return _text;
    }
}