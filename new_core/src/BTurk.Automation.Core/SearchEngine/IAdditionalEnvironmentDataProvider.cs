namespace BTurk.Automation.Core.SearchEngine;

public interface IAdditionalEnvironmentDataProvider
{
    void Process(EnvironmentContext context);
}