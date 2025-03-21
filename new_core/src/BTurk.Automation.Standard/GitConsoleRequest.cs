﻿using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class GitConsoleRequest : Request
{
    public GitConsoleRequest()
    {
        Configure()
            .SetText("git")
            .AddChildRequestsProvider<Repository>()
            .SetCommand(r => new OpenGitConsoleCommand(r.Path))
            .ProcessCondition(r => r.Type == RepositoryType.Git);
    }
}