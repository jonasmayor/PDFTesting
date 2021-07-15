using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SavePDFUsingNative
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDcwODQxQDMxMzkyZTMyMmUzMFVaWnNjdDl5Mm9XN2hCNXFJajJkbzVXSC8vZ0ZKZFJtT2I5K2pSdzBsYlE9");
			MainPage = new NavigationPage( new SavePDFUsingNative.MainPage());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
