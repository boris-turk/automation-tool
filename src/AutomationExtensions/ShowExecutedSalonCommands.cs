using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AutomationEngine;

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable StringCompareToIsCultureSpecific

namespace AutomationExtensions
{
    public class ShowExecutedSalonCommands : IPluginExecutor
    {
        public string Id => "missing_salon_commands";

        public void Execute(params string[] arguments)
        {
            var commandSubstring = ShowDialog("Command substring", "Command substring:");

            var missingSalonIds = GetMissingSalons(commandSubstring);

            var message = $"Missing salons:{Environment.NewLine}{string.Join(Environment.NewLine, missingSalonIds)}";

            MessageBox.Show(message);
        }

        private IEnumerable<string> GetMissingSalons(string commandSubstring)
        {
            var salonIds = GetValidSalonIds().ToList();

            var processedSalonIds = GetProcessedSalonIds(commandSubstring).ToList();

            var cro = processedSalonIds.Where(IsCroSalon).ToList();
            var slo = processedSalonIds.Where(IsSloSalon).ToList();
            var srb = processedSalonIds.Where(IsSrbSalon).ToList();

            if (cro.Any())
            {
                foreach (var salonId in salonIds.Where(_ => IsCroSalon(_) && !cro.Contains(_)))
                    yield return salonId;
            }

            if (slo.Any())
            {
                foreach (var salonId in salonIds.Where(_ => IsSloSalon(_) && !slo.Contains(_)))
                    yield return salonId;
            }

            if (srb.Any())
            {
                foreach (var salonId in salonIds.Where(_ => IsSrbSalon(_) && !srb.Contains(_)))
                    yield return salonId;
            }
        }

        private IEnumerable<string> GetProcessedSalonIds(string commandSubstring)
        {
            var regex = new Regex(@".*\\[^_]*_(\d\d\d)");
            var directory = @"C:\Users\Boris\Koofr\Upload";

            if (!Directory.Exists(directory))
                yield break;

            foreach (var filePath in Directory.GetFiles(directory))
            {
                if (!filePath.Contains(commandSubstring))
                    continue;

                var match = regex.Match(filePath);

                if (!match.Success)
                    continue;

                yield return match.Groups[1].Value;
            }
        }

        private IEnumerable<string> GetValidSalonIds()
        {
            var filePath = @"C:\Users\Boris\Dropbox\Automation\custom\mic_saloni.txt";

            if (!File.Exists(filePath))
                yield break;

            var regex = new Regex(@"^(\d\d\d)\s");

            foreach (var line in File.ReadAllLines(filePath))
            {
                var match = regex.Match(line);

                if (!match.Success)
                    continue;


                yield return match.Groups[1].Value;
            }
        }

        private bool IsCroSalon(string salonId)
        {
            return salonId.CompareTo("001") >= 0 && salonId.CompareTo("200") == -1;
        }

        private bool IsSloSalon(string salonId)
        {
            return salonId.CompareTo("200") >= 0 && salonId.CompareTo("400") == -1;
        }

        private bool IsSrbSalon(string salonId)
        {
            return salonId.CompareTo("400") >= 0 && salonId.CompareTo("500") == -1;
        }

        public static string ShowDialog(string caption, string text)
        {
            var prompt = new Form
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            var textLabel = new Label
            {
                Left = 20, Top = 15, Text = text, AutoSize = true
            };

            var textBox = new TextBox
            {
                Left = 20, Top = 40, Width = prompt.ClientRectangle.Width - 40
            };

            var confirmation = new Button
            {
                Text = "Ok", Left = 200, Width = 100, Top = 75, DialogResult = DialogResult.OK
            };

            confirmation.Click += (_, __) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}