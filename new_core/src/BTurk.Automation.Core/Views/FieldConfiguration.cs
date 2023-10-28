using System;
using BTurk.Automation.Core.Converters;

namespace BTurk.Automation.Core.Views;

public abstract class FieldConfiguration : IControlConfiguration
{
    public int MaxLength { get; set; }
    public FieldInputStyle InputStyle { get; set; }
    public string LabelText { get; set; }

    public abstract TGui GetValue<TGui>(IGuiValueConverter valueConverter);
    public abstract void SetValue<TGui>(TGui value, IGuiValueConverter valueConverter);
}

public class FieldConfiguration<TDomain> : FieldConfiguration
{
    public Func<TDomain> Getter { get; set; }
    public Action<TDomain> Setter { get; set; }

    public override TGui GetValue<TGui>(IGuiValueConverter valueConverter)
    {
        if (Getter == null)
            return default;

        return valueConverter.ToGuiValue<TDomain, TGui>(Getter.Invoke());
    }

    public override void SetValue<TGui>(TGui value, IGuiValueConverter valueConverter)
    {
        if (Setter != null)
        {
            var domainValue = valueConverter.FromGuiValue<TGui, TDomain>(value);
            Setter.Invoke(domainValue);
        }
    }
}