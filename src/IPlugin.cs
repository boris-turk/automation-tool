﻿namespace AutomationEngine
{
    public interface IPlugin
    {
        string Id { get; }
        void Execute();
    }
}