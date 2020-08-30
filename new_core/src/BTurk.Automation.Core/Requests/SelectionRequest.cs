using System;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<T> : SequentialRequest
    {
        public SelectionRequest(Action<T> onSelectedAction)
        {
            OnSelectedAction = onSelectedAction ?? throw new ArgumentNullException(nameof(onSelectedAction));
        }

        public Predicate<T> Filter { get; set; }

        public Action<T> OnSelectedAction { get; }
    }
}