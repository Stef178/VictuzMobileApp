using System.Collections.ObjectModel;
using VictuzMobileApp.MVVM.Model;
using System;
using System.Net.Http;
using Microsoft.Maui.Controls;



namespace VictuzMobileApp.MVVM.View;

public partial class WalletPage : ContentPage

{
    public ObservableCollection<Ticket> ReservedTickets { get; set; } = new ObservableCollection<Ticket>();

    private readonly HttpClient _httpClient = new();


    public WalletPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadTickets();
    }

    private async void LoadTickets()
    {
        if (App.CurrentUser == null) return;

        ReservedTickets.Clear();
        var tickets = await App.Database.GetAllAsync<Ticket>();
        var userTickets = tickets.Where(t => t.ParticipantId == App.CurrentUser.Id).ToList();

        foreach (var ticket in userTickets)
        {
            ticket.Activity = await App.Database.GetAsync<Activity>(ticket.ActivityId);
            ReservedTickets.Add(ticket);
        }
    }

    private async void OnQrClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShowQr());
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is Ticket ticket)
        {
            bool confirm = await DisplayAlert("Bevestigen", "Weet je zeker dat je dit ticket wilt verwijderen?", "Ja", "Nee");
            if (!confirm) return;

            try
            {

                await App.Database.DeleteAsync(ticket);


                ReservedTickets.Remove(ticket);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", $"Kan ticket niet verwijderen: {ex.Message}", "OK");
            }
        }
    }
}