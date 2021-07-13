using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Xamarin.Forms;

[assembly: Dependency(typeof(SavePDFUsingNative.UWP.AlertViewUWP))]
namespace SavePDFUsingNative.UWP
{
    class AlertViewUWP : IAlertView
    {
        public async void Show(string description)
        {
            MessageDialog dialog = new MessageDialog(description);
            await dialog.ShowAsync();
        }
    }
}
