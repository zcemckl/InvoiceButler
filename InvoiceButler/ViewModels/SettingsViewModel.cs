using InvoiceButler.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace InvoiceButler.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private string partnerName = "";
        private string configFile = Directory.GetCurrentDirectory() + "\\invoiceButler.config";
        private string pdfFolder = "";
        private string outputDirectory = "";
        private string transferDirectory = "";
        private string changeLog = "";

        public DelegateCommand SelectConfigFolderCommand { get; }
        public DelegateCommand SelectPdfFolderCommand { get; }
        public DelegateCommand SelectOutputDirectoryCommand { get; }
        public DelegateCommand SelectTransferDirectoryCommand { get; }
        public DelegateCommand<object> DeletePartnersCommand { get; }
        public DelegateCommand AddPartnerCommand { get; }
        public DelegateCommand SaveConfigCommand { get; }

        public ObservableCollection<string> PartnerList { get; } = new ObservableCollection<string>();

        public string PartnerName 
        {
            get
            { 
                return partnerName;
            } 
            set 
            {
                SetProperty(ref partnerName, value);
            } 
        }
        public string ConfigFile 
        {
            get
            {
                return configFile; 
            } 
            set
            { 
                SetProperty(ref configFile, value); 
            } 
        }
        public string PdfFolder 
        { 
            get 
            {
                return pdfFolder;
            }
            set 
            { 
                SetProperty(ref pdfFolder, value);
            }
        }
        public string OutputDirectory
        {
            get
            {
                return outputDirectory;
            }
            set
            {
                SetProperty(ref outputDirectory, value);
            }
        }
        public string TransferDirectory
        {
            get
            {
                return transferDirectory;
            }
            set
            {
                SetProperty(ref transferDirectory, value);
            }
        }
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

        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;

            SelectConfigFolderCommand = new DelegateCommand(SelectConfigFolder);
            SelectPdfFolderCommand = new DelegateCommand(SelectPdfFolder);
            SelectOutputDirectoryCommand = new DelegateCommand(SelectOutputDirectory);
            SelectTransferDirectoryCommand = new DelegateCommand(SelectTransferDirectory);
            DeletePartnersCommand = new DelegateCommand<object>(DeletePartners);
            AddPartnerCommand = new DelegateCommand(AddPartner);
            SaveConfigCommand = new DelegateCommand(SaveConfig);

            PartnerName = "";

            if (File.Exists(ConfigFile))
            {
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(SettingsXml));

                    using (var reader = new FileStream(ConfigFile, FileMode.Open))
                    {
                        var config = xmlSerializer.Deserialize(reader) as SettingsXml;

                        if (config != null)
                        {
                            PdfFolder = config.PdfFolderPath;
                            OutputDirectory = config.OutputFolderPath;
                            TransferDirectory = config.TransferFolderPath;

                            PartnerList.Clear();
                            PartnerList.AddRange(config.PartnerNames);

                            Settings.PdfFolderPath = config.PdfFolderPath;
                            Settings.PartnerNames = config.PartnerNames.ToArray();
                            Settings.OutputDirectory = config.OutputFolderPath;
                            Settings.TransferDirectory = config.TransferFolderPath;
                            _ea.GetEvent<SettingsChangedEvent>().Publish();

                            ChangeLog = "File Loaded";
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    File.Delete(ConfigFile);
                }
            }
        }

        private void AddPartner()
        {
            if (string.IsNullOrEmpty(PartnerName))
            {
                return;
            }

            PartnerList.Add(PartnerName);

            PartnerName = string.Empty;
        }
        private void DeletePartners(object param)
        {
            var partnerNames = (param as IList)?.Cast<string>().ToList();

            if (partnerNames == null)
            {
                return;
            }

            foreach (string partnerName in partnerNames)
            {
                PartnerList.Remove(partnerName);
            }
        }
        private void SelectConfigFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Open Configuration Folder";
            folderBrowserDialog.UseDescriptionForTitle = true;
            folderBrowserDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ConfigFile = folderBrowserDialog.SelectedPath + "\\invoiceButler.config";

            PartnerList.Clear();
            PdfFolder = "";

            if (File.Exists(ConfigFile))
            {
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(SettingsXml));

                    using (var reader = new FileStream(ConfigFile, FileMode.Open))
                    {
                        var config = xmlSerializer.Deserialize(reader) as SettingsXml;

                        if (config != null)
                        {
                            PdfFolder = config.PdfFolderPath;
                            OutputDirectory = config.OutputFolderPath;
                            TransferDirectory = config.TransferFolderPath;

                            PartnerList.AddRange(config.PartnerNames);

                            Settings.PdfFolderPath = config.PdfFolderPath;
                            Settings.PartnerNames = config.PartnerNames.ToArray();
                            Settings.OutputDirectory = config.OutputFolderPath;
                            Settings.TransferDirectory = config.TransferFolderPath;
                            _ea.GetEvent<SettingsChangedEvent>().Publish();

                            ChangeLog = "File Loaded";
                        }
                    }
                }
                catch
                {
                    File.Delete(ConfigFile);
                }
            }
        }
        private void SelectPdfFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select Invoice Folder";
            folderBrowserDialog.UseDescriptionForTitle = true;
            folderBrowserDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            PdfFolder = folderBrowserDialog.SelectedPath;
        }
        private void SelectOutputDirectory()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select Output Directory";
            folderBrowserDialog.UseDescriptionForTitle = true;
            folderBrowserDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            OutputDirectory = folderBrowserDialog.SelectedPath;
        }
        private void SelectTransferDirectory()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select Transfer Directory";
            folderBrowserDialog.UseDescriptionForTitle = true;
            folderBrowserDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            TransferDirectory = folderBrowserDialog.SelectedPath;
        }
        private void SaveConfig()
        {
            if(string.IsNullOrEmpty(PdfFolder) ||
                string.IsNullOrEmpty(OutputDirectory) ||
                string.IsNullOrEmpty(TransferDirectory) ||
                PartnerList.Count == 0)
            {
                ChangeLog = "Save Failed! Please check for missing directories or partner names.";
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(SettingsXml));

                using (var stream = new FileStream(ConfigFile, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(stream, new SettingsXml(PdfFolder, PartnerList, OutputDirectory, TransferDirectory));
                }
                
                Settings.PdfFolderPath = PdfFolder;
                Settings.PartnerNames = PartnerList.ToArray();
                Settings.OutputDirectory = OutputDirectory;
                Settings.TransferDirectory = TransferDirectory;
                _ea.GetEvent<SettingsChangedEvent>().Publish();

                ChangeLog = "File Saved.";
            }
            catch
            {
            }
        }
    }
}
