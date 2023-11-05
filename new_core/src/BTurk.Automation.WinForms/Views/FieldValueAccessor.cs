using BTurk.Automation.Core.Converters;
using BTurk.Automation.Core.Views;

namespace BTurk.Automation.WinForms.Views;

public class FieldValueAccessor
{
    public FieldValueAccessor(IGuiValueConverter valueConverter, FieldConfiguration fieldConfiguration)
    {
        ValueConverter = valueConverter;
        FieldConfiguration = fieldConfiguration;
    }

    private IGuiValueConverter ValueConverter { get; }

    private FieldConfiguration FieldConfiguration { get; }

    public TValue GetValue<TValue>()
    {
        return FieldConfiguration.GetValue<TValue>(ValueConverter);
    }

    public void SetValue<TValue>(TValue value)
    {
        FieldConfiguration.SetValue(value, ValueConverter);
    }

    public bool CanSetValue()
    {
        return FieldConfiguration.CanSetValue();
    }
}