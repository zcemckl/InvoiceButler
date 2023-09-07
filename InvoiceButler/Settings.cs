using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceButler
{
    public static class Settings
    {
        public static string PdfFolderPath { get; set; } = "";
        public static string[] PartnerNames { get; set; } = Array.Empty<string>();
        public static string OutputDirectory { get; set; } = "";
        public static string TransferDirectory { get; set; } = "";
    }
}
