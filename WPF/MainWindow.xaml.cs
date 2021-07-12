using Syncfusion.Windows.PdfViewer;
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

namespace SyncFusionViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileName = null;

        private HashSet<string> _annotationNames = new HashSet<string>();

        public MainWindow()
        {
            InitializeComponent();
            Viewer.ToolbarSettings.ShowFileTools = false;
            Viewer.ShapeAnnotationChanged += Viewer_ShapeAnnotationChanged;
            Viewer.InkAnnotationChanged += Viewer_InkAnnotationChanged;
            Viewer.FreeTextAnnotationChanged += Viewer_FreeTextAnnotationChanged;
            Viewer.TextMarkupAnnotationChanged += Viewer_TextMarkupAnnotationChanged;
            Viewer.StampAnnotationChanged += Viewer_StampAnnotationChanged;
            Viewer.StickyNoteAnnotationChanged += Viewer_StickyNoteAnnotationChanged;
            Viewer.HandwrittenSignatureChanged += Viewer_HandwrittenSignatureChanged;
            
        }

        private void Viewer_HandwrittenSignatureChanged(object sender, Syncfusion.Windows.PdfViewer.HandwrittenSignatureChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void Viewer_StickyNoteAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.StickyNoteAnnotationChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void Viewer_StampAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.StampAnnotationChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void Viewer_TextMarkupAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.TextMarkupAnnotationChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void Viewer_FreeTextAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.FreeTextAnnotationChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void Viewer_InkAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.InkAnnotationChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void Viewer_ShapeAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.ShapeAnnotationChangedEventArgs e)
        {
            checkAnnotation(e);
        }

        private void checkAnnotation(AnnotationChangedEventArgs e)
        {
            if(e.Action == AnnotationChangedAction.Add
                && !_annotationNames.Contains(e.Name))
            {
                _annotationNames.Add(e.Name);
            }

        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".pdf";
            openFileDlg.Filter = "Pdf documents (.pdf)|*.pdf";
            
            if (openFileDlg.ShowDialog() == true)
            {
                fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDlg.FileName);
                Viewer.Load(openFileDlg.FileName);
            }
        }

        private void ImportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".xdfd";
            openFileDlg.Filter = "xfdf documents (.xfdf)|*.xfdf";
            if (openFileDlg.ShowDialog() == true)
            {
                Viewer.ImportAnnotations(openFileDlg.FileName, Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
            }
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            filterKahuaAnnotations();

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".xdfd";
            saveFileDialog.Filter = "xfdf documents (.xfdf)|*.xfdf";
            saveFileDialog.FileName = $"{fileName}.xfdf";

            if (!string.IsNullOrEmpty(fileName) && saveFileDialog.ShowDialog() == true)
            {
                Viewer.ExportAnnotations(saveFileDialog.FileName, Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
            }
        }

        private void filterKahuaAnnotations()
        {
            for (int p = 0; p < Viewer.PageCount; p++)
            {
                var page = Viewer.LoadedDocument.Pages[p];
                for (int a = page.Annotations.Count-1; a >= 0; a--)
                {
                    var annotation = page.Annotations[a];
                    if (_annotationNames.Contains(annotation.Name))
                    {
                        annotation.Name = $"Kahua-{annotation.Name}";
                    }

                    if (annotation.Name?.StartsWith("Kahua-") != true)
                    {
                        page.Annotations.RemoveAt(a);
                    }
                }
            }
            _annotationNames.Clear();
        }
    }
}
