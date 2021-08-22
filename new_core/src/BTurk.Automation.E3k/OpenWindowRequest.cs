﻿using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k
{
    public class OpenWindowRequest : Request, ISelectionRequest
    {
        public OpenWindowRequest() : base("window")
        {
        }

        public IEnumerable<Request> GetRequests(EnvironmentContext context)
        {
            yield return new SelectionRequest<Module>
            {
                //Selected = solution => solution.Open()
            };
        }
    }
}