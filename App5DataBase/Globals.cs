using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App5DataBase
{
   public class Globals
    {
        public static string RootDirectory
        {
            get
            {
                string _finalRoot = String.Empty;

                var storageState = Android.OS.Environment.ExternalStorageState;

                string contextRoot = null;

                if (storageState == Android.OS.Environment.MediaMounted)
                    contextRoot = Application.Context.GetExternalFilesDir(null).ToString();

                var envRoot = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

                if (String.IsNullOrEmpty(contextRoot))
                    return envRoot;

                var splittedEnvRoot = envRoot.Split(Java.IO.File.SeparatorChar);
                var splittedContextRoot = contextRoot.Split(Java.IO.File.SeparatorChar);
                _finalRoot = envRoot;
                if (splittedEnvRoot.Length > 0 && splittedContextRoot.Length >= splittedEnvRoot.Length)
                {
                    for (var i = 0; i < splittedEnvRoot.Length; i++)
                    {
                        splittedEnvRoot[i] = splittedContextRoot[i];
                    }

                    _finalRoot = String.Join(Java.IO.File.Separator, splittedEnvRoot);
                }

                return _finalRoot;
            }
        }

    }
}