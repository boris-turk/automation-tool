﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine
{
    [DebuggerDisplay("{" + nameof(RequestTypeName) + "}")]
    public class SearchStep
    {
        public SearchStep(Request request)
        {
            Request = request;
            Children = new List<Request>();
            Text = "";
        }

        public Request Request { get; }

        public List<Request> Children { get; }

        public string Text { get; set; }

        private string RequestTypeName
        {
            get
            {
                if (Request == null)
                    return "";

                var type = Request.GetType();

                if (!type.IsGenericType)
                    return type.Name;

                var typeName = Regex.Replace(type.Name, @"`\d$", "");
                var argumentTypes = type.GetGenericArguments().Select(_ => _.Name);
                var argumentNames = string.Join(", ", argumentTypes);

                return $"{typeName}<{argumentNames}>";
            }
        }
    }
}