﻿using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class Solution : Request, IFileRequest
    {
        public string Path { get; set; }
    }
}