#if __WASM__
using System;
using System.Collections.Generic;
using System.Text;
//using kahua.kdk.utility;
using Uno.Foundation;
using Uno.UI.Runtime.WebAssembly;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace kahua.host.uno.common.pdf
{
    public class PdfWebControl : Control
    {
        private string _folderPath;

        public PdfWebControl()
        {
            Console.WriteLine($"{nameof(PdfWebControl)}.ctor");

            Background = new SolidColorBrush(Colors.Transparent);
            Loaded += PdfWebControl_Loaded;
        }

        private void PdfWebControl_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"{nameof(PdfWebControl)}.PdfWebControl_Loaded");

            var htmlId = $"PDF_{this.GetHtmlId()}";
            this.SetHtmlContent($@"<div id=""{htmlId}""></div>");
        }

        public void ImportXfdf(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var htmlId = $"PDF_{this.GetHtmlId()}";
                var fullPath = System.IO.Path.Combine(_folderPath, path);
                Console.WriteLine(fullPath);

                string javascript = $@"
                (function(){{
                    var pdfviewer = (document.getElementById('{htmlId}')).ej2_instances[0];    
                    pdfviewer.importAnnotation('{fullPath}');
                }})();";
                this.ExecuteJavascript(javascript);
            }
            else
            {
                // No file was picked or the dialog was cancelled.
            }
        }

        public void ExportXfdf()
        {
            var htmlId = $"PDF_{this.GetHtmlId()}";

            string javascript = $@"
                (function(){{
                    var pdfviewer = (document.getElementById('{htmlId}')).ej2_instances[0];    
                    pdfviewer.exportAnnotation('Xfdf');
                }})();";
            this.ExecuteJavascript(javascript);
        }

        public bool LoadPdfViewer(string path, bool bigDocument = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                this.ExecuteJavascript("alert('Please enter a path value');");
                return false;
            }

            _folderPath = path;
            Console.WriteLine($"{nameof(PdfWebControl)}.TryLoadDocument");

            var htmlId = $"PDF_{this.GetHtmlId()}";

#if RELEASE
            var serviceUrl = "https://titan.kahua.com/webmarkupservice01/api/pdfviewer";
#else
            var serviceUrl = "http://localhost:50801/api/pdfviewer";
#endif
            //serviceUrl = "https://ej2services.syncfusion.com/production/web-services/api/pdfviewer";
            //var documentPath = "https://www.clickdimensions.com/links/TestPDFfile.pdf";
            var javascript = string.Empty;
            if(bigDocument)
            {
                var bigDocumentPath = "c:/websites/com.kahua.titan/markupfiles/bigdocument.pdf";
                javascript = $@"
                (function(){{
                    // initialize PDF Viewer component.
                    ej.pdfviewer.PdfViewer.Inject(ej.pdfviewer.Toolbar, ej.pdfviewer.Annotation, ej.pdfviewer.TextSelection, ej.pdfviewer.TextSearch, ej.pdfviewer.Navigation, ej.pdfviewer.Print);

                    var pdfviewer = new ej.pdfviewer.PdfViewer({{
                    serviceUrl: '{serviceUrl}',
                    documentPath: '{bigDocumentPath}'
                    }});

                    pdfviewer.appendTo('#{htmlId}');
                }})();";
            }
            else
            {
                javascript = $@"
                (function(){{
                    // initialize PDF Viewer component.
                    ej.pdfviewer.PdfViewer.Inject(ej.pdfviewer.Toolbar, ej.pdfviewer.Annotation, ej.pdfviewer.TextSelection, ej.pdfviewer.TextSearch, ej.pdfviewer.Navigation, ej.pdfviewer.Print);

                    var pdfviewer = new ej.pdfviewer.PdfViewer({{
                    serviceUrl: '{serviceUrl}'
                    }});

                    pdfviewer.appendTo('#{htmlId}');
                }})();";
            }
            this.ExecuteJavascript(javascript);
            return true;
        }
    }
}
#endif