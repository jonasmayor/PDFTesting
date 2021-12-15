using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace SyncFusionViewer4._8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _workingfileWithPath = null;
        private string _fileNameWithPath = null;
        private string _downloadFile = null;
        private WebClient _webClient;
        private const string _uriPath = @"https://titan.kahua.com/markupfiles/";
        private bool _importingAnnotations = false;
        private bool _selecting = false;

        private HashSet<string> _annotationNames = new HashSet<string>();

        public MainWindow()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc0MDQ3QDMxMzkyZTMyMmUzMGtIdW1leW5Tb3UwWFNUWExYSCs5cEtTdFJmblJXYnIzMllUN0c4UWdib1k9");
            InitializeComponent();
            Viewer.RenderingEngine = PdfRenderingEngine.Pdfium;

            // Syncfusion can't hide the left side tool bar, you rather need to manually disable all functionality there
            Viewer.ThumbnailSettings.IsVisible = false;
            Viewer.IsBookmarkEnabled = false;
            // Hides the layer icon. 
            Viewer.EnableLayers = false;
            // Hides the organize page icon. 
            Viewer.PageOrganizerSettings.IsIconVisible = false;
            // Hides the redaction icon. 
            Viewer.EnableRedactionTool = false;
            // Hides the form icon. 
            Viewer.FormSettings.IsIconVisible = false;
            Viewer.CursorMode = PdfViewerCursorMode.HandTool;
            Viewer.ToolbarSettings.ShowFileTools = false;

            Viewer.ShapeAnnotationChanged += Viewer_ShapeAnnotationChanged;
            Viewer.InkAnnotationChanged += Viewer_InkAnnotationChanged;
            Viewer.FreeTextAnnotationChanged += Viewer_FreeTextAnnotationChanged;
            Viewer.TextMarkupAnnotationChanged += Viewer_TextMarkupAnnotationChanged;
            Viewer.StampAnnotationChanged += Viewer_StampAnnotationChanged;
            Viewer.StickyNoteAnnotationChanged += Viewer_StickyNoteAnnotationChanged;
            Viewer.HandwrittenSignatureChanged += Viewer_HandwrittenSignatureChanged;
            Viewer.CurrentPageChanged += Viewer_CurrentPageChanged;
            Viewer.HyperlinkClicked += Viewer_HyperlinkClicked;

        }

        private void Viewer_HyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            
        }

        private void Viewer_CurrentPageChanged(object sender, EventArgs args)
        {
            
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
            if (e.Action == AnnotationChangedAction.Select)
            {
                if (_shouldLock && !e.Settings.IsLocked)
                    e.Settings.IsLocked = true;
            }
            checkAnnotation(e);
        }

        private void Viewer_TextMarkupAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.TextMarkupAnnotationChangedEventArgs e)
        {
            if (e.Action == AnnotationChangedAction.Select)
            {
                if (_shouldLock && !e.Settings.IsLocked)
                    e.Settings.IsLocked = true;
            }
            checkAnnotation(e);
        }

        private void Viewer_FreeTextAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.FreeTextAnnotationChangedEventArgs e)
        {
            if (e.Action == AnnotationChangedAction.Select)
            {
                if (_shouldLock && !e.Settings.IsLocked)
                    e.Settings.IsLocked = true;
            }
            checkAnnotation(e);
        }

        private void Viewer_InkAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.InkAnnotationChangedEventArgs e)
        {
            if (e.Action == AnnotationChangedAction.Select)
            {
                if (_shouldLock && !e.Settings.IsLocked)
                    e.Settings.IsLocked = true;
            }
            checkAnnotation(e);
        }

        private void Viewer_ShapeAnnotationChanged(object sender, Syncfusion.Windows.PdfViewer.ShapeAnnotationChangedEventArgs e)
        {
            if (e.Action == AnnotationChangedAction.Select)
            {
                if (_shouldLock && !e.Settings.IsLocked)
                    e.Settings.IsLocked = true;
            }
            if(e.Action == AnnotationChangedAction.Resize)
            {
                MessageBox.Show("Trying to resize");
            }
            checkAnnotation(e);
            
        }

        private void checkAnnotation(AnnotationChangedEventArgs e)
        {
            if (!_importingAnnotations && e.Action == AnnotationChangedAction.Add
                && !_annotationNames.Contains(e.Name))
            {
                _annotationNames.Add(e.Name);
            }
            else if (e.Action == AnnotationChangedAction.Select)
            {
                NameId.Text = e.Name;
                if(_selecting)
                {
                    Viewer.GotoPage(e.PageNumber);
                }

            }
            else if (e.Action == AnnotationChangedAction.Deselect)
                NameId.Text = string.Empty;


        }

        private void OpenPdfClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            _workingfileWithPath = menuItem.Tag.ToString();
            _fileNameWithPath = System.IO.Path.GetFileNameWithoutExtension(_workingfileWithPath);
            openPDf();
        }

        private void OpenPdfPasiveClick(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            _workingfileWithPath = menuItem.Tag.ToString();
            _fileNameWithPath = System.IO.Path.GetFileNameWithoutExtension(_workingfileWithPath);
            openPdfPasive();
        }


        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".pdf";
            openFileDlg.Filter = "Pdf documents (.pdf)|*.pdf";

            if (openFileDlg.ShowDialog() == true)
            {
                _workingfileWithPath = openFileDlg.FileName;
                _fileNameWithPath = System.IO.Path.GetFileNameWithoutExtension(_workingfileWithPath);
                openPDf();

            }
        }

        private void openPDf()
        {
            var xfdf = _workingfileWithPath.Replace(".pdf", ".xfdf");
            if (File.Exists(xfdf))
                Viewer.DocumentLoaded += Viewer_DocumentLoaded1;
            Viewer.Load(_workingfileWithPath);
        }

        private void OpenMenuItemPasive_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".pdf";
            openFileDlg.Filter = "Pdf documents (.pdf)|*.pdf";

            if (openFileDlg.ShowDialog() == true)
            {
                _workingfileWithPath = openFileDlg.FileName;
                _fileNameWithPath = System.IO.Path.GetFileNameWithoutExtension(_workingfileWithPath);
                Viewer.Load(_workingfileWithPath); 
                //openPdfPasive();

            }
        }

        private void openPdfPasive()
        {
            var xfdf = _workingfileWithPath.Replace(".pdf", ".xfdf");
            if (File.Exists(xfdf))
                Viewer.DocumentLoaded += Viewer_DocumentLoaded1Async;
            Viewer.Load(_workingfileWithPath);
        }

        private void Viewer_DocumentLoaded1(object sender, EventArgs args)
        {
            Viewer.DocumentLoaded += Viewer_DocumentLoaded1;
            var xfdf = _workingfileWithPath.Replace(".pdf", ".xfdf");
            loadXfdf(xfdf);
        }

        private async void Viewer_DocumentLoaded1Async(object sender, EventArgs args)
        {
            Viewer.DocumentLoaded += Viewer_DocumentLoaded1;
            await Task.Delay(4000);
            var xfdf = _workingfileWithPath.Replace(".pdf", ".xfdf");
            loadXfdf(xfdf);
        }

        private void ImportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".xdfd";
            openFileDlg.Filter = "xfdf documents (.xfdf)|*.xfdf";
            if (openFileDlg.ShowDialog() == true)
            {
                loadXfdf(openFileDlg.FileName);
            }
        }

        private void loadXfdf(string file)
        {
            if (!File.Exists(file))
                return;
            var xfdf = System.IO.File.ReadAllText(file);
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(xfdf);
                    writer.Flush();
                    stream.Position = 0;
                    try
                    {
                        _importingAnnotations = true;
                        Viewer.ImportAnnotations(stream, Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        _importingAnnotations = false;
                    }
                }
            }
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            filterKahuaAnnotations();

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".xdfd";
            saveFileDialog.Filter = "xfdf documents (.xfdf)|*.xfdf";
            saveFileDialog.FileName = $"{_fileNameWithPath}.xfdf";

            if (!string.IsNullOrEmpty(_fileNameWithPath) && saveFileDialog.ShowDialog() == true)
            {
                Viewer.ExportAnnotations(saveFileDialog.FileName, Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
            }
        }

        private void filterKahuaAnnotations()
        {
            for (int p = 0; p < Viewer.PageCount; p++)
            {
                var page = Viewer.LoadedDocument.Pages[p];
                for (int a = page.Annotations.Count - 1; a >= 0; a--)
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

        private void PreLoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            _downloadFile = menuItem.Tag.ToString();
            _fileNameWithPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _downloadFile);
            if (File.Exists($"{_fileNameWithPath}.pdf"))
                loadWithXfdf();
            else
            {
                if (_webClient == null)
                {
                    _webClient = new WebClient();
                    _webClient.DownloadDataCompleted += _webClient_DownloadDataCompleted;
                }

                if (!_webClient.IsBusy)
                {
                    _webClient.DownloadDataAsync(new Uri($"{_uriPath}/{_downloadFile}.pdf"));
                }
            }
        }

        private void _webClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

            File.WriteAllBytes($"{_fileNameWithPath}.pdf", e.Result);
            _webClient = null;
            Dispatcher.Invoke(() => loadWithXfdf());
        }

        private void loadWithXfdf()
        {
            if (!File.Exists($"{_fileNameWithPath}.xfdf"))
            {
                if (_webClient == null)
                {
                    _webClient = new WebClient();
                    _webClient.DownloadDataCompleted += _webClient2_DownloadDataCompleted;
                }

                if (!_webClient.IsBusy)
                {
                    _webClient.DownloadDataAsync(new Uri($"{_uriPath}/{_downloadFile}.xml"));
                }
            }
            else
            {
                loadPredefined();
            }
        }

        private void _webClient2_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

            File.WriteAllBytes($"{_fileNameWithPath}.xfdf", e.Result);
            _webClient = null;
            Dispatcher.Invoke(() =>
            {
                loadPredefined();
            });
        }

        private void loadPredefined()
        {
            Viewer.DocumentLoaded += Viewer_DocumentLoaded;
            Viewer.Load($"{_fileNameWithPath}.pdf");
        }

        private void Viewer_DocumentLoaded(object sender, EventArgs args)
        {
            Viewer.DocumentLoaded -= Viewer_DocumentLoaded;
            try
            {
                _importingAnnotations = true;
                Viewer.ImportAnnotations($"{_fileNameWithPath}.xfdf", Syncfusion.Pdf.Parsing.AnnotationDataFormat.XFdf);
            }
            finally
            {
                _importingAnnotations = false;
            }
            _downloadFile = null;
        }

        private void PageRotation_Click(object sender, RoutedEventArgs e)
        {
            var RotationAngle = Viewer.PageOrganizer.GetPageRotation(Viewer.CurrentPageIndex);
            MessageBox.Show(RotationAngle.ToString());
        }

        private void Arrow_Click(object sender, RoutedEventArgs e)
        {
            if (Viewer.AnnotationCommand.CanExecute("Arrow"))
                Viewer.AnnotationCommand.Execute("Arrow");
            
        }

        private void RotateClockwise_Click(object sender, RoutedEventArgs e)
        {
            Viewer.PageOrganizer.RotateClockwise(new[] { Viewer.CurrentPageIndex });
        }

        private void RotateCounterClockwise_Click(object sender, RoutedEventArgs e)
        {
            Viewer.PageOrganizer.RotateCounterclockwise(new[] { Viewer.CurrentPageIndex });
        }

        private void RotateClockwiseCommand_Click(object sender, RoutedEventArgs e)
        {
            Viewer.PageOrganizer.RotatePagesClockwiseCommand.Execute(new[] { Viewer.CurrentPageIndex });
        }

        private void RotateCounterClockwiseCommand_Click(object sender, RoutedEventArgs e)
        {
            Viewer.PageOrganizer.RotatePagesCounterclockwiseCommand.Execute(new[] { Viewer.CurrentPageIndex });
        }

        private void SelectAnnotation_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(NameId.Text))
            {
                try
                {
                    _selecting = true;
                    Viewer.SelectAnnotation(NameId.Text);
                }
                finally
                {
                    _selecting = false;
                }
            }
        }

        private bool _shouldLock = false;
        private void LockMenu_Click(object sender, RoutedEventArgs e)
        {
            _shouldLock = !_shouldLock;
        }

        private void OpenAndLock_Click(object sender, RoutedEventArgs e)
        {
            _workingfileWithPath = "../../../Pdf Libraries/Single Stamp.pdf";
            _fileNameWithPath = System.IO.Path.GetFileNameWithoutExtension(_workingfileWithPath);

            openPDf();
            _shouldLock = true;
           
        }
    }
}
