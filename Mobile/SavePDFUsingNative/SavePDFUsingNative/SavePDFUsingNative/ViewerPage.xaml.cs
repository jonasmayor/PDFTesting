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
        private MemoryStream _xfdfStream;

        private void Export_Clicked(object sender, EventArgs e)
        {
            try
            {
                pdfViewerControl.AnnotationSettings.IsLocked = true;

                _xfdfStream = pdfViewerControl.ExportAnnotations(Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf) as MemoryStream;

                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var xfdfPath = Path.Combine(path, $"annotations.xfdf");
                using (var outputFileStream = new FileStream(xfdfPath, FileMode.Create, FileAccess.Write))
                {
                    _xfdfStream.WriteTo(outputFileStream);
                }

                _xfdfStream.Position = 0;
            }
            catch (Exception err)
            {
            }
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

        void ImportExported_Clicked(System.Object sender, System.EventArgs e)
        {
            pdfViewerControl.ClearAllAnnotations();
            if(_xfdfStream != null)
            {
                pdfViewerControl.ImportAnnotations(_xfdfStream, AnnotationDataFormat.XFdf);
            }
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var actions = new string[] { "Start Ink", "End Ink" };
            var actionSheetResult = await DisplayActionSheet("Actions", "Cancel", null, actions);
            if(actionSheetResult == "Start Ink")
            {
                pdfViewerControl.AnnotationMode = AnnotationMode.Ink;
            }
            else
            {
                pdfViewerControl.EndInkSession(true);
            }
        }
    }
}
