using System;
using VictuzMobileApp.MVVM.Model;
using Vonage;
using Vonage.Request;
using Vonage.Verify;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System.Net;

namespace VictuzMobileApp.MVVM.View
{
    public partial class RegisterPage : ContentPage
    {
        private string _requestId;
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnAccountAanmakenClicked(object sender, EventArgs e)
        {
            string name = UsernameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string phonenumber = PhoneNumberEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Fout", "Alle velden moeten worden ingevuld.", "OK");
                return;
            }

            var participant = new Participant
            {
                Name = name,
                Email = email,
                PhoneNumber = phonenumber,
                Password = password

            };

            await Verify();

            //try
            //{
            //    await App.Database.AddAsync(participant);

            //    await DisplayAlert("Succes", "Account aangemaakt!", "OK");

            //    await Navigation.PushAsync(new LoginPage());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error saving participant: {ex.Message}");
            //    await DisplayAlert("Fout", "Er is iets misgegaan bij het aanmaken van het account.", "OK");
            //}

        }

        private async Task Verify()
        {
            string phoneNumber = PhoneNumberEntry.Text;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                await DisplayAlert("Fout", "Voer een geldig telefoonnummer in.", "OK");
                return;
            }

            var credentials = Credentials.FromApiKeyAndSecret("12a72374", "v91yY0OR3U3bxnMT");
            var client = new VonageClient(credentials);

            var request = new VerifyRequest
            {
                Number = phoneNumber,
                Brand = "MauiAuthApp",
                CodeLength = 4,
                SenderId = "Vonage",
            };




            try
            {
                var response = await client.VerifyClient.VerifyRequestAsync(request);

                if (response.Status == "0")
                {
                    _requestId = response.RequestId; // Sla het request ID op
                    await DisplayAlert("Succes", "Verificatiecode is verzonden!", "OK");

                    // Direct vragen om de verificatiecode
                    await AskForVerificationCode();
                }
                else
                {
                    await DisplayAlert("Fout", "Kon geen code verzenden: " + response.ErrorText, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", "Er ging iets mis: " + ex.Message, "OK");
            }
        }


        private async Task AskForVerificationCode()
        {
            string code = await DisplayPromptAsync("Verificatie", "Voer de code in die je per sms hebt ontvangen:",
                                                   accept: "Verifiëren", cancel: "Annuleren",
                                                   placeholder: "123456", keyboard: Keyboard.Numeric);

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(_requestId))
            {
                await DisplayAlert("Fout", "Voer een geldige code in.", "OK");
                return;
            }

            await VerifyCode(code);
        }


        private async Task VerifyCode(string code)
        {
            var credentials = Credentials.FromApiKeyAndSecret("12a72374", "v91yY0OR3U3bxnMT");
            var client = new VonageClient(credentials);

            var request = new VerifyCheckRequest { RequestId = _requestId, Code = code };

            try
            {
                var response = await client.VerifyClient.VerifyCheckAsync(request);

                if (response.Status == "0")
                {
                    await DisplayAlert("Succes", "Code is correct! Inloggen voltooid.", "OK");
                    await DisplayAlert("Succes", "Account aangemaakt!", "OK");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Fout", "Verificatie mislukt: " + response.ErrorText, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", "Er ging iets mis: " + ex.Message, "OK");
            }
        }


    }
}
