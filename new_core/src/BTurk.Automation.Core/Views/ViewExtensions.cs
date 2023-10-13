using System;

namespace BTurk.Automation.Core.Views;

public static class ViewExtensions
{
    public static void Show(this IView view)
    {
        view.Execute(new ShowViewAction());
    }

    public static Builder<ViewConfiguration> Builder(this IViewProvider viewProvider)
    {
        return new Builder<ViewConfiguration>(new ViewConfiguration(viewProvider));
    }

    public static IView CreateAndShow(this Builder<ViewConfiguration> builder)
    {
        var view = builder.Instance.ViewProvider.Create(builder.Instance);
        view.Show();
        return view;
    }

    public static Builder<FieldConfiguration<T>> AddField<T>(this Builder<ViewConfiguration> builder)
    {
        return builder.Instance.AddField<T>();
    }

    public static Builder<ViewConfiguration> ModalDialogStyle(this Builder<ViewConfiguration> builder)
    {
        builder.Instance.ShowAsModalDialog = true;
        return builder;
    }

    public static Builder<ViewConfiguration> CancelQuestion(this Builder<ViewConfiguration> builder, string question)
    {
        builder.Instance.CancelQuestion = question;
        return builder;
    }

    public static Builder<FieldConfiguration<T>> PasswordInputStyle<T>(this Builder<FieldConfiguration<T>> builder)
    {
        builder.Instance.InputStyle = FieldInputStyle.Password;
        return builder;
    }

    public static Builder<FieldConfiguration<T>> BindSetter<T>(
        this Builder<FieldConfiguration<T>> builder, Action<T> setter)
    {
        builder.Instance.Setter = setter;
        return builder;
    }
}