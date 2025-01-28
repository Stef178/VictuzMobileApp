using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Webkit;

namespace VictuzMobileApp.Platforms.Android
{
    public class CustomWebViewClient : WebViewClient
    {
        public override void OnPageFinished(WebView view, string url)
        {
            view.Settings.JavaScriptEnabled = true;
            view.Settings.DomStorageEnabled = true;
            base.OnPageFinished(view, url);
        }
    }
}
