using System;

namespace BTurk.Automation.Core.Views;

public class FieldConfiguration : IControlConfiguration
{
    public int MaxLength { get; set; }
    public FieldInputStyle InputStyle { get; set; }
    public string LabelText { get; set; }
}

public class FieldConfiguration<T> : FieldConfiguration
{
    public Func<T> Getter { get; set; }
    public Action<T> Setter { get; set; }
}