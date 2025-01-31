namespace VictuzMobileApp.MVVM.View;

public partial class SettingsPage : ContentPage
{
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
    }
}