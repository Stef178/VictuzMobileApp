namespace VictuzMobileApp.MVVM.View;
using VictuzMobileApp.MVVM.Model;


public partial class ActivityDetailPage : ContentPage
{
    public ActivityDetailPage(Activity activity)
    {
        InitializeComponent();
        BindingContext = activity;
    }

    private async void OnNavigateToEventClicked(object sender, EventArgs e)
    {
        var eventLocation = new Location(50.8808, 5.9747);

        try
        {
            
            await Map.OpenAsync(eventLocation, new MapLaunchOptions
            {
                Name = "Zuyd Hogeschool"
                
            });
        }
        catch (Exception ex)
        {
            
            await DisplayAlert("Fout", $"Er is een fout opgetreden bij het openen van de kaart: {ex.Message}", "OK");
        }
    }

}
