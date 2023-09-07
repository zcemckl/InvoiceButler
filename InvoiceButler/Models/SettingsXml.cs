using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceButler.Models
{
    public class SettingsXml
    {
        public SettingsXml()
        {
            PdfFolderPath = "";
            PartnerNames = Array.Empty<string>();
            OutputFolderPath = "";
            TransferFolderPath = "";
        }

        public SettingsXml(
            string pdfFolderPath, 
            IEnumerable<string> partnerNames, 
            string outputFolderPath,
            string transferFolderPath)
        {
            PdfFolderPath = pdfFolderPath;
            PartnerNames = partnerNames.ToArray();
            OutputFolderPath = outputFolderPath;
            TransferFolderPath = transferFolderPath;
        }

        public string PdfFolderPath { get; set; }
        public string OutputFolderPath { get; set; }
        public string TransferFolderPath { get; set; }
        public string[] PartnerNames { get; set; }
    }
}
