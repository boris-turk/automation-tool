using System;
using System.Windows.Forms;
using BTurk.Automation.Core.Converters;
using BTurk.Automation.Core.Views;
using BTurk.Automation.WinForms;
using BTurk.Automation.WinForms.Providers;

namespace BTurk.Automation.DependencyResolution;

public class GuiValueConverter : IGuiValueConverter
{
    public TGui ToGuiValue<TDomain, TGui>(TDomain value)
    {
        var converter = GetConverter<TDomain, TGui>();
        return converter.ToGuiValue(value);
    }

    public TDomain FromGuiValue<TGui, TDomain>(TGui value)
    {
        var converter = GetConverter<TDomain, TGui>();
        return converter.FromGuiValue(value);
    }

    private IGuiValueConverter<TDomain, TGui> GetConverter<TDomain, TGui>()
    {
        throw new NotImplementedException();
    }

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