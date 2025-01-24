using Microsoft.Maui.Controls;
using VictuzMobileApp.MVVM.View;
using VictuzMobileApp.MVVM.Data;

namespace VictuzMobileApp;

public partial class App : Application
{
    public static Constants Database { get; private set; }
    public App()
    {
        InitializeComponent();
        InitializeDatabase();

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
