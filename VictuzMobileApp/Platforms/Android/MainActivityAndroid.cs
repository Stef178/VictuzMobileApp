using Plugin.Media;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace VictuzMobileApp.MVVM.View
{
    [Activity(Label = "VictuzMobileApp", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivityAndroid : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Initialiseer de media plugin
            CrossMedia.Current.Initialize();
        }
    }
}
