namespace BTurk.Automation.Core.Requests
{
    public class AhkRequestExecutor : IRequestExecutor<AhkRequest>
    {
        public void Execute(RequestExecutionContext<AhkRequest> context)
        {
            var command = context.Request.Command;
        }
    }
}