using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace SavePDFUsingNative
{
    public partial class FileViewerPage : ContentPage
    {
        private MemoryStream _pdfStream;
        private MemoryStream _xfdfStream;

        public FileViewerPage(MemoryStream pdfStream, MemoryStream xfdfStream)
        {
            InitializeComponent();
            _pdfStream = pdfStream;
            _xfdfStream = xfdfStream;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            pdfViewerControl.InputFileStream = _pdfStream;
            try
            {
                pdfViewerControl.ImportAnnotations(_xfdfStream, Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
            }
            catch
            {
                DisplayAlert("Error importing annotations", "Email ddecker@kahua.com to get help on this issue.", "Cancel");
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                if (pdfViewerControl.InputFileStream != null)
                {
                    pdfViewerControl.InputFileStream.Dispose();
                }

                _pdfStream.Dispose();

            }
            catch (Exception ex)
            {
            }
        }
    }
}
