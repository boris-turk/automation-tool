﻿using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.DataPersistence;
using BTurk.Automation.Core.FileSystem;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard;

public class UrlsProvider : IRequestsProvider<UrlRequest>
{
    private readonly IResourceProvider _resourceProvider;

    public UrlsProvider(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public IEnumerable<UrlRequest> GetRequests()
    {
        return _resourceProvider.Load<List<UrlRequest>>(
            new FileParameters(DirectoryParameters.Configuration, "urls.json")
        );
    }
}