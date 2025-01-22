namespace VictuzMobileApp.MVVM.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        // Handle login logic here
        await Navigation.PushAsync(new HomePage());
    }
}