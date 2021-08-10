using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Syncfusion.Pdf.Parsing;
using Syncfusion.SfPdfViewer.XForms;
using Xamarin.Forms;

namespace SavePDFUsingNative
{
    public partial class KahuaViewerPage : ContentPage
    {
        private WebClient _webclient;

        public KahuaViewerPage()
        {
            InitializeComponent();
            _webclient = new WebClient();
        }

        async void Button1_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(1);
        }

        async void Button2_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(2);
        }

        async void Button3_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(3);
        }

        async void Button4_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(4);
        }

        async void Button5_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(5);
        }

        async void Button6_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(6);
        }

        async void Button7_Clicked(System.Object sender, System.EventArgs e)
        {
            await loadDocumentData(7);
        }

        private async Task loadDocumentData(int fileNum)
        {
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading(fileNum == 7 ? "Loading a HUGE file..." : string.Empty);
            var stream = await getPDFUrl(fileNum);
            var xfdfStream = await getAnnotationDataUrl(fileNum);
            loadFileViewerPage(stream, xfdfStream, getTitle(fileNum));
            Acr.UserDialogs.UserDialogs.Instance.HideLoading();
        }

        private void loadFileViewerPage(byte[] stream, byte[] xfdfStream, string title)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var fileViewerPage = new FileViewerPage(new MemoryStream(stream), new MemoryStream(xfdfStream));
                    fileViewerPage.Title = title;
                    Navigation.PushAsync(fileViewerPage);
                }
                catch (Exception ex)
                {

                }
            });
        }

        private async Task<byte[]> getPDFUrl(int v)
        {
            var url = string.Empty;
            var webAddress = "https://titan.kahua.com/markupfiles/";
            url = $"{webAddress}{getTitle(v)}.pdf";

            return await _webclient.DownloadDataTaskAsync(new Uri(url));
        }

        private async Task<byte[]> getAnnotationDataUrl(int v)
        {
            var url = string.Empty;
            var webAddress = "https://titan.kahua.com/markupfiles/";
            url = $"{webAddress}{getTitle(v)}.xml";

            return await _webclient.DownloadDataTaskAsync(new Uri(url));
        }

        private string getTitle(int num)
        {
            var title = string.Empty;
            switch (num)
            {
                case 1:
                    title = $"01-SinglePage-Vector";
                    break;
                case 2:
                    title = $"02-SinglePage-Raster";
                    break;
                case 3:
                    title = $"03-MultiPage-Vector";
                    break;
                case 4:
                    title = $"04-MultiPage-Raster";
                    break;
                case 5:
                    title = $"05-SinglePage-Vector-Rotated";
                    break;
                case 6:
                    title = $"06-SinglePage-Raster-Rotated";
                    break;
                case 7:
                    title = $"BigDocument";
                    break;
            }
            return title;
        }
    }
}
