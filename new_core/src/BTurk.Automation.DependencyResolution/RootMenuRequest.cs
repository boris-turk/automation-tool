﻿using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution;

public class RootMenuRequest : CollectionRequest<IRequest>
{
    public RootMenuRequest() : base("Root")
    {
    }

    protected override IEnumerable<IRequest> GetRequests()
    {
        yield return new MainMenuRequest();
        yield return new VisualStudioRequest();
    }
}