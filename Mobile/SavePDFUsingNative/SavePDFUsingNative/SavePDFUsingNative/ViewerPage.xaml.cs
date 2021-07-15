using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Syncfusion.Pdf.Parsing;
using Syncfusion.SfPdfViewer.XForms;
using Xamarin.Forms;

namespace SavePDFUsingNative
{
    public partial class ViewerPage : ContentPage
    {
        private Stream m_pdfDocumentStream;
        private ShapeAnnotation lineAnnotation;
        private ShapeAnnotation arrowAnnotation;
        public ViewerPage(string documentName)
        {
            _documentName = documentName;

            InitializeComponent();
            m_pdfDocumentStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream($"SavePDFUsingNative.Assets.{_documentName}Document.pdf");
            pdfViewerControl.LoadDocument(m_pdfDocumentStream);
            pdfViewerControl.DocumentSaveInitiated += PdfViewerControl_DocumentSaveInitiated;
        }

        private void PdfViewerControl_DocumentSaveInitiated(object sender, Syncfusion.SfPdfViewer.XForms.DocumentSaveInitiatedEventArgs args)
        {
            string filePath = DependencyService.Get<ISave>().Save(args.SaveStream as MemoryStream);
            string message = "The PDF has been saved to " + filePath;
            DependencyService.Get<IAlertView>().Show(message);
        }

        private void Clear_Clicked(object sender, EventArgs e)
        {
            pdfViewerControl.ClearAllAnnotations();
        }

        Stream exportedStream;
        private string _documentName;

        private void Export_Clicked(object sender, EventArgs e)
        {
            exportedStream = pdfViewerControl.ExportAnnotations(Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
        }

        private void Import_PDFTronOnly_Clicked(object sender, EventArgs e)
        {
            //Import annotations from "xfdf" data format
            Stream xfdfStreamToImport = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream($"SavePDFUsingNative.Assets.{_documentName}PDFTronOnly.xfdf");
            pdfViewerControl.ImportAnnotations(xfdfStreamToImport, AnnotationDataFormat.XFdf);
        }

        private void Import_SyncfusionOnly_Clicked(object sender, EventArgs e)
        {
            //Import annotations from "xfdf" data format
            Stream xfdfStreamToImport = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream($"SavePDFUsingNative.Assets.{_documentName}SyncfusionMobileOnly.xfdf");
            pdfViewerControl.ImportAnnotations(xfdfStreamToImport, AnnotationDataFormat.XFdf);
        }

        private void Import_Mixed_Clicked(object sender, EventArgs e)
        {
            //Import annotations from "xfdf" data format
            Stream xfdfStreamToImport = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream($"SavePDFUsingNative.Assets.{_documentName}Mixed.xfdf");
            pdfViewerControl.ImportAnnotations(xfdfStreamToImport, AnnotationDataFormat.XFdf);
        }
    }
}
