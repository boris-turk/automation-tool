using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Standard;

namespace BTurk.Automation.Mic;

public class SalonsCollectionRequest : Request
{
    public SalonsCollectionRequest()
    {
        Configure()
            .SetText("salon")
            .AddChildRequestsProvider<Salon>();
            //.SetCommand(GetCommand);
    }

    public ICommand GetCommand(Salon salon)
    {
        var computerName = GetComputerName(salon);
        return new ConnectWithAlwaysOnCommand(computerName);
    }

    private string GetComputerName(Salon salon)
    {
        if (salon.Id == "000")
            return "win-mico-sql";

        if (salon.Id.HasLength())
            return salon.Name;

        return salon.Id;
    }
}