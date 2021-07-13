using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Android.Support.V4.App;
using Android;
using AndroidX.Core.Content;
using AndroidX.Core.App;
//using Android.Support.V4.Content;

namespace SavePDFUsingNative.Droid
{
    [Activity(Label = "SavePDFUsingNative", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private int PERMISSION_REQUEST_CODE = 1;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!PermissionGrantedForExternalStorage)
                {
                    // Code for above or equal 23 API Oriented Device 
                    // Your Permission is not granted already, so request the permission to access external storage to save the files
                    RequestPermission();
                }
            }
        }

        /// <summary>
        /// Check whether this application has permission to access the external storage
        /// </summary>
        private bool PermissionGrantedForExternalStorage
        {
            get
            {
                Permission permissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage);
                if (permissionResult == Permission.Granted)
                {
                    // if permission is already granted return true otherwise return false
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Request to enable permission to write the files on external storage of android device
        /// </summary>
        private void RequestPermission()
        {
            if (!ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, PERMISSION_REQUEST_CODE);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

