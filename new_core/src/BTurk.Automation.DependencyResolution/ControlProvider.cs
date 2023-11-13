using System.Windows.Forms;
using BTurk.Automation.Core.Views;
using BTurk.Automation.WinForms.Providers;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

public class ControlProvider : IControlProvider
{
    public ControlProvider(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

    public Control Create(IControlConfiguration configuration)
    {
        var control = GenericMethodInvoker.Instance(this)
            .Method(nameof(Create))
            .WithGenericTypes(configuration.GetType())
            .WithArguments(configuration)
            .Invoke();

        return (Control)control;
    }

    public object Create<TConfiguration>(TConfiguration configuration) where TConfiguration : IControlConfiguration
    {
        var provider = Container.GetInstance<IControlProvider<TConfiguration>>();
        var instance = provider.CreateInstance(configuration);
        return instance;
    }
}