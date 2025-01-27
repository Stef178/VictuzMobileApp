using Microsoft.Maui.Controls;
using VictuzMobileApp.MVVM.View;
using VictuzMobileApp.MVVM.Data;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp;

public partial class App : Application
{
    public static Constants Database { get; private set; }
    public static Participant CurrentUser { get; set; }

    public App()
    {
        InitializeComponent();
        InitializeDatabase();

        CurrentUser = Database.GetActiveUser();

        if (CurrentUser != null)
        {
            MainPage = new NavigationPage(new HomePage());
        }
        else
        {
            MainPage = new NavigationPage(new StartPage());
        }
    }

    private void InitializeDatabase()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "VictuzMobile.db");
        Database = new Constants(dbPath);
    }

    public static Participant AdminUser { get; } = new Participant
    {
        Id = -1, 
        Name = "Admin",
        Email = "admin@victuz.com",
        Password = "admin123",
        IsActive = false
    };
}
