using System.Windows.Forms;
using BTurk.Automation.Core.Views;

namespace BTurk.Automation.WinForms.Providers;

public interface IControlProvider
{
    Control Create(IControlConfiguration configuration);
}

public interface IControlProvider<in TControlConfiguration> where TControlConfiguration : IControlConfiguration
{
    Control CreateInstance(TControlConfiguration configuration);
}
