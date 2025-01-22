using Microsoft.Maui.Controls;
using VictuzMobileApp.MVVM.View;

namespace VictuzMobileApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // NavigationPage instellen
        MainPage = new NavigationPage(new StartPage());
        //MainPage = new NavigationPage(new VictuzMobileApp.MVVM.View.HomePage());
    }
}
