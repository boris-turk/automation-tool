using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutomationEngine;

namespace TravelOrderRecorder
{
    public class TravelOrdersReport : IPlugin
    {
        public string Id => "travel_orders_report";

        public void Execute(params string[] arguments)
        {
            List<string> dates = (
                from travelOrder in TravelOrdersCollection.Instance.TravelOrders
                where travelOrder.IsTravelOrder
                select travelOrder.Date.ToString("d.M."))
                .ToList();

            string result = string.Join(", ", dates);

            Clipboard.SetText(result);
        }
    }
}