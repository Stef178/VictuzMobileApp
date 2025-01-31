using System.Net.Http;
using System.Threading.Tasks;
using VictuzMobileApp.MVVM.Data;
using VictuzMobileApp.MVVM.Model;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;

namespace VictuzMobileApp.MVVM.View
{
    public partial class LoginPage : ContentPage
    {
        private const string SecretKey = "6LdATMgqAAAAALQlY-u-DShEK0vL4RBKAplPVEST"; // Vervang met je echte Secret Key

        private string _captchaToken;

        public LoginPage()
        {
            InitializeComponent();
            LoadCaptcha();
        }

        private void LoadCaptcha()
        {
            CaptchaWebView.Navigating += CaptchaWebView_Navigating;
            CaptchaWebView.Source = new HtmlWebViewSource
            {
                Html = @"<!DOCTYPE html>
                        <html>
                        <head>
                            <script src='https://www.google.com/recaptcha/api.js'></script>
                        </head>
                        <body>
                            <form>
                                <div class='g-recaptcha' data-sitekey='6LfL1ccqAAAAAJzeedg9bcA5IfTV6g1J2-QImiJR' data-callback='onSubmit'></div>
                            </form>
                            <script>
                                function onSubmit(token) {
                                    window.location.href = 'callback://' + token;
                                }
                            </script>
                        </body>
                        </html>"
            };
        }



        private async void CaptchaWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith("callback://"))
            {
                e.Cancel = true; // Dit werkt nu correct!

                _captchaToken = e.Url.Replace("callback://", "");
                bool isValid = await ValidateCaptchaAsync(_captchaToken);

                if (isValid)
                {
                    await DisplayAlert("Succes", "Captcha gevalideerd!", "OK");
                    LoginButton.IsEnabled = true;
                }
                else
                {
                    await DisplayAlert("Fout", "Captcha ongeldig, probeer opnieuw.", "OK");
                }
            }
        }

        private async Task<bool> ValidateCaptchaAsync(string token)
        {
            using HttpClient client = new HttpClient();
            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", SecretKey),
                    new KeyValuePair<string, string>("response", token)
                })
            );

            var json = await response.Content.ReadAsStringAsync();
            return json.Contains("\"success\": true");
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Fout", "Vul alstublieft zowel e-mailadres als wachtwoord in.", "OK");
                return;
            }

            try
            {
                if (email == App.AdminUser.Email && password == App.AdminUser.Password)
                {
                    App.CurrentUser = App.AdminUser;
                    await DisplayAlert("Succes", "Admin succesvol ingelogd!", "OK");
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                    return;
                }

                var users = await App.Database.GetAllAsync<Participant>();
                var matchingUser = users.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (matchingUser != null)
                {
                    await App.Database.SetActiveUser(matchingUser.Id);
                    App.CurrentUser = matchingUser;
                    await DisplayAlert("Succes", "U bent succesvol ingelogd!", "OK");

                    Application.Current.MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    await DisplayAlert("Fout", "Onjuist e-mailadres of wachtwoord. Probeer het opnieuw.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", $"Er is iets misgegaan: {ex.Message}", "OK");
            }
        }
    }
}
