namespace BTurk.Automation.Core.SearchEngine;

public class WordToken : SearchToken
{
    public WordToken(string word)
    {
        Text = word;
    }
}