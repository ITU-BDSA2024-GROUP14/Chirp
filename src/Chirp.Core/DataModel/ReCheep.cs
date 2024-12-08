﻿namespace Chirp.Core.DataModel;

public class RepostCheep : Cheep
{
    public required OriginalCheep Content { get; init; }
    public Author OriginalPoster { get { return Content.Author; } }
    public override string GetText()
    {
        return Content.GetText();
    }
}