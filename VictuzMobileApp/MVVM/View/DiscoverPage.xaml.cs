using VictuzMobileApp.MVVM.Model;
using System.Linq;

namespace VictuzMobileApp.MVVM.View
{
    public partial class DiscoverPage : ContentPage
    {
        public List<Activity> AllEvents { get; set; }
        public List<Activity> UpcomingEvents { get; set; }

        public DiscoverPage()
        {
            InitializeComponent();
            LoadAllEvents();
        }

        private async void LoadAllEvents()
        {
            try
            {
                var activities = await App.Database.GetAllAsync<Activity>();

                AllEvents = activities.OrderBy(a => a.StartTime).ToList();

                // Filter upcoming events (events with start time in the future)
                UpcomingEvents = AllEvents.Where(a => a.StartTime > DateTime.Now).ToList();

                BindingContext = this;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading events: {ex.Message}");
                await DisplayAlert("Error", "Er is een fout opgetreden bij het laden van de evenementen.", "OK");
            }
        }

        private async void OnReserveTicketClicked(object sender, EventArgs e)
        {
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Fout", "U moet ingelogd zijn om een ticket te reserveren.", "OK");
                return;
            }

            var selectedActivity = (Activity)((Button)sender).BindingContext; // Haal de geselecteerde activiteit op

            if (selectedActivity == null)
            {
                await DisplayAlert("Fout", "Er zijn geen evenementen om te reserveren.", "OK");
                return;
            }

            // Controleer of de Participants collectie geïnitieerd is
            if (selectedActivity.Participants == null)
            {
                selectedActivity.Participants = new List<Participant>();
            }

            // Check of het evenement al vol is
            if (selectedActivity.Participants.Count >= selectedActivity.MaxParticipants)
            {
                await DisplayAlert("Vol", "Dit evenement is volgeboekt.", "OK");
                return;
            }

            var ticket = new Ticket
            {
                ParticipantId = App.CurrentUser.Id,
                ActivityId = selectedActivity.Id,
                Price = 0, // Pas dit aan als er een prijs is
                IsPaid = false
            };

            await App.Database.AddAsync(ticket);
            selectedActivity.Participants.Add(App.CurrentUser); // Voeg deelnemer toe aan de activiteit
            await App.Database.UpdateAsync(selectedActivity); // Update de activiteit met de nieuwe deelnemer

            await DisplayAlert("Succes", "Uw ticket is gereserveerd en toegevoegd aan uw Wallet!", "OK");
        }


        private async void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            // Haal de geselecteerde activiteit op
            var button = (Button)sender;
            var activity = (Activity)button.BindingContext;

            // Navigeer naar de detailpagina
            await Navigation.PushAsync(new ActivityDetailPage(activity));
        }
    }
}
