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

            var selectedActivity = (Activity)((Button)sender).BindingContext;

            if (selectedActivity == null)
            {
                await DisplayAlert("Fout", "Er zijn geen evenementen om te reserveren.", "OK");
                return;
            }

            
            if (selectedActivity.Participants == null)
            {
                selectedActivity.Participants = new List<Participant>();
            }

            
            if (selectedActivity.Participants.Count >= selectedActivity.MaxParticipants)
            {
                await DisplayAlert("Vol", "Dit evenement is volgeboekt.", "OK");
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
            selectedActivity.Participants.Add(App.CurrentUser); 
            await App.Database.UpdateAsync(selectedActivity); 

            await DisplayAlert("Succes", "Uw ticket is gereserveerd en toegevoegd aan uw Wallet!", "OK");
        }


        private async void OnDetailsButtonClicked(object sender, EventArgs e)
        {
            
            var button = (Button)sender;
            var activity = (Activity)button.BindingContext;

            
            await Navigation.PushAsync(new ActivityDetailPage(activity));
        }
    }
}
