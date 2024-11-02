using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.E3k;
using BTurk.Automation.Mic;
using BTurk.Automation.Standard;

// ReSharper disable StringLiteralTypo

namespace BTurk.Automation.DependencyResolution;

public class MainMenuRequest : Request
{
    public MainMenuRequest()
    {
        Configure()
            .ProcessCondition(c => c == EnvironmentContext.Empty)
            .AddChildRequests(
                new CommitRepositoryRequest(),
                new ShowRepositoryLogRequest(),
                new OpenSolutionRequest(),
                new OpenNoteRequest(),
                new OpenWindowRequest(),
                new UrlCollectionRequest(),
                new GitConsoleRequest(),
                new BuildCleanupRequest(),
                new SalonsCollectionRequest(),
                new OpenProgramRequest("gvim")
            );
    }
}