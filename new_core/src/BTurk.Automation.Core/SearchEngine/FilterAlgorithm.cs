﻿using System;
using System.Linq;

namespace BTurk.Automation.Core.SearchEngine;

public class FilterAlgorithm
{
    private readonly string[] _filterWords;

    public FilterAlgorithm(string filterText)
    {
        _filterWords = filterText.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries).ToArray();
    }

    public int GetScore(string text)
    {
        if (!_filterWords.Any())
            return 1; // no filter specified => all items match with same score

        var score = 0;

        var words = text.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries).ToList();

        if (!words.Any())
            return 0; // name not specified => item is not properly defined, filter it out

        for (int i = 0; i < _filterWords.Length; i++)
        {
            if (words.Count == 0)
                return 0;

            // get name word with the highest match score for the current filter word
            var element = words
                .Select((value, index) => new
                {
                    index,
                    matchScore = MatchScore(value, _filterWords[i])
                })
                .OrderByDescending(x => x.matchScore)
                .First();

            int multiplier = 1;

            if (element.index == 0 && i == 0)
                multiplier = 5; // first word match is more important, multiply its score by 5

            score += element.matchScore * multiplier;

            if (element.matchScore == 0)
                return 0; // one of the name words did not match => whole item does not match

            // the best matched word is no longer included in match evaluation
            words.RemoveAt(element.index);
        }

        return score;
    }

    private int MatchScore(string value, string text)
    {
        if (value.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
            return 1000;

        if (value.StartsPartiallyWith(text))
            return 100;

        if (value.ContainsPartially(text))
            return 1;

        return 0;
    }
}