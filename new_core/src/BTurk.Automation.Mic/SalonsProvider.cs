﻿using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.DataPersistence;
using BTurk.Automation.Core.FileSystem;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Mic;

public class SalonsProvider : IRequestsProvider<Salon>
{
    private readonly IResourceProvider _resourceProvider;

    public SalonsProvider(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public IEnumerable<Salon> GetRequests()
    {
        var salons = _resourceProvider.Load<List<Salon>>(
            new FileParameters(DirectoryParameters.Configuration, "salons.json")
        );
        return salons;
    }
}