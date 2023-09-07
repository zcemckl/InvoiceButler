using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace InvoiceButler.ViewModels
{
    public class PdfViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private string changeLog = "";

        public string SelectedPdf { get; set; } = "";
        public string SelectedPartner { get; set; } = "";
        public IEnumerable<string> PdfList => 
            Directory
            .GetFiles(Settings.PdfFolderPath, "*.pdf", SearchOption.AllDirectories)
            .Select(o => o.TrimStart((Settings.PdfFolderPath + "\\").ToCharArray()))
            .ToList();
        public IEnumerable<string> PartnerList => Settings.PartnerNames.ToList();
        public DateTime SelectedDate { get; set; } = DateTime.Now;
        public string InvoiceNumber { get; set; } = "";
        public DateTime LastAvailableDate { get; set; } = DateTime.Now.AddYears(3);
        public DateTime FirstAvailableDate { get; set; } = DateTime.Now.AddYears(-3);
        public string ChangeLog
        {
            get
            {
                return changeLog;
            }
            set
            {
                SetProperty(ref changeLog, value);
            }
        }


        public DelegateCommand SavePdfCommand { get; }

        public PdfViewModel(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            _ea.GetEvent<SettingsChangedEvent>().Subscribe(SettingsChanged);

            SavePdfCommand = new DelegateCommand(SavePdf);
        }

        private void SettingsChanged()
        {
            RaisePropertyChanged(nameof(PdfList));
            RaisePropertyChanged(nameof(PartnerList));
        }
        private void SavePdf()
        {
            if (string.IsNullOrEmpty(SelectedPdf) ||
                string.IsNullOrEmpty(SelectedPartner) ||
                SelectedDate == DateTime.MinValue ||
                string.IsNullOrEmpty(InvoiceNumber))
            {
                ChangeLog = "Save Failed! Please check for missing fields.";
                return;
            }

            string outputDirectory = Settings.OutputDirectory + "\\" + SelectedPartner;
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var newName = 
                "INVOICE-" + 
                SelectedPartner + 
                "-" + 
                InvoiceNumber + 
                "." + 
                SelectedDate.ToString("ddmmyy") + 
                "." + 
                Guid.NewGuid().ToString() + 
                ".PDF";

            var newFileName = outputDirectory + "\\" + newName;
            if (File.Exists(newFileName))
            {
                var duplicateFileMessage = "Save Failed! Duplicate file exists.";
                MessageBox.Show(duplicateFileMessage);

                ChangeLog = duplicateFileMessage;
                return;
            }

            File.Copy(Settings.PdfFolderPath + "\\" + SelectedPdf, newFileName);
            File.Copy(newFileName, Settings.TransferDirectory + "\\" + newName);
        }
    }
}
