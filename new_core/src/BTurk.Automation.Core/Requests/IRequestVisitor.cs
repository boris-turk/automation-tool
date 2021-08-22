namespace BTurk.Automation.Core.Requests
{
    public interface IRequestVisitor
    {
        void Visit(RequestVisitContext context);
    }

    public interface IRequestVisitor<TRequest, TChild> where TRequest : IRequest where TChild : IRequest
    {
        void Visit(RequestVisitContext<TRequest, TChild> context);
    }
}