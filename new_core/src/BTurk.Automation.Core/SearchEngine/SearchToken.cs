using System.Linq;
using System.Text.RegularExpressions;

namespace BTurk.Automation.Core.SearchEngine;

public abstract class SearchToken
{
    public string Text { get; protected set; }

    public static SearchToken[] GetSearchTokens(string text)
    {
        var tokens = (
            from part in Regex.Split(text ?? "", @"\b")
            where string.IsNullOrEmpty(part) == false
            select ToSearchToken(part)
            ).ToArray();

        return tokens;

        SearchToken ToSearchToken(string textPart)
        {
            if (Regex.IsMatch(textPart, @"^\s+$"))
                return new SpaceToken();

            return new WordToken(textPart.Trim());
        }
    }
}