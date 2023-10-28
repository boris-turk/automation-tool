namespace BTurk.Automation.Core.Converters;

public interface IGuiValueConverter
{
    TGui ToGuiValue<TDomain, TGui>(TDomain value);
    TDomain FromGuiValue<TGui, TDomain>(TGui value);
}

public interface IGuiValueConverter<TDomain, TGui>
{
    TGui ToGuiValue(TDomain value);
    TDomain FromGuiValue(TGui value);
}
