using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VictuzMobileApp.MVVM.Model;

namespace VictuzMobileApp.MVVM.Data
{
    public class Constants
    {
        private readonly SQLiteAsyncConnection _database;

        public Constants(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            try
            {
                // Tabellen maken

                _database.CreateTableAsync<Activity>().Wait();
                _database.CreateTableAsync<Organisor>().Wait();
                _database.CreateTableAsync<Participant>().Wait();
                _database.CreateTableAsync<Ticket>().Wait();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating tables: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
