using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace VictuzMobileApp.MVVM.View
{
    public class PhotoModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
    }

    public class DatabaseService
    {
        private SQLiteConnection _connection;

        // Pad naar de database (in de lokale appmap)
        public DatabaseService()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "photos.db3");
            _connection = new SQLiteConnection(databasePath);
            _connection.CreateTable<PhotoModel>(); // Maak de tabel aan als deze nog niet bestaat
        }

        // Voeg een foto toe aan de database
        public void AddPhoto(PhotoModel photo)
        {
            _connection.Insert(photo);
        }

        // Haal alle foto's op uit de database
        public List<PhotoModel> GetAllPhotos()
        {
            return _connection.Table<PhotoModel>().ToList();
        }
    }
}

