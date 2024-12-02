﻿namespace Chirp.Core.DataModel;

public class OriginalCheep : Cheep
{
    private readonly string _text = null!; //The field is never null, since it has a setter that is required. The warning can therefore be ignored.

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