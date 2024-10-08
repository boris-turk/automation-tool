﻿using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.E3k;
using BTurk.Automation.Mic;
using BTurk.Automation.Standard;

// ReSharper disable StringLiteralTypo

namespace BTurk.Automation.DependencyResolution;

public class MainMenuRequest : CollectionRequest<IRequest>
{
    public MainMenuRequest() : base("Main menu")
    {
    }

    protected override IEnumerable<IRequest> GetRequests()
    {
        yield return new CommitRepositoryRequest();
        yield return new ShowRepositoryLogRequest();
        yield return new OpenSolutionRequest();
        yield return new OpenNoteRequest();
        yield return new OpenWindowRequest();
        yield return new UrlCollectionRequest();
        yield return new GitConsoleRequest();
        yield return new BuildCleanupRequest();
        yield return new SalonsCollectionRequest();
        yield return new OpenProgramRequest("gvim");
    }

    protected override bool CanAccept(DispatchPredicateContext context)
    {
        return context.EnvironmentContext == EnvironmentContext.Empty;
    }
}