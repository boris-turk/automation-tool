using System.Collections.Generic;
using AutomationEngine;

namespace TravelOrderRecorder
{
    public class TravelOrdersCollection : FileStorage<TravelOrdersCollection>
    {
        public TravelOrdersCollection()
        {
            TravelOrders = new List<TravelOrder>();
        }

        public List<TravelOrder> TravelOrders { get; set; }

        public override string StorageFileName => @"xlab\travel_orders.xml";
    }
}