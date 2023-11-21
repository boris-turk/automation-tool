// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Queries;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    TResult Handle(TQuery queryData);
}