namespace VictuzMobileApp.MVVM.View;

public partial class SettingsPage : ContentPage
{
    private bool isSettings_on = false;

    public SettingsPage()
    {
        InitializeComponent();
    }

    // Methode die de knop activeert om het filter toe te voegen
    private void OnNoBlueLightButtonClicked(object sender, EventArgs e)
    {
        bool isBlueLightActivated = Preferences.Get("NoBlueLight", false);
        isBlueLightActivated = !isBlueLightActivated;

        // Sla de voorkeur op
        Preferences.Set("NoBlueLight", isBlueLightActivated);

        // Wijzig de achtergrondkleur voor de hele app
        if (isBlueLightActivated)
        {
            Application.Current.Resources["AppBackgroundColor"] = Color.FromRgb(255, 255, 180); // Geel
        }
        else
        {
            Application.Current.Resources["AppBackgroundColor"] = Colors.White; // Normale achtergrondkleur
        }

        if (isSettings_on)
        {
            BluelightButton.Source = "settings_off.png";
        }
        else
        {
            BluelightButton.Source = "settings_on.png";
        }
        isSettings_on = !isSettings_on;
    }

    private async void OnNotificationClicked(object sender, EventArgs e)
    {
        if (isSettings_on)
        {
            NotificationButton.Source = "settings_off.png";
        }
        else
        {
            NotificationButton.Source = "settings_on.png";
        }
        isSettings_on = !isSettings_on;
    }
}