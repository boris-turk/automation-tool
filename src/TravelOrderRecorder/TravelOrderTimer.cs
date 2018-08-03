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

            DialogResult answer;
            try
            {
                if (!IsLjubljanaNetwork())
                {
                    return;
                }
                answer = RecordTravelOrderMessage();
            }
            catch (Exception e)
            {
                answer = RecordTravelOrderMessage(e);
                if (answer == DialogResult.Cancel)
                {
                    return;
                }
            }

            bool isTravelOrder = answer == DialogResult.Yes;

            TravelOrdersCollection.Instance.TravelOrders.Add(new TravelOrder
            {
                Date = DateTime.Today,
                IsTravelOrder = isTravelOrder
            });

            TravelOrdersCollection.Instance.Save();
        }

        private DialogResult RecordTravelOrderMessage(Exception ex = null)
        {
            string title = "Potni nalog";

            string message;
            if (ex == null)
            {
                message = "Zabelezim potni nalog?";
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Prislo je do napake pri preverjanju omrezij. ");
                builder.Append("Vseeno zabelezim potni nalog?");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
                builder.Append(ex);
                message = builder.ToString();
            }

            using (var form = new Form())
            {
                var buttons = MessageBoxButtons.YesNo;
                var focusedButton = MessageBoxDefaultButton.Button1;

                if (ex != null)
                {
                    buttons = MessageBoxButtons.YesNoCancel;
                    focusedButton = MessageBoxDefaultButton.Button3;
                }

                DialogResult answer = MessageBox.Show(
                    form, message, title, buttons, MessageBoxIcon.None,
                    focusedButton, (MessageBoxOptions) 0x40000);

                return answer;
            }
        }

        private bool IsLjubljanaNetwork()
        {
            WlanClient client = new WlanClient();
            List<string> networks = GetWirelessNetworks(client);
            return networks.Any(x =>
		        x.ToLower().Contains("mukija") ||
		        x.ToLower().Contains("sistem8"));
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
