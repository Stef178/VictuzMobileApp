namespace VictuzMobileApp.MVVM.View;


public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = new VictuzMobileApp.MVVM.ViewModel.ProfileViewModel();
    }
}
