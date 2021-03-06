using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Foundation;
using UIKit;
using System.IO;

[assembly: Dependency(typeof(SavePDFUsingNative.iOS.SaveiOS))]

namespace SavePDFUsingNative.iOS
{
    public class SaveiOS : ISave
    {
        public string Save(MemoryStream fileStream)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filepath = Path.Combine(path, "SavedDocument.pdf");

            FileStream outputFileStream = File.Open(filepath, FileMode.Create);
            fileStream.Position = 0;
            fileStream.CopyTo(outputFileStream);
            outputFileStream.Close();
            return filepath;
        }
    }
}