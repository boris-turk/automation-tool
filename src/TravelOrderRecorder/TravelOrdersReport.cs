using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutomationEngine;

namespace TravelOrderRecorder
{
    public class TravelOrdersReport : IPluginExecutor
    {
        public string Id => "travel_orders_report";

        public void Execute(params string[] arguments)
        {
            var previousMonth = DateTime.Now.GetPreviousMonthStart();

            List<string> dates = (
                from travelOrder in TravelOrdersCollection.Instance.TravelOrders
                let date = travelOrder.Date
                where date.Month == previousMonth.Month
                where date.Year == previousMonth.Year
                where travelOrder.IsTravelOrder
                orderby date
                select date.ToString("d.M."))
                .ToList();

            string result = string.Join(", ", dates);

            Clipboard.SetText(result);
        }
    }
}