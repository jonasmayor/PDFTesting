using Syncfusion.Pdf.Parsing;
using Syncfusion.SfPdfViewer.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace SavePDFUsingNative
{
	public partial class MainPage : ContentPage
	{
        private Stream m_pdfDocumentStream;
        private ShapeAnnotation lineAnnotation;
        private ShapeAnnotation arrowAnnotation;
        public MainPage()
		{
			InitializeComponent();
            m_pdfDocumentStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SavePDFUsingNative.Assets.vectorDocument.pdf");
            pdfViewerControl.LoadDocument(m_pdfDocumentStream);
            pdfViewerControl.DocumentSaveInitiated += PdfViewerControl_DocumentSaveInitiated;
            pdfViewerControl.ShapeAnnotationAdded += PdfViewerControl_ShapeAnnotationAdded;
            pdfViewerControl.InkAdded += PdfViewerControl_InkAdded;
		}

        private void PdfViewerControl_InkAdded(object sender, InkAddedEventArgs args)
        {
            (sender as InkAnnotation).Settings.Color = Color.Black;
        }

        private void PdfViewerControl_ShapeAnnotationAdded(object sender, ShapeAnnotationAddedEventArgs args)
        {
           if (sender is ShapeAnnotation)
            {

               if( (sender as ShapeAnnotation).ShapeAnnotationType == ShapeAnnotationType.Line)
                {
                    lineAnnotation = (sender as ShapeAnnotation);
                }
                if ((sender as ShapeAnnotation).ShapeAnnotationType == ShapeAnnotationType.Arrow)
                {
                    arrowAnnotation = (sender as ShapeAnnotation);
                }
            }
        }

        private void PdfViewerControl_DocumentSaveInitiated(object sender, Syncfusion.SfPdfViewer.XForms.DocumentSaveInitiatedEventArgs args)
        {
            string filePath = DependencyService.Get<ISave>().Save(args.SaveStream as MemoryStream);
            string message = "The PDF has been saved to " + filePath;
            DependencyService.Get<IAlertView>().Show(message);
        }


        private void Text_Clicked(object sender, EventArgs e)
        {
            pdfViewerControl.AnnotationMode = AnnotationMode.FreeText;
        }

        private void Line_Clicked_2(object sender, EventArgs e)
        {
            pdfViewerControl.AnnotationMode = AnnotationMode.Line;
        }

        private void Arrow_Clicked_3(object sender, EventArgs e)
        {
            pdfViewerControl.ClearAllAnnotations();

            //Stream xfdfStreamToImport = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SavePDFUsingNative.Assets.Exporteddata.xfdf");
            //var xdoc = XDocument.Load(xfdfStreamToImport);
            //var xdocString = xdoc.ToString();
            //var listMarkupFilePath = loadRawAnnotationData(xdocString);


            //var xDoc = XDocument.Parse(xdocString);
            //xDoc.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var markupFilePath = Path.Combine(path, $"annotationData.xfdf");

            //if (System.IO.File.Exists(markupFilePath))
            //{
            //    System.IO.File.Delete(markupFilePath);
            //}

            //xDoc.Save(markupFilePath);


            //var fileStream = new FileStream(markupFilePath, FileMode.OpenOrCreate);


            //pdfViewerControl.ImportAnnotations(fileStream, AnnotationDataFormat.XFdf);
        }


        private string loadRawAnnotationData(string rawData)
        {
            var listMarkupFilePath = string.Empty;
            var fileName = "listAnnotationData.xfdf";
            try
            {
                listMarkupFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                var xDoc = XDocument.Parse(rawData);

                if (System.IO.File.Exists(listMarkupFilePath))
                {
                    System.IO.File.Delete(listMarkupFilePath);
                }
                xDoc.Save(listMarkupFilePath);
            }
            catch { }

            return listMarkupFilePath;
        }

        private void Ink_Clicked_4(object sender, EventArgs e)
        {
            pdfViewerControl.AnnotationMode = AnnotationMode.Ink;
         

        }

        private void None_Clicked_5(object sender, EventArgs e)
        {
            pdfViewerControl.AnnotationMode = AnnotationMode.None;
          
        }
        private void End_Clicked_5(object sender, EventArgs e)
        {
            pdfViewerControl.EndInkSession(true);

        }

        Stream exportedStream;

        private void Export_Clicked(object sender, EventArgs e)
        {
            exportedStream = pdfViewerControl.ExportAnnotations(Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
        }

        private void Import_Clicked_1(object sender, EventArgs e)
        {

            //Import annotations from "fdf" data format
            Stream xfdfStreamToImport = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SavePDFUsingNative.Assets.Exporteddata.xfdf");
            pdfViewerControl.ImportAnnotations(xfdfStreamToImport, AnnotationDataFormat.XFdf);
        }

        private void ImportAgain_Clicked_1(object sender, EventArgs e)
        {
           try
            {

                if (exportedStream != null)
                {

                    exportedStream.Position = 0;
                    pdfViewerControl.ImportAnnotations(exportedStream, Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
                }
            }
            catch
            (Exception er)
            {

            }

        }
     



    }
}
