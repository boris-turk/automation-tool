// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Core.Converters;

public class InvariantGuiValueConverter<TDomain> : IGuiValueConverter<TDomain, TDomain>
{
    public TDomain ToGuiValue(TDomain value) => value;
    public TDomain FromGuiValue(TDomain value) => value;
}