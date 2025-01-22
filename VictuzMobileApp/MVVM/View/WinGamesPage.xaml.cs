using System;
using Microsoft.Maui.Controls;

namespace VictuzMobileApp.MVVM.View
{
    public partial class WinGamesPage : ContentPage
    {
        // Mogelijke symbolen
        private readonly string[] _symbols = { "win.png", "loss.png", "retry.png" };
        private readonly Random _random = new();

        public WinGamesPage()
        {
            InitializeComponent();
        }

        private void OnSpinButtonClicked(object sender, EventArgs e)
        {
            // Willekeurig selecteer iconen voor de drie slots
            Slot1.Source = _symbols[_random.Next(_symbols.Length)];
            Slot2.Source = _symbols[_random.Next(_symbols.Length)];
            Slot3.Source = _symbols[_random.Next(_symbols.Length)];

            // Optioneel: Voeg een win-logica toe
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (Slot1.Source.ToString() == Slot2.Source.ToString() && Slot2.Source.ToString() == Slot3.Source.ToString())
            {
                DisplayAlert("Gefeliciteerd!", "Je hebt gewonnen!", "OK");
            }
            else if (Slot1.Source.ToString().Contains("retry") &&
                     Slot2.Source.ToString().Contains("retry") &&
                     Slot3.Source.ToString().Contains("retry"))
            {
                DisplayAlert("Opnieuw proberen!", "Je mag nog een keer spinnen.", "OK");
            }
        }
    }
}
