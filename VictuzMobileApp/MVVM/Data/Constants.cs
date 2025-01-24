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

				_database.CreateTableAsync<ParticipantActivity>().Wait();
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
        public Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return _database.Table<T>().ToListAsync();
        }

        public Task<int> AddAsync<T>(T item) where T : new()
        {
            return _database.InsertAsync(item);
        }

        public Task<int> UpdateAsync<T>(T item) where T : new()
        {
            return _database.UpdateAsync(item);
        }

        public Task<int> DeleteAsync<T>(T item) where T : new()
        {
            return _database.DeleteAsync(item);
        }

        public Task<int> AddAllAsync<T>(IEnumerable<T> items) where T : new()
        {
            return _database.InsertAllAsync(items);
        }
    }
}
