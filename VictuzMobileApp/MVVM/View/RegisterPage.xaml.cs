namespace VictuzMobileApp.MVVM.View;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        // Handle registration logic here
        await DisplayAlert("Succes", "Account aangemaakt!", "OK");
        await Navigation.PushAsync(new LoginPage());
    }
}