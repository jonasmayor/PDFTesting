using Xamarin.Forms;

namespace SavePDFUsingNative
{
    public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent();
		}


        void LoadNormalButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ViewerPage("Normal"));

        }

        void LoadDrawingButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ViewerPage("Drawing"));

        }

        void LoadBigButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ViewerPage("Big"));
        }

        void KahuaTestingButton_Clicked(System.Object sender, System.EventArgs e)
        {

            Navigation.PushAsync(new KahuaViewerPage());
        }

    }
}
