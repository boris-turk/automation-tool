namespace BTurk.Automation.Core.Messages
{
    public class ShowingAutomationWindowMessage : IMessage
    {
        private ShowingAutomationWindowMessage()
        {
        }

        public static readonly ShowingAutomationWindowMessage MainMenu = new ShowingAutomationWindowMessage();

        public static readonly ShowingAutomationWindowMessage ApplicationMenu = new ShowingAutomationWindowMessage();
    }
}