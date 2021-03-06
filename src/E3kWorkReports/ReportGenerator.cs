﻿using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Aspose.Cells;
using AutomationEngine;

// ReSharper disable LocalizableElement
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace E3kWorkReports
{
    public class ReportGenerator
    {
        private string Directory { get; }
        private string BorutFilePath { get; }

        public ReportGenerator(string directory, string borutFileName)
        {
            Directory = directory;
            BorutFilePath = Path.Combine(Directory, borutFileName);
        }

        public string OutputFilePath
        {
            get
            {
                var dateTime = DateTime.Now.GetPreviousMonthStart();
                var fileName = $"work_report_{dateTime.Year}_{dateTime.Month:00}.xlsx";
                return Path.Combine(Directory, fileName);
            }
        }

        public void Execute()
        {
            try
            {
                ExecuteInternal();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ExecuteInternal()
        {
            if (File.Exists(OutputFilePath))
                throw new InvalidOperationException($"Output file {OutputFilePath} already exists");

            LoadAsposeLicense();
            CreateTargetFile();
            GenerateReports();
        }

        private void LoadAsposeLicense()
        {
            const string licenseName = "E3kWorkReports.Aspose.Total.lic";
            License license = new License();
            license.SetLicense(licenseName);
        }

        private void CreateTargetFile()
        {
            const string templateFileName = "E3kWorkReports.template.xlsx";

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(templateFileName))
            using (var outputFile = new FileStream(OutputFilePath, FileMode.Create, FileAccess.Write))
            {
                resource?.CopyTo(outputFile);
            }
        }

        private void GenerateReports()
        {
            var compositeEntriesProvider = new CompositeEntriesProvider(
                new ClockifyWorkEntriesProvider(),
                new BorutWorkEntriesProvider(BorutFilePath));

            var generator = new ExcelWorkReportGenerator(OutputFilePath, compositeEntriesProvider);

            generator.Execute();
        }
    }
}