using System;
using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine;

public abstract class SearchToken
{
    public string Text { get; protected set; }

    public static List<SearchToken> GetSearchTokens(string text)
    {
        var words = text.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries).ToArray();
        var tokens = words.Select(w => new WordToken(w)).Cast<SearchToken>().ToList();

        if (tokens.Any() && (text.EndsWith(" ") || text.EndsWith("\t")))
            tokens.Insert(tokens.Count, new SpaceToken());

        return tokens;
    }
}