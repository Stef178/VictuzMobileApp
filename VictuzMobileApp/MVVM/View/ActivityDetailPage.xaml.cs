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

	private async void OnReserveTicketClicked(object sender, EventArgs e)
	{
		if (App.CurrentUser == null)
		{
			await DisplayAlert("Fout", "U moet ingelogd zijn om een ticket te reserveren.", "OK");
			return;
		}

		if (BindingContext is not Activity selectedActivity)
		{
			await DisplayAlert("Fout", "Er is een probleem opgetreden bij het laden van de activiteit.", "OK");
			return;
		}

		var ticket = new Ticket
		{
			ParticipantId = App.CurrentUser.Id,
			ActivityId = selectedActivity.Id,
			Price = 0,
			IsPaid = false
		};

		await App.Database.AddAsync(ticket);

		await DisplayAlert("Succes", "Uw ticket is gereserveerd en toegevoegd aan uw Wallet!", "OK");
	}


}
