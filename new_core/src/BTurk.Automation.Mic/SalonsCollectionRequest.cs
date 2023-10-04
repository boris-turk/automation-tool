using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Standard;

namespace BTurk.Automation.Mic;

public class SalonsCollectionRequest : CollectionRequest<Salon>
{
    public SalonsCollectionRequest() : base("salon")
    {
    }

    protected override void OnRequestLoaded(Salon salon)
    {
        var computerName = GetComputerName(salon);
        salon.Command = new ConnectWithAlwaysOnCommand(computerName);
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