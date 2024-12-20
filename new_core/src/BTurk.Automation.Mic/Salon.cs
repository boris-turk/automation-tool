﻿using System.Runtime.Serialization;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Mic;

public class Salon : Request
{
    public Salon()
    {
        Configure().SetText(GetText);
    }

    [DataMember(Name = "Id")]
    public string Id { get; set; }

    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Type")]
    public string Type { get; set; }

    [DataMember(Name = "Address")]
    public string Address { get; set; }

    [DataMember(Name = "Company")]
    public string Company { get; set; }

    private string GetText()
    {
        var text = $"{Name} {Type} {Address} {Company}";

        if (Id.HasLength())
            text = $"{Id} {text}";

        return text;
    }
}