using BTurk.Automation.Core.Views;

namespace BTurk.Automation.WinForms;

public class ViewProvider : IViewProvider
{
    //private readonly IControlProvider _controlProvider;

    //public ViewProvider(IControlProvider controlProvider)
    //{
    //    _controlProvider = controlProvider;
    //}

    public IView Create(ViewConfiguration configuration)
    {
        var form = new CustomForm
        {
            Configuration = configuration
        };

        //foreach (var field in configuration.Fields)
        //{
        //    _controlProvider.Create()
        //    new FieldBuilder()
        //}

        return form;
    }
}