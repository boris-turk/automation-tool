using System.Collections.Generic;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.WinApi;

namespace BTurk.Automation.Core.SearchEngine
{
    public class EnvironmentContextProvider : IEnvironmentContextProvider,
        IMessageHandler<ShowingAutomationWindowMessage>
    {
        private EnvironmentContext _context;

        private readonly IEnumerable<IAdditionalEnvironmentDataProvider> _additionalDataProviders;

        public EnvironmentContextProvider(IEnumerable<IAdditionalEnvironmentDataProvider> additionalDataProviders)
        {
            _context = EnvironmentContext.Empty;
            _additionalDataProviders = additionalDataProviders;
        }

        private EnvironmentContext GetContext(ShowingAutomationWindowMessage message)
        {
            if (message == ShowingAutomationWindowMessage.MainMenu)
                return EnvironmentContext.Empty;

            var windowHandle = Methods.GetActiveWindow();
            var windowText = Methods.GetWindowText(windowHandle);
            var windowClass = Methods.GetClassName(windowHandle);

            var context = new EnvironmentContext(windowText, windowClass);

            var additionalDataProvider = GetAdditionalDataProvider(context);
            additionalDataProvider?.Process(context);

            return context;
        }

        private IAdditionalEnvironmentDataProvider GetAdditionalDataProvider(EnvironmentContext context)
        {
            foreach (var provider in _additionalDataProviders)
                provider.Process(context);

            return null;
        }

        EnvironmentContext IEnvironmentContextProvider.Context => _context;

        void IMessageHandler<ShowingAutomationWindowMessage>.Handle(ShowingAutomationWindowMessage message)
        {
            _context = GetContext(message);
        }
    }
}