﻿using SQLite;
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
        public Task<T> GetAsync<T>(int id) where T : new()
        {
            return _database.FindAsync<T>(id);
        }


        public Participant GetActiveUser()
        {
            try
            {
               
                return _database.Table<Participant>().Where(p => p.IsActive).FirstOrDefaultAsync().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving active user: {ex.Message}");
                return null;
            }
        }

        public async Task SetActiveUser(int userId)
        {
            try
            {
                
                var allParticipants = await _database.Table<Participant>().ToListAsync();
                foreach (var participant in allParticipants)
                {
                    participant.IsActive = false;
                    await _database.UpdateAsync(participant);
                }

               
                var activeUser = await _database.Table<Participant>().Where(p => p.Id == userId).FirstOrDefaultAsync();
                if (activeUser != null)
                {
                    activeUser.IsActive = true;
                    await _database.UpdateAsync(activeUser);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting active user: {ex.Message}");
            }
        }
    }
}
