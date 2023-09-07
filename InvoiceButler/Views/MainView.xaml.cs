using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoiceButler.Views
{
    /// <summary>
    /// Interaction logic for InvoiceButlerView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void PdfView_SelectedPdfChanged(object sender, SelectedPdfChangedEventArgs e)
        {
            webBrowser.Source = new Uri(Settings.PdfFolderPath + "\\" + e.CurrentSelectedPdf);
        }
    }
}
