using Microsoft.Maui.Controls;
using VictuzMobileApp.MVVM.View;
using VictuzMobileApp.MVVM.Data;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp;

public partial class App : Application
{
    public static Constants Database { get; private set; }

    // Statische property om de huidige gebruiker op te slaan
    public static Participant CurrentUser { get; set; }

    public App()
    {
        InitializeComponent();
        InitializeDatabase();

        // Hier stel je CurrentUser in (haal de actieve gebruiker uit de database)
        CurrentUser = Database.GetActiveUser(); // Zorg dat je deze methode implementeert in je Constants-klasse

        MainPage = new NavigationPage(new StartPage());
        //MainPage = new NavigationPage(new VictuzMobileApp.MVVM.View.HomePage());
    }

    private void InitializeDatabase()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db");
        Console.WriteLine(dbPath);
        Database = new Constants(dbPath);
    }
}
