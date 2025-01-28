using VictuzMobileApp.MVVM.Data;
using VictuzMobileApp.MVVM.Model;
using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace VictuzMobileApp.MVVM.View;

public partial class LoginPage : ContentPage

{

    private const string siteKey = "6LcNBMYqAAAAADOlaFyBM78OePnZmtYuuf74JRWA";
    private const string secretKey = "6LcNBMYqAAAAADu__4bewg0h5c3Jcir788sgzrNE";
    private string captchaToken;

    public LoginPage()
    {
        InitializeComponent();
        LoadCaptcha();
        SetupWebView();
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

    private void SetupWebView()
    {
        var androidWebView = CaptchaWebView.Handler.PlatformView as Android.Webkit.WebView;
        if (androidWebView != null)
        {
            androidWebView.SetWebViewClient(new CustomWebViewClient());
            androidWebView.Settings.JavaScriptEnabled = true;
            androidWebView.Settings.DomStorageEnabled = true;
        }
    }

    private void LoadCaptcha()
    {
        string html = $@"
        <html>
        <head>
            <script src='https://www.google.com/recaptcha/api.js'></script>
        </head>
        <body>
            <form action='#' method='POST'>
                <div class='g-recaptcha' data-sitekey='{siteKey}' data-callback='setCaptchaToken'></div>
            </form>
            <script>
                function setCaptchaToken(token) {{
                    window.location.href = 'maui://token=' + token;
                }}
            </script>
        </body>
        </html>";

        CaptchaWebView.Source = new HtmlWebViewSource { Html = html };
    }

    private void CaptchaWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (e.Url.StartsWith("maui://token="))
        {
            captchaToken = e.Url.Split('=')[1];
        }
    }

    private async void OnCaptchaSubmit(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(captchaToken))
        {
            await DisplayAlert("Fout", "Captcha niet ingevuld", "OK");
            return;
        }

        bool isValid = await VerifyCaptchaAsync(captchaToken);
        await DisplayAlert("Verificatie", isValid ? "Succesvol!" : "Mislukt!", "OK");
    }

    private async Task<bool> VerifyCaptchaAsync(string token)
    {
        using HttpClient client = new();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("secret", secretKey),
            new KeyValuePair<string, string>("response", token)
        });

        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
        string responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);
        return doc.RootElement.GetProperty("success").GetBoolean();
    }

}
