using BTurk.Automation.Core.Converters;
using SimpleInjector;

namespace BTurk.Automation.DependencyResolution;

public class GuiValueConverter : IGuiValueConverter
{
    public GuiValueConverter(Container container)
    {
        Container = container;
    }

    private Container Container { get; }

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
        var converter = Container.GetInstance<IGuiValueConverter<TDomain, TGui>>();
        return converter;
    }
}