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
//using kahua.ktree.utility;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace kahua.host.uno.common.pdf
{
    public sealed partial class PdfEditor : UserControl
    {
        private PdfWebControl _control;

        public PdfEditor()
        {
            this.InitializeComponent();

            this.Loaded += PdfEditor_Loaded;
        }

        private void PdfEditor_Loaded(object sender, RoutedEventArgs e)
        {
            TryLoadDocument();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            _control.ImportXfdf();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            _control.ExportXfdf();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
#if  RELEASE
            DownloadsTextBox.Text = "c:/websites/com.kahua.titan/markupfiles/xfdf/XFDF for web";
#else
            DownloadsTextBox.Text = "c:/users/blindsey/downloads";
#endif
            var loaded = _control.LoadPdfViewer(DownloadsTextBox.Text.Replace("\\","/"));
            if(loaded)
            {
                Load.IsEnabled = false;
                Import.IsEnabled = true;
                Export.IsEnabled = true;
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