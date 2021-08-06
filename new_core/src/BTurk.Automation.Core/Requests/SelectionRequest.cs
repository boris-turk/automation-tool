﻿using System;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<TRequest> : Request, ISelectionRequest where TRequest : Request
    {
        public SelectionRequest()
        {
        }

        public SelectionRequest(string text)
        {
            Text = text;
        }

        public Action<TRequest> Selected { get; set; }

        public override bool CanVisit(VisitPredicateContext predicateContext)
        {
            return predicateContext.ActionType == ActionType.Execute;
        }
    }
}