namespace VictuzMobileApp.MVVM.View;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnProfileButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnDiscoverButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DiscoverPage());
    }
}