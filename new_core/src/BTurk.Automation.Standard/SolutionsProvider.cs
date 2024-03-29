﻿using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.DataPersistence;
using BTurk.Automation.Core.FileSystem;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard;

public class SolutionsProvider : IRequestsProvider<Solution>
{
    private readonly IResourceProvider _resourceProvider;

    public SolutionsProvider(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public IEnumerable<Solution> GetRequests()
    {
        return _resourceProvider.Load<List<Solution>>(
            new FileParameters(DirectoryParameters.Configuration, "solutions.json")
        );
    }
}