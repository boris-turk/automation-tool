namespace BTurk.Automation.Core.Queries;

public interface IQueryProcessor
{
    TResult Process<TResult>(IQuery<TResult> query);
}