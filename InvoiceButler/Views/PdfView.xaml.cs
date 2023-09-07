using System;
using System.Collections.Generic;
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
    /// Interaction logic for InvoiceButlerPdfView.xaml
    /// </summary>
    public partial class PdfView : UserControl
    {
        public event SelectedPdfChangedEventHandler SelectedPdfChanged;

        public PdfView()
        {
            InitializeComponent();
        }

        private void pdfListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count != 1 || e.RemovedItems.Count > 1)
            {
                return;
            }

            var newSelection = e.AddedItems.Cast<string>().First();
            var oldSelection = e.RemovedItems.Cast<string>().FirstOrDefault();

            SelectedPdfChanged.Invoke(this, new SelectedPdfChangedEventArgs(newSelection, oldSelection));
        }
    }
}
