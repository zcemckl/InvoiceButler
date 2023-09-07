namespace InvoiceButler
{
    public delegate void SelectedPdfChangedEventHandler(object sender, SelectedPdfChangedEventArgs e);

    public class SelectedPdfChangedEventArgs
    {
        public SelectedPdfChangedEventArgs(string pdf1, string? pdf2)
        {
            CurrentSelectedPdf = pdf1;
            PreviousSelectedPdf = pdf2;
        }

        public string CurrentSelectedPdf { get; set; }
        public string? PreviousSelectedPdf { get; set; }
    }
}