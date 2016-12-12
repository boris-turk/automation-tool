using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutomationEngine;

namespace TravelOrderRecorder
{
    public class TravelOrderTimer : ITimer
    {
        public void Execute()
        {
            if (TravelOrdersCollection.Instance.TravelOrders.Any(x => x.Date == DateTime.Today))
            {
                return;
            }
            if (!IsLjubljanaNetwork())
            {
                return;
            }

            string title = "Potni nalog";
            string message = "Zabelezim potni nalog?";

            DialogResult answer = MessageBox.Show(
                message, title, MessageBoxButtons.YesNo, MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

            bool isTravelOrder = answer == DialogResult.Yes;

            TravelOrdersCollection.Instance.TravelOrders.Add(new TravelOrder
            {
                Date = DateTime.Today,
                IsTravelOrder = isTravelOrder
            });

            TravelOrdersCollection.Instance.Save();
        }

        private bool IsLjubljanaNetwork()
        {
            WlanClient client = new WlanClient();
            List<string> networks = GetWirelessNetworks(client);
            return networks.Any(x => x.ToLower().Contains("mukija"));
        }

        private List<string> GetWirelessNetworks(WlanClient client)
        {
            return (
                from wlanInterface in client.Interfaces
                from network in wlanInterface.GetAvailableNetworkList(0)
                //where network.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.WEP
                select GetStringForSSID(network.dot11Ssid))
                .Distinct()
                .ToList();
        }

        private string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.Ssid, 0, (int)ssid.SsidLength);
        }
    }
}
