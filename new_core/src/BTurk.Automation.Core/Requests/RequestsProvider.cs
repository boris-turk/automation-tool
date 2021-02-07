using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests
{
    public abstract class RequestsProvider<TRequest> where TRequest : Request
    {
        protected abstract IEnumerable<TRequest> Load();

        public void Populate(IRequestsConsumer<TRequest> consumer)
        {
            consumer.Add(Load());
        }
    }
}