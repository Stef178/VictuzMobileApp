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



}