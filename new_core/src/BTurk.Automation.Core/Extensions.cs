using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core;

public static class Extensions
{
    public static bool HasLength(this string text)
    {
        return !string.IsNullOrWhiteSpace(text);
    }

    public static bool StartsPartiallyWith(this string testedText, string value)
    {
        return ContainsPartially(testedText, value, true);
    }

    public static bool ContainsPartially(this string testedText, string value)
    {
        return ContainsPartially(testedText, value, false);
    }

    private static bool ContainsPartially(string testedText, string value, bool mustMatchAtStart)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        int j = 0;
        for (int i = 0; i < testedText.Length; i++)
        {
            if (j >= value.Length)
            {
                break;
            }
            if (char.ToUpperInvariant(value[j]) == char.ToUpperInvariant(testedText[i]))
            {
                j++;
            }
            else if (j == 0 && mustMatchAtStart)
            {
                return false;
            }
        }

        return j == value.Length;
    }

    public static TV MaxOrDefault<T, TV>(this IEnumerable<T> items, Func<T, TV> selector)
    {
        var list = items.ToList();
        return list.Any() ? list.Select(selector).Max() : default;
    }

    public static string GetDebuggerDisplayText(IRequestV2 request)
    {
        var type = request.GetType();

        string displayText;

        if (type.IsGenericType)
        {
            var typeName = Regex.Replace(type.Name, @"`\d$", "");
            var argumentTypes = type.GetGenericArguments().Select(t => t.Name);
            var argumentNames = string.Join(", ", argumentTypes);
            displayText = $"{typeName}<{argumentNames}>";
        }
        else
        {
            displayText = type.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Configuration.Text))
            displayText = $"{displayText}: {request.Configuration.Text}";

        return displayText;
    }
}