namespace VictuzMobileApp.MVVM.View;


public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();

        if (App.CurrentUser == null)
        {
            DisplayAlert("Fout", "Geen gebruiker gevonden", "OK");
            return;
        }

        BindingContext = new VictuzMobileApp.MVVM.ViewModel.ProfileViewModel();
    }
}
