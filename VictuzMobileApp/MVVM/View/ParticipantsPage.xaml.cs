using SQLite;
using VictuzMobileApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace VictuzMobileApp.MVVM.View
{
	public partial class ParticipantsPage : ContentPage
	{
		private string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db");
		public ObservableCollection<Participant> Participants { get; set; }

		public ParticipantsPage()
		{
			InitializeComponent();
			Participants = new ObservableCollection<Participant>();
			BindingContext = this;

			LoadParticipants();
		}

		private void LoadParticipants()
		{
			using (var db = new SQLiteConnection(_dbPath))
			{
				db.CreateTable<Participant>();
				var participantsFromDb = db.Table<Participant>().ToList();

				Participants.Clear();
				foreach (var participant in participantsFromDb)
				{
					Participants.Add(participant);
				}

				Console.WriteLine($"Aantal deelnemers geladen: {Participants.Count}");  
			}
		}


		private async void OnDeleteButtonClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			var participant = button?.BindingContext as Participant;

			if (participant != null)
			{
				bool confirm = await DisplayAlert("Bevestigen", $"Weet je zeker dat je {participant.Name} wilt verwijderen?", "Ja", "Nee");

				if (confirm)
				{
					using (var db = new SQLiteConnection(_dbPath))
					{
						db.CreateTable<Participant>();
						db.Delete(participant);  
					}

					Participants.Remove(participant);

					await DisplayAlert("Verwijderd", $"{participant.Name} is succesvol verwijderd.", "OK");
				}
			}
		}

		private void OnParticipantSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var selectedParticipant = e.SelectedItem as Participant;

			if (selectedParticipant != null)
			{
			}
		}
	}
}
