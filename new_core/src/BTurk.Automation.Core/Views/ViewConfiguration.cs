using System.Collections.Generic;

namespace BTurk.Automation.Core.Views;

public class ViewConfiguration
{
    public ViewConfiguration(IViewProvider viewProvider)
    {
        ViewProvider = viewProvider;
    }

    public IViewProvider ViewProvider { get; }

    public List<FieldConfiguration> Fields { get; } = new();

    public bool ShowAsModalDialog { get; set; }

    public string CancelQuestion { get; set; }

    public Builder<FieldConfiguration<T>> AddField<T>()
    {
        var fieldConfiguration = new FieldConfiguration<T>();
        Fields.Add(fieldConfiguration);
        return new Builder<FieldConfiguration<T>>(fieldConfiguration);
    }
}