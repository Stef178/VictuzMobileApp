using Microsoft.Maui.Controls;

namespace VictuzMobileApp.Controls;

public partial class IconBar : ContentView
{
    public IconBar()
    {
        InitializeComponent();

        BindingContext = new VictuzMobileApp.MVVM.ViewModel.HomePageViewModel();
    }
}
