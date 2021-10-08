using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
//using kahua.ktree.utility;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace kahua.host.uno.common.pdf
{
    public sealed partial class PdfEditor : UserControl
    {
        ObservableCollection<string> paths = new ObservableCollection<string>();

        private PdfWebControl _control;

        public PdfEditor()
        {
            this.InitializeComponent();

            this.Loaded += PdfEditor_Loaded;
            PathsCombo.SelectionChanged += PathsCombo_SelectionChanged;
        }

        private void PathsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Import.IsEnabled = !string.IsNullOrEmpty(PathsCombo.SelectedItem as string);
        }

        private void PdfEditor_Loaded(object sender, RoutedEventArgs e)
        {
            TryLoadDocument();
            loadPaths();
        }

        private void loadPaths()
        {
            paths.Add("");
            paths.Add("01-SinglePage-Vector - No freetext or stamp.xfdf");
            paths.Add("01-SinglePage-Vector.xfdf");
            paths.Add("02-SinglePage-Raster.xfdf");
            paths.Add("02-SinglePage-Raster-Fresh-export.xfdf");
            paths.Add("03-MultiPage-Vector.xfdf");
            paths.Add("03-MultiPage-Vector_markups.xfdf");
            paths.Add("04-MultiPage-Raster.xfdf");
            paths.Add("05-SinglePage-Vector-Rotated.xfdf");
            paths.Add("06-SinglePage-Raster-Rotated.xfdf");
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            _control.ImportXfdf(PathsCombo.SelectedItem as string);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            _control.ExportXfdf();
        }

        private void Big_Load_Click(object sender, RoutedEventArgs e)
        {
            load(true);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            load();
        }

        void load(bool bigDocument = false)
        {

#if RELEASE
            DownloadsTextBox.Text = "c:/websites/com.kahua.titan/markupfiles/xfdf/XFDF for web";
#else
            DownloadsTextBox.Text = "c:/users/blindsey/downloads";
#endif
            var loaded = _control.LoadPdfViewer(DownloadsTextBox.Text.Replace("\\", "/"), bigDocument);
            if (loaded)
            {
                Load.IsEnabled = false;
                LoadBig.IsEnabled = false;
                Import.IsEnabled = true;
                Export.IsEnabled = true;
                PathsCombo.IsEnabled = true;
            }
        }

#if __WASM__
        private Task TryLoadDocument()
        {
            if (RootGrid.Children.Count > 0)
            {
                RootGrid.Children.RemoveAt(0);
            }

            _control = new PdfWebControl();
            RootGrid.MinHeight = 600;
            RootGrid.Children.Add(_control);

            return Task.CompletedTask;
        }
#else
        private Task TryLoadDocument()
        {
            return Task.CompletedTask;
        }
#endif
    }
}